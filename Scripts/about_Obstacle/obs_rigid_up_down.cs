using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class obs_rigid_up_down : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigid;
    Vector3 pos;
    Vector3 v;
    [SerializeField]
    float delta = 1.0f;
    float speed = 3.0f;
    [SerializeField]
    float timeOffset = 0.0f; // 시작 시간 오프셋
    void Awake()
    {
        v = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 시간 오프셋을 추가하여 시작 시간을 조정
        float timeWithOffset = Time.time + timeOffset;
        pos = new Vector3(
            0,
            delta * Mathf.Sin(timeWithOffset * speed),
            0
        );


        transform.position = v + pos;
    }
}