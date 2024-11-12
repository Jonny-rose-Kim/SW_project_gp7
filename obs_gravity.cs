using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obs_gravity : MonoBehaviour
{
     public Transform player;            // 플레이어의 Transform
    public float attractionRange = 500f;  // 끌어당기는 거리 범위
    public float attractionForce = 50f; // 끌어당기는 힘의 크기
    public float Weight = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // 플레이어가 attractionRange 내에 있을 경우에만 힘을 가함
        if (distance < attractionRange)
        {
            if(distance<20){
                Weight = 2;
                if(distance<10){
                    Weight = 3;
                }
            }
            // 플레이어가 장애물을 향하도록 방향 벡터 계산
            Vector2 direction = (transform.position - player.position).normalized;

            // 플레이어의 Rigidbody2D에 힘을 가해 끌어당김
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(direction *Weight* attractionForce * Time.fixedDeltaTime, ForceMode2D.Force);
            }
        }
    }
}
