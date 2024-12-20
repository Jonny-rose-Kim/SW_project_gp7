using UnityEngine;
using UnityEngine.UI; // UI ��Ҹ� ����ϱ� ���� �߰�

public class obs_BlackHole_time : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public float gravityStrength = 500f; // ����Ȧ�� �߷� ����
    public float forwardForce = 2f; // ������ ȸ���� ���� �߰����� ��
    public float minimumDistance = 0.5f; // ����Ȧ �߽ɿ� �ʹ� ��������� �ʵ��� �ϴ� �ּ� �Ÿ�
    public float effectRadius = 10f; // ����Ȧ�� �ۿ� �ݰ�
    [SerializeField] private float activeDuration = 6f; // �߷��� Ȱ��ȭ�Ǵ� �ð�
    [SerializeField] private float inactiveDuration = 3f; // �߷��� ��Ȱ��ȭ�Ǵ� �ð�
    [SerializeField] private Text timerDisplay; // �ð��� ǥ���� UI Text

    private Rigidbody2D playerRb; // �÷��̾��� Rigidbody2D
    private float timer; // ���� Ÿ�̸�
    private bool isActive; // �߷� Ȱ��ȭ ����

    void Start()
    {
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }
        timer = inactiveDuration; // �ʱ⿡�� �߷��� ��Ȱ��ȭ ���·� ����
        isActive = false; // �ʱ� ���� ����
    }

    void Update()
    {
        timer -= Time.deltaTime; // �ð��� ����
        if (timer <= 0)
        {
            isActive = !isActive; // Ȱ�� ���¸� ��ȯ
            timer = isActive ? activeDuration : inactiveDuration; // Ÿ�̸� �缳��
        }

        // UI�� Ÿ�̸� ǥ��
        if (timerDisplay != null)
            timerDisplay.text = $"Gravity Active: {isActive} Time: {timer:F2}s";
       
    }

    void FixedUpdate()
    {
        if (player != null && playerRb != null && isActive) // isActive ���� �߰�
        {
            Vector2 directionToCenter = (Vector2)transform.position - (Vector2)player.position;
            float distance = directionToCenter.magnitude;

            if (distance <= effectRadius && distance > minimumDistance)
            {
                float gravityForce = gravityStrength / Mathf.Pow(distance, 1);
                //gravityForce�� �߷��� ��
                Vector2 gravity = directionToCenter.normalized * gravityForce;
                //forward Force�� ���� ���������� ������� ���� ũ��
                Vector2 perpendicularForce = Vector2.Perpendicular(directionToCenter).normalized * forwardForce;

                playerRb.AddForce(gravity + perpendicularForce, ForceMode2D.Force);
                Debug.Log("가해지는 힘의 크기: " + gravity.magnitude+perpendicularForce.magnitude);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimumDistance);
    }
}
