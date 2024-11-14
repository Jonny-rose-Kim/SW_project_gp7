using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float dashSpeed = 0.5f;
    [SerializeField]
    private float cur_x = 10.0f;
    [SerializeField]
    private float cur_y = 5.0f;

    private bool isDashing = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // z값 고정 .

        // 카메라 화면 좌표값을 반영하여 마우스 포지션을 설정한다 . 
        // Debug.Log(mousePos);

        float toX = Mathf.Clamp(mousePos.x, -cur_x, cur_x);
        float toY = Mathf.Clamp(mousePos.y, -cur_y, cur_y);

        Vector3 targetPosition = new Vector3(mousePos.x, mousePos.y, transform.position.z);


        if (Input.GetMouseButton(1))
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, dashSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        // mousePos.x , ... 
        // z값을 그대로 유지하고 싶으면 transform.position.z , 마우스 위치에 따라 바꾸고 싶으면 mousePos.x >>

    }
}