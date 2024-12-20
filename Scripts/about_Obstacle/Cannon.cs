using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject bullet; // 발사할 탄환 프리팹
    public Transform firePoint; // 탄환이 발사되는 위치
    [SerializeField]
    public float bulletSpeed = 10f; // 탄환 속도
    [SerializeField]
    public float fireRate = 3.7f; // 발사 간격 (초)
    private float nextFireTime = 0f;
    [SerializeField]
    private float rangeZ = 45f; // z값 변동 범위
    // >> 각도 설정 값에 따라 대포가 고개 흔드는 범위가 늘거나 줄어듭니당
    private float initialZ; // 초기 rotation.z 값
    private AudioSource audiot;

    void Start()
    {
        audiot = this.GetComponent<AudioSource>();
        // 현재 설정된 rotation.z 값을 가져옴
        initialZ = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        // 발사 간격 체크
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time  + fireRate + UnityEngine.Random.Range(0.0f, 2.0f);
        }

        // z값을 초기 z값 기준으로 +30 ~ -30 범위로 설정
        float zValue = Mathf.PingPong(Time.time * 60f, rangeZ * 2) - rangeZ + initialZ;
        transform.rotation = Quaternion.Euler(0, 0, zValue);
    }

    void Fire()
    {
        
        audiot.Play();
        // 탄환 생성
        GameObject Bullet = Instantiate(bullet, transform.position, transform.rotation);

        // 탄환에 힘을 가해 발사
        Rigidbody2D rb = Bullet.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * bulletSpeed; // transform의 위쪽 방향으로 발사
    }
}
