using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private Player2 player; // 플레이어 객체를 참조
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float followSpeed = 0.5f; // 카메라의 따라가는 속도

    private void Update()
    {

        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = targetPosition;
        // player.transform.position.x
        // 
        // player.transform.position.y


    }
}
