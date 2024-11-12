using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float dashSpeed = 0.5f;

    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private float weaponSpeed = 10.0f; // 무기 발사 속도

    private bool isDashing = false; // 대시 상태를 추적

    public bool IsDashing => isDashing; // 카메라에서 대시 상태 접근용

    [SerializeField]
    private float dashCoolTime = 5.0f; // dash cooltime
    private float lastDashTime = -5.0f; // last dashtime


    [SerializeField]
    private float maxSpeed = 5.0f; // 최대 속도
    [SerializeField]
    private float maxAcceleration = 2.0f; // 최대 가속도
    [SerializeField]
    private float stoppingDistance = 0.1f; // 목표에 도달했음을 판단할 거리

    private Vector3 targetPosition; // 마우스 위치를 목표로 설정

    private float currentSpeed = 0.0f; // 현재 속도
    private float currentAcceleration = 0.0f; // 현재 가속도

    private Vector3 lastDirection = Vector3.zero; // 마지막 이동 방향

    void Start()
    {
        // 시작 시 플레이어의 초기 목표 위치는 현재 위치로 설정
        targetPosition = transform.position;
    } // start는 추후에 없애던지 하자.

    void Update()
    {

        // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // mousePos.z = transform.position.z; // z값 고정
        // 기존에 있던 코드


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // Z값 고정
        targetPosition = mousePos;

        if (Input.GetMouseButton(1) && Time.time >= lastDashTime + dashCoolTime)
        {
            // 마지막 시간이 Time.time으로 갱신되면 dashcool이 5초라 다시 5초 기다려야함.
            isDashing = true; // 대시 상태로 전환
            lastDashTime = Time.time; // 대시 실행 시간 갱신. 
            transform.position = Vector3.Lerp(transform.position, targetPosition, dashSpeed * Time.deltaTime);
            // transform.position = Vector3.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);
            // moveTowards >> 뚝딱이처럼 움직임.
            // moveTowards and Lerp difference is what?
        }

        else
        {
            isDashing = false; // 대시 상태 해제
            // transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
        }



        // 목표 위치까지 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // 방향 변경 감지
        if (Vector3.Dot(lastDirection, directionToTarget) < 0 && currentSpeed > 0)
        {
            // 방향이 반대라면 감속 시작 (가속도는 음수, 속도는 점차 감소)
            currentAcceleration = -maxAcceleration;
            currentSpeed = Mathf.Max(0, currentSpeed + currentAcceleration * Time.deltaTime);
        }
        else if (distanceToTarget > stoppingDistance) // 목표까지 이동
        {
            // 목표까지 도달하지 않은 경우 가속
            currentAcceleration = Mathf.Lerp(currentAcceleration, maxAcceleration, Time.deltaTime);
            currentSpeed = Mathf.Min(currentSpeed + currentAcceleration * Time.deltaTime, maxSpeed);
        }
        else
        {
            // 목표 위치에 도달 시 정지
            currentSpeed = 0.0f;
            currentAcceleration = 0.0f;
        }

        // 속도에 따라 플레이어 이동
        transform.position += directionToTarget * currentSpeed * Time.deltaTime;

        // 디버그 출력 (속도와 가속도 확인)
        Debug.Log($"현재 속도: {currentSpeed:F2}, 현재 가속도: {currentAcceleration:F2}, 목표까지 거리: {distanceToTarget:F2}");

        // 이동 후 방향 갱신
        lastDirection = directionToTarget;
    }



    private void ShootingWeapon()
    {
        // 무기 인스턴스 생성
        GameObject newWeapon = Instantiate(weapon, transform.position, Quaternion.identity);

        // 무기 발사 방향 설정 (45도 각도로 발사)
        Rigidbody2D rb = newWeapon.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 launchDirection = new Vector2(1, 1).normalized; // 45도 방향 벡터
            rb.AddForce(launchDirection * weaponSpeed, ForceMode2D.Impulse); // 초기 속도 부여
        }

        // 3초 후에 무기 삭제
        Destroy(newWeapon, 3.0f);
    }

    private void RotateTowardsMouse(Vector3 mousePos)
    {
        // 마우스와 플레이어 사이의 방향 벡터 .
        Vector3 direction = (mousePos - transform.position).normalized;

        //방향 벡터를 각도로 변환 (z축 회전을 위한 각도 계산)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 플레이어 회전 설정
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }



}
