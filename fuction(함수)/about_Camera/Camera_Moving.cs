using UnityEngine;

public class Camera_Moving : MonoBehaviour
{
    public Transform player;       // 따라갈 플레이어의 Transform
    private Vector3 offset;        // 초기 오프셋 (카메라와 플레이어 사이의 거리)

    void Start()
    {
        // 카메라와 플레이어 사이의 초기 오프셋 계산
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // 플레이어의 위치에 초기 오프셋을 더해 카메라 위치를 갱신
        transform.position = player.position + offset;
    }
}

