using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    [SerializeField]
    private MinRange minRange; 
    [SerializeField]
    private float gravityStrength = 9.8f;
    [SerializeField]
    private float gravityRange = 5.0f;


    // Update is called once per frame
    void Update()
    {
        // 마우스 위치를 얻어와 화면상의 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // z값 고정

        // MinRange 범위 계산
        float minDistance = minRange.GetRangeRadius(); // MinRange의 반지름
        Vector3 directionToMouse = (mousePos - minRange.transform.position).normalized; // MinRange에서 마우스까지의 방향

        // MinRange 범위 안으로 들어가려 할 경우 범위 가장자리에 위치
        if (Vector3.Distance(mousePos, minRange.transform.position) < minDistance)
        {
            // MinRange의 가장자리 위치로 제한
            mousePos = minRange.transform.position + directionToMouse * minDistance;
        }

        // 마우스 포인터 위치 업데이트
        transform.position = mousePos;
    }

    // 플레이어가 사용할 중력 계산 메서드
    public Vector3 CalculateGravity(Vector3 playerPosition)
    {
        // MousePointer와 Player 사이의 거리 계산
        Vector3 directionToPlayer = transform.position - playerPosition;
        // 마우스의 위치 - 플레이어의 위치
        float distance = directionToPlayer.magnitude;

        // 거리 범위 내에 있을 때만 중력 적용
        if (distance <= gravityRange)
        {
            // 거리 비례 중력 계산 (거리 반비례)
            Vector3 gravityForce = directionToPlayer.normalized * gravityStrength * distance;
            // directionToPlayer.normalized * gravityStrength
            // (distance * distance)
            return gravityForce;
        }

        // 범위를 벗어나면 중력 작용 ㄴㄴ
        return Vector3.zero;
        
    }

}
