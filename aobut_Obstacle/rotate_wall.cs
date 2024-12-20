using UnityEngine;

public class rotate_wall : MonoBehaviour
{
    public float rotationSpeed = 50.0f;  // 회전 속도
    private float currentAngle = 0.0f;

    void FixedUpdate()
    {
        // 현재 각도에 회전 속도를 더함
        currentAngle += rotationSpeed * Time.deltaTime;
        // 각도가 360을 초과하면 0으로 초기화
        currentAngle = currentAngle % 360;

        // Z축을 기준으로 회전 각도 설정
        transform.eulerAngles = new Vector3(0, 0, currentAngle);
    }
}
