using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_Rigid : MonoBehaviour
{
    public GameObject bullet; // 발사할 탄환 프리팹
    public Transform firePoint; // 탄환이 발사되는 위치
    public float bulletSpeed = 10f; // 탄환 속도
    private bool isFiring = false; // 발사 상태를 나타내는 플래그
    [SerializeField]
    private float switchTime = 3f; // 발사와 정지 상태를 전환하는 시간
    [SerializeField]
    private float sleep_time = 3f;
    private float lastSwitchTime; // 마지막 상태 전환 시간
    [SerializeField]
    public float fireRate = 0.5f; // 발사 간격 (초)
    private float nextFireTime = 0f;

    void Start()
    {
        if (sleep_time > 0) isFiring = true;
        lastSwitchTime = 0;// Time.time ; // 초기 상태 전환 시간 설정
    }

    void Update()
    {
        // 3초마다 발사 상태 전환
        if (Time.time - lastSwitchTime > switchTime)
        {
            isFiring = !isFiring; // 발사 상태 전환
            lastSwitchTime = Time.time; // 마지막 상태 전환 시간 업데이트
        }

        // 발사 상태일 때만 탄환 발사
        if (isFiring)
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Fire()
    {
        // 탄환 생성
        GameObject bulletInstance = Instantiate(bullet, firePoint.position, firePoint.rotation);

        // 탄환에 힘을 가해 발사
        Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * bulletSpeed; // firePoint의 위쪽 방향으로 발사
    }
}

