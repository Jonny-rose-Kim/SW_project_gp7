using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obs_rigid_circle : MonoBehaviour
{
    Vector3 pos;
    Vector3 v;
    [SerializeField]
    float delta = 10.0f; // 원의 크기
    [SerializeField]
    float speed = 3.0f; // 회전 속도
    [SerializeField]
    float timeOffset = 0.0f; // 시작 시간 오프셋

    void Awake()
    {
        v = transform.position; // 초기 위치 저장
    }

    void FixedUpdate()
    {
        // 시간 오프셋을 추가하여 시작 시간을 조정
        float timeWithOffset = Time.time + timeOffset;

        pos = new Vector3(
            delta * Mathf.Sin(timeWithOffset * speed),
            delta * Mathf.Cos(timeWithOffset * speed),
            0
        );

        transform.position = v + pos;
    }
}
