    using System.Collections;
    using Unity.Mathematics;
    using UnityEngine;

    public class BlackHole : MonoBehaviour
    {
    public Transform player;            // 플레이어의 Transform
    [SerializeField] public float gravityStrength = 5000f; // 블랙홀의 중력 강도
    [SerializeField] private Vector3 teleport = new Vector3(-490, 100, 0);
    public float forwardForce = 2f;     // 나선형 회전을 위한 추가적인 힘
    public float minimumDistance = 0.5f; // 블랙홀 중심에 너무 가까워지지 않도록 하는 최소 거리
    public float effectRadius = 30f;    // 블랙홀의 작용 반경
    private Rigidbody2D playerRb;       // 플레이어의 Rigidbody2D
    [SerializeField] private float EventHorizon = 20.0f;

    void Start()
    {   
        // 플레이어의 Rigidbody2D를 가져옴
        if (player != null){
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        // 플레이어가 블랙홀의 영향을 받을 수 있는지 확인
        if (player != null && playerRb != null){
            
            // 블랙홀 중심과 플레이어 사이의 거리 계산
            Vector2 directionToCenter = (Vector2)transform.position - (Vector2)player.position;
            
            float distance = directionToCenter.magnitude;
            if (distance <= EventHorizon){
                playerRb.transform.SetPositionAndRotation(teleport,Quaternion.identity);
            }
            // 플레이어가 블랙홀의 작용 반경(effectRadius) 내에 있는 경우에만 힘 적용
            if (distance <= effectRadius && distance > minimumDistance)
            {
                // 거리에 반비례하는 중력 계산
                float gravityForce = gravityStrength / Mathf.Pow(distance, 1);
                Vector2 gravity = directionToCenter.normalized * gravityForce;

                // 나선형 효과: 중심으로 끌어당기면서 회전
                //Vector2 perpendicularForce = Vector2.Perpendicular(directionToCenter).normalized * forwardForce;

                // 중력과 나선형 힘을 합산하여 플레이어에게 적용
                playerRb.AddForce(gravity /*+ perpendicularForce*/, ForceMode2D.Force);
            }


        }
    } 
    void OnDrawGizmosSelected()
    {
        // 블랙홀의 작용 반경과 최소 거리 시각화 (에디터에서만 보임)
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, effectRadius); // 작용 반경
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, minimumDistance); // 최소 거리
    }
    }

