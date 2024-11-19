using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigid;
    public float forceScale = 5.0f; // 기본 힘의 크기
    private Vector2 moveDirection;
    public Text hitCounterText;          // 남은 충돌 횟수를 표시할 UI 텍스트
    public GameObject pausePanel;        // 일시정지 시 표시할 패널
    public Text pauseText;
     private bool isPaused = false;
     private int remainingHits = 3;

    [SerializeField] private float maxSpeed = 10f; // 최대 속도
    [SerializeField] private float brakeForce = 2f; // 제동력
    [SerializeField] private bool isBraking = false; // 현재 제동 중인지 확인

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // MoveDirection을 업데이트 (마우스 방향으로 설정)
        UpdateMoveDirection();

        // Spacebar로 이동 힘 추가
        if (Input.GetKey(KeyCode.Space))
        {
            AddForce(moveDirection * forceScale);
        }

        // Ctrl로 브레이크 기능
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            StartBraking();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            StopBraking();
        }

        // 최대 속도 제한 적용
        LimitSpeed();
    }

    void FixedUpdate()
    {
        if (isBraking)
        {
            ApplyBrakeForce();
        }
    }

    // MoveDirection 함수: 마우스 방향으로 벡터 설정
    void UpdateMoveDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // 2D에서 Z축 값은 0으로 고정
        moveDirection = (mousePosition - transform.position).normalized;
    }

    // AddForce 함수: 특정 방향으로 힘을 가함
    public void AddForce(Vector2 force)
    {
        rigid.AddForce(force, ForceMode2D.Force);
    }

    // 브레이크 시작
    public void StartBraking()
    {
        isBraking = true;
    }

    // 브레이크 멈춤
    public void StopBraking()
    {
        isBraking = false;
    }

    // 브레이크 힘 적용
    void ApplyBrakeForce()
    {
        if (rigid.velocity.magnitude > 0.1f) // 속도가 거의 0에 가까워질 때까지 제동
        {
            Vector2 brakeDirection = -rigid.velocity.normalized; // 현재 속도의 반대 방향
            rigid.AddForce(brakeDirection * brakeForce, ForceMode2D.Force);
        }
        else
        {
            // 속도가 매우 낮아지면 완전히 멈춤
            rigid.velocity = Vector2.zero;
            StopBraking();
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

    // 장애물 충돌 시 반대 방향으로 힘을 가함
    void Hitted_by_Obstacle(Collision2D collision)
{
    // 현재 속도를 가져옵니다
    Vector2 currentVelocity = rigid.velocity;

    // 반대 방향으로 속도를 절반으로 줄이기 위해 필요한 속도 변화
    Vector2 targetVelocity = -currentVelocity * 1f;

    // 필요한 속도 변화 (현재 속도 -> 목표 속도로 변화)
    Vector2 deltaVelocity = targetVelocity - currentVelocity;

    // 필요한 힘 계산 (F = m * a, a = deltaVelocity / deltaTime)
    Vector2 requiredForce = rigid.mass * (deltaVelocity / Time.fixedDeltaTime);

    // 힘을 가합니다
    rigid.AddForce(requiredForce, ForceMode2D.Force);

    Debug.Log($"Applied Force: {requiredForce}, Target Velocity: {targetVelocity}");
}

    // 벽 충돌 시 속도 제거 및 반대 방향으로 약한 힘 적용
    void Hitted_by_Wall(Collision2D collision)
    {
        Vector2 reflectDirection = Vector2.Reflect(rigid.velocity.normalized, collision.contacts[0].normal); // 반사 방향 계산
        rigid.velocity = Vector2.zero; // 속도 제거
        rigid.AddForce(reflectDirection * (forceScale * 0.5f)); // 반대 방향으로 약한 힘 가하기
        rigid.AddForce(reflectDirection * (forceScale * 0.5f)); // 반대 방향으로 약한 힘 가하기

        Debug.Log("Hitted by Wall");
    }

    // 충돌 감지
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Hitted_by_Obstacle(collision);
            HandleCollision();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Hitted_by_Wall(collision);
        }
    }
     // 충돌 횟수 관리
    void HandleCollision()
    {
        remainingHits--; // 충돌 횟수 감소
        UpdateHitCounter();

        if (remainingHits <= 0)
        {
            EndGame();
        }
    }

    // 남은 충돌 횟수 UI 업데이트
    void UpdateHitCounter()
    {
        hitCounterText.text = "Hits Left: " + remainingHits;
    }

    // 게임 종료
    void EndGame()
    {
        Time.timeScale = 0f; // 게임 정지
        hitCounterText.text = "Game Over";
        Debug.Log("Game Over");
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
}
