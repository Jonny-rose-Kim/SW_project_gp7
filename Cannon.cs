using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject bullet;    // 발사할 탄환 프리팹
    public Transform firePoint;        // 탄환이 발사되는 위치
    public float bulletSpeed = 10f;    // 탄환 속도
    public float fireRate = 3f;        // 발사 간격 (초)
    private float nextFireTime = 0f;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        firePoint =  transform;
        InvokeRepeating("Fire", 0f, fireRate);
    }
    void Fire()
    {
        // 탄환 생성
        GameObject Bullet = Instantiate(bullet, firePoint.position, firePoint.rotation);

        // 탄환에 힘을 가해 발사
        Rigidbody2D rb = Bullet.GetComponent<Rigidbody2D>();
        rb.velocity = -firePoint.right * bulletSpeed; // firePoint의 오른쪽 방향으로 발사
        Destroy(Bullet, 2.0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }
}
