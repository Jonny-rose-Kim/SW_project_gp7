using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class Player_alt : MonoBehaviour
{
private float maxRightClickTime = 3f; // 최대 우 클릭 활성 시간 (초)
private float currentRightClickTime = 0f; // 현재 우 클릭 활성 시간 추적
public Rigidbody2D rigid;
private Vector3 moveDirection;
public Text hitCounterText;          // 남은 충돌 횟수를 표시할 UI 텍스트
public Text TimeRemaining;          //남은 시간을 표현할 UI 텍스트
public GameObject pausePanel;        // 일시정지 시 표시할 패널
public Text pauseText;
public Text countdownText;
private bool isPaused = false;


private Vector2 lastVelocity;
public bool wasHitted = false;
public float speedBoostDuration = 0.25f;
public Slider Accel_Bar; // 슬라이더 참조 추가
[SerializeField]public int remainingHits = 10;
[SerializeField] private float boosterSpeed = 160;
[SerializeField] private float Max = 65;
[SerializeField] private float Min = 45;
[SerializeField] public float maxSpeed = 50;
[SerializeField] private float time_limit = 180f;
[SerializeField] private Vector3 resetPosition;
[SerializeField] private float controlForce = 3f;
[SerializeField] private float  AccelLimit =3.0f;
public Transform endPoint; // 충돌한 특정 오브젝트
public float shrinkDuration = 2.0f; // 점점 작아지는 시간
public float rotationSpeed = 360f; // Z축 회전 속도 (도/초)
public float endGameDelay = 3.0f; // 게임 종료 딜레이
private bool isInteracting = false; // 상호작용 상태 플래그
public bool isGameOver = false;

public SpriteRenderer getDamaged;
void Start()
{
    getDamaged.enabled = false; // 비네팅 비활성화

    // 스테이지 번호 설정
    SetCurrentStage();

    // 초기화
    Time.timeScale = 1;
    hitCounterText.text = "Hits Left: " + remainingHits;
    TimeRemaining.text = "Time: " + time_limit;
    StartCoroutine(beReady());
}

// 현재 스테이지를 설정하는 함수
void SetCurrentStage()
{
    // 스테이지 이름으로부터 스테이지 번호 추출
    string currentSceneName = SceneManager.GetActiveScene().name;
    if (currentSceneName.Contains("Stage"))
    {
        // "Stage1", "Stage2" 같은 이름에서 숫자를 추출
        string stageNumberString = currentSceneName.Replace("Stage", "");
        if (int.TryParse(stageNumberString, out int stageNumber))
        {
            // NewStageManager에 스테이지 번호 전달
            NewStageManager stageManager = FindObjectOfType<NewStageManager>();
            if (stageManager != null)
            {
                stageManager.SetStageIndex(stageNumber);
                Debug.Log($"Set current stage to {stageNumber}");
            }
            else
            {
                Debug.LogError("NewStageManager not found!");
            }
        }
    }
}


IEnumerator beReady()
{
// 카운트다운 텍스트 활성화
countdownText.gameObject.SetActive(true);

// 플레이어 입력 및 움직임 비활성화
rigid.velocity = Vector2.zero;
enabled = false;

// 카운트다운 진행
for (int i = 3; i > 0; i--)
{
    countdownText.text = i.ToString();
    yield return new WaitForSeconds(1f);
}

// "Start!" 표시
countdownText.text = "Start!";
yield return new WaitForSeconds(1f);

// 카운트다운 종료
countdownText.gameObject.SetActive(false);
enabled = true; // 플레이어 입력 활성화

// 게임 로직 시작
GameStart();
}

void GameStart()
{
Time.timeScale = 1;
InvokeRepeating("tik_tak", 0f, 1f);
hitCounterText.text = "Hits Left: " + remainingHits;
TimeRemaining.text = "Time " + time_limit;
currentRightClickTime = 0; // 우 클릭 시간 초기화

transform.position = resetPosition;
}
void Awake()
{
    hitCounterText.text = "Hits Left: " + remainingHits;
    rigid = GetComponent<Rigidbody2D>();
}
void Update()
{
    UpdateMoveDirection();
    ApplyForceBasedOnDistance();
    if (Input.GetMouseButton(1))
    {
        if (currentRightClickTime < maxRightClickTime && !wasHitted)
        {
            currentRightClickTime += Time.deltaTime; // 우 클릭 활성 시간 업데이트
            rigid.velocity = moveDirection * boosterSpeed;
            maxSpeed = boosterSpeed;
            Accel_Bar.value = AccelLimit - currentRightClickTime;// / maxRightClickTime;
        }
    }
    else if (Input.GetMouseButtonUp(1))
    {
        if (currentRightClickTime >= maxRightClickTime)
        {
            Debug.Log("Right click time limit reached");
            // 추가적인 처리가 필요할 수 있습니다.
        }
    }
    lastVelocity = rigid.velocity;

    LimitSpeed();
}

void ResetPlayerPosition()
{
    transform.position = resetPosition;
    rigid.velocity = Vector2.zero; // 속도 초기화
    Debug.Log("BlackHole 충돌: 플레이어 위치 초기화");
}

void ApplyForceBasedOnDistance()
{
    if (isInteracting == false)
    {
        if (!Input.GetMouseButton(1)) // 마우스 우 클릭이 아닐 때만 힘 적용
        {
            float distance = Vector3.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (moveDirection.x * mousePosition.x > 0 || moveDirection.y * mousePosition.y > 0)
            {
                float appliedForce = Mathf.Clamp(distance , 0, Max); //1.4f는 변하는 속도 (Min,Max사이의 그래프기울기)
                AddForce(moveDirection * appliedForce * controlForce);
            }
            else//반대방향일때
            {
                float appliedForce = Mathf.Clamp(distance * 0.2f, 0, Max); //반대방향일때 느리게 더한다 (관성크게)
                AddForce(moveDirection * 2.0f * appliedForce * controlForce);
            }
        }

    }
}
// MoveDirection 함수: 마우스 방향으로 벡터 설정
void UpdateMoveDirection()
{
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = 0; // 2D에서 Z축 값은 0으로 고정

    // Lerp를 사용하여 부드럽게 위치를 변경
    moveDirection = Vector3.Lerp(moveDirection, (mousePosition - transform.position).normalized, 1f);
    float distance = Vector3.Distance(transform.position, mousePosition);
    maxSpeed = Mathf.Clamp(distance , Min, Max);  //m axSpeed를 현재 거리에 따라 값을 변경한다
}

// AddForce 함수: 특정 방향으로 힘을 가함
public void AddForce(Vector2 force)
{
    if (wasHitted == false)
    {
        rigid.AddForce(force, ForceMode2D.Force);
    }

}
// 최대 속도 제한 함수
void LimitSpeed()
{
    // 현재 속도가 최대 속도를 초과하면 제한
    if (rigid.velocity.magnitude > maxSpeed)
    {
        rigid.velocity = rigid.velocity.normalized * maxSpeed;
    }
}
void OnTriggerEnter2D(Collider2D other)
{
    // 태그가 "Item"인 경우 처리
    if (other.CompareTag("Item"))
    {

        // 아이템 제거
        Destroy(other.gameObject);


        // 점수 증가
        time_limit += 15;
        TimeRemaining.text = "Time " + time_limit;


        // 디버그 메시지 출력
    }
    else if (other.CompareTag("Finish"))
    {
        if (!wasHitted)
        {
            if (!isInteracting)
            {
                isInteracting = true;
                CancelInvoke("tik_tak");
                StartCoroutine(ShrinkAndRotate());
        }
    }
    }
}

void Hitted_by_Wall(Collision2D collision)
{
    Vector2 normal = collision.contacts[0].normal;
    Vector2 reflectDirection = Vector2.Reflect(lastVelocity, normal);

    if (reflectDirection.magnitude < 0.1f)
    {
        reflectDirection = reflectDirection.normalized * 2f;
    }

    rigid.velocity = reflectDirection * 0.3f;
    if (Input.GetMouseButton(1))
    {

    }
    Debug.Log($"Wall Hit! Reflect Direction: {reflectDirection}, Velocity: {rigid.velocity}");
}
void Hitted_by_Obstacle(Collision2D collision)
{
    Vector2 normal = collision.contacts[0].normal;
    Vector2 reflectDirection = Vector2.Reflect(lastVelocity, normal);

    if (reflectDirection.magnitude < 0.1f)
    {
        reflectDirection = reflectDirection.normalized * 2f;
    }

    rigid.velocity = reflectDirection * 1f;
    if (Input.GetMouseButton(1))
    {

    }
    Debug.Log($"Wall Hit! Reflect Direction: {reflectDirection}, Velocity: {rigid.velocity}");
}
void Hitted_by_Bullet(Collision2D collision)
{
    Vector2 normal = collision.contacts[0].normal;
    Vector2 reflectDirection = Vector2.Reflect(lastVelocity, normal);

    if (reflectDirection.magnitude < 0.1f)
    {
        reflectDirection = reflectDirection.normalized * 2f;
    }

    rigid.velocity = reflectDirection * 0.6f;
    if (Input.GetMouseButton(1))
    {

    }
    Debug.Log($"Wall Hit! Reflect Direction: {reflectDirection}, Velocity: {rigid.velocity}");
}

// 충돌 감지
public void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Wall"))
    {
        if (!wasHitted)
        {
            StartCoroutine(HandleCollision(collision));
            Hitted_by_Wall(collision);
            HandleHitCounter();
        }

    }
    else if (collision.gameObject.CompareTag("Obstacle"))
    {
        if (!wasHitted)
        {
            StartCoroutine(HandleCollision(collision));
            Hitted_by_Obstacle(collision);
            HandleHitCounter();
        }
    }
    else if (collision.gameObject.CompareTag("Bullet"))
    {
        if (!wasHitted)
        {
            StartCoroutine(HandleCollision(collision));
            Hitted_by_Bullet(collision);
            HandleHitCounter();
        }
    }
}

// 충돌 횟수 관리
public void HandleHitCounter( )
{
    StartCoroutine(printHitEffect());
    remainingHits--; // 충돌 횟수 감소
    UpdateHitCounter();

    if (remainingHits <= 0)
    {
        EndGame();
    }
}
public IEnumerator printHitEffect()
{
    getDamaged.enabled = true;
    yield return new WaitForSeconds(speedBoostDuration);
    getDamaged.enabled = false;
}

public IEnumerator HandleCollision(Collision2D collision)
{
    wasHitted = true; // 충돌 상태 활성화
    
    yield return new WaitForSeconds(speedBoostDuration);
    
    wasHitted = false; // 충돌 상태 해제
}

// 남은 충돌 횟수 UI 업데이트
void UpdateHitCounter()
{
    printHitEffect();
    hitCounterText.text = "Hits Left: " + remainingHits;  
    
}
void tik_tak()
{
    time_limit--;
    TimeRemaining.text = "Time: " + time_limit;
    if (time_limit <= 0)
    {
        EndGame();
    }
}
IEnumerator ShrinkAndRotate()
{
    // 초기값 저장
    Vector3 initialScale = transform.localScale; // 현재 크기
    Vector3 targetScale = Vector3.zero; // 최종 크기 (0으로 축소)
    Vector3 initialPosition = transform.position; // 현재 위치
    Vector3 targetPosition = endPoint.position; // 이동 목표 (중심)

    float elapsedTime = 0f;
    rigid.velocity = Vector2.zero; // 속도 초기화
    int finalScore = Mathf.RoundToInt(time_limit * 10); // 남은 시간 기반 점수 계산
    FindObjectOfType<NewStageManager>().playerScore = finalScore; // 점수 전달
    // 점점 작아지기
    while (elapsedTime < shrinkDuration)
    {
        // 비율 계산
        float t = elapsedTime / shrinkDuration;

        // 크기 점진적 감소
        transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
        // 위치 점진적으로 중심으로 이동
        transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

        // 회전
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        elapsedTime += Time.deltaTime;
        yield return null;
    }
    Camera.main.GetComponent<Camera_Moving>().enabled = false;


    // 최종적으로 크기를 0으로 설정
    transform.localScale = targetScale;
    Debug.Log("Camera Position: " + Camera.main.transform.position);
    // 게임 종료 딜레이
    yield return new WaitForSeconds(endGameDelay);

    // 게임 종료
    //EndGame();
    FindObjectOfType<NewStageManager>().CompleteStage();
    //SceneManager.LoadScene("Stage_select");
}

// 게임 종료
void EndGame()
{
    Time.timeScale = 0f;
    hitCounterText.text = "Game Over";
    Debug.Log("Game Over");
    currentRightClickTime = 0; // 우 클릭 시간 초기화
    isGameOver = true;
    SceneManager.LoadScene("Stage_select");
}
// ESC 키로 일시정지
void TogglePause()
{
    isPaused = !isPaused;

    if (isPaused)
    {
        Time.timeScale = 0f; // 게임 정지
        pausePanel.SetActive(true); // Paused UI 활성화
        pauseText.text = "Paused";
    }
    else
    {
        Time.timeScale = 1f; // 게임 재개
        pausePanel.SetActive(false); // Paused UI 비활성화
    }
}

    internal void HandleCollision(Collision2D collision2D, object collision)
    {
        throw new NotImplementedException();
    }
}

