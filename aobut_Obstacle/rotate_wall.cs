using UnityEngine;

public class rotate_wall : MonoBehaviour
{
    public float rotationSpeed = 50.0f;  // ȸ�� �ӵ�
    private float currentAngle = 0.0f;

    void FixedUpdate()
    {
        // ���� ������ ȸ�� �ӵ��� ����
        currentAngle += rotationSpeed * Time.deltaTime;
        // ������ 360�� �ʰ��ϸ� 0���� �ʱ�ȭ
        currentAngle = currentAngle % 360;

        // Z���� �������� ȸ�� ���� ����
        transform.eulerAngles = new Vector3(0, 0, currentAngle);
    }
}
