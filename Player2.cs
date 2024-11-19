using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float dashSpeed = 10.0f;
    [SerializeField]
    private float limit_x = 10.0f;
    [SerializeField]
    private float limit_y = 5.0f;
    [SerializeField]
    private float dashCoolTime = 5.0f; // dash cooltime
    [SerializeField]
    private float lastDashTime = -5.0f; // last dashtime

    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private float weaponSpeed = 10.0f; // 무기 발사 속도 

    private bool isDashing = false; // 대시 상태를 추적

    // public bool IsDashing => isDashing; // 카메라에서 대시 상태 접근용



    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // z값 고정

        // float toX = Mathf.Clamp(mousePos.x, -limit_x, limit_x);
        // float toY = Mathf.Clamp(mousePos.y, -limit_y, limit_y);

        // toX, toY 는 화면이 안 움직이는 것 > 보류 . 

        Vector3 targetPosition = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        // mousePos로 설정을 해야 내가 움직이는 만큼 카메라도 따라 움직여 온다 .
        // mousePos.x , mousePos.y

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
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        // weapon shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootingWeapon(); //method
        }

        RotateTowardsMouse(mousePos);

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
