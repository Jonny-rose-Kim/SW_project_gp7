using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinRange : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private float rangeScale = 30.0f; // MinRange의 Scale을 조절하는 변수

    void Update()
    {
        // 플레이어 위치에 따라 MinRange 이동
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = targetPosition;

        // MinRange의 크기를 rangeScale로 조절
        transform.localScale = new Vector3(rangeScale, rangeScale, 1);
    }

    // MinRange의 Scale의 반지름을 반환
    public float GetRangeRadius()
    {
        return rangeScale / 2.0f; // 반지름을 반환
    }
}
