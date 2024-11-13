using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private MousePointer mousePointer; // MousePointer를 참조

    private Vector3 velocity = Vector3.zero; // 현재 이동 속도

    void Update()
    {
        if (mousePointer == null) return; // MousePointer가 설정되지 않았으면 동작하지 않음

        // MousePointer에서 계산된 중력 벡터를 가져와 Player에 적용
        Vector3 gravityForce = mousePointer.CalculateGravity(transform.position);
        // 현재 위치에서 작용되는 중력을 반환받음.

        if (gravityForce != Vector3.zero)
        {
            // 중력 효과가 존재하면 속도에 중력 추가
            velocity += gravityForce * Time.deltaTime;
        }
        else
        {
            // 중력이 없으면 속도를 0으로 정지
            velocity = Vector3.zero;
        }

        // 속도에 따라 플레이어 위치 업데이트
        transform.position += velocity * Time.deltaTime;

        // 디버그 로그로 속도 확인
        Debug.Log($"현재 속도: {velocity.magnitude:F2}");
        Debug.Log($"현재 time: {Time.deltaTime:F3}");
    }
}
