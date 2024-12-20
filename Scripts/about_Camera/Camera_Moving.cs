using UnityEngine;

public class Camera_Moving : MonoBehaviour
{
    public Transform player;       // 따라갈 플레이어의 Transform
    public float additionalOffsetX = 25f; // X 방향 추가 오프셋 값
    private Vector3 offset;        // 초기 오프셋 (카메라와 플레이어 사이의 거리)

    void Start()
    {
        // 카메라와 플레이어 사이의 초기 오프셋 계산
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // X 방향 추가 오프셋을 적용한 카메라 위치 갱신
        Vector3 newOffset = offset + new Vector3(additionalOffsetX, 0, 0);
        transform.position = player.position + newOffset;
    }
}

