using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obs_rigid_circle : MonoBehaviour
{
    Vector3 pos;
    Vector3 v;
    [SerializeField]
    float delta = 10.0f; // ���� ũ��
    [SerializeField]
    float speed = 3.0f; // ȸ�� �ӵ�
    [SerializeField]
    float timeOffset = 0.0f; // ���� �ð� ������

    void Awake()
    {
        v = transform.position; // �ʱ� ��ġ ����
    }

    void FixedUpdate()
    {
        // �ð� �������� �߰��Ͽ� ���� �ð��� ����
        float timeWithOffset = Time.time + timeOffset;

        pos = new Vector3(
            delta * Mathf.Sin(timeWithOffset * speed),
            delta * Mathf.Cos(timeWithOffset * speed),
            0
        );

        transform.position = v + pos;
    }
}
