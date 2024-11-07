using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float emitInterval = 0.10f;
    [SerializeField] private float emitDuration = 999.0f;
    private Vector2 distanceDiff = Vector2.zero;
    private bool isRaycasting = false;
    private LineRenderer lineRenderer;
    [SerializeField]
    public float minZ = -10.5f;
    [SerializeField]
    public float maxZ = 9.5f;
    void Start()
    {
        // LineRenderer 추가 및 설정
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        
        float randomRotation = UnityEngine.Random.Range(0f, 360f); // 0도에서 360도 사이의 랜덤 회전값
        this.transform.Rotate(0, 0, randomRotation); // Z 축 기준 회전
        InvokeRepeating("StartRaycasting", 0f, emitInterval);    // emitDuration마다 Raycast 시작
    }

    void LaserRotation(){
        // Vector3 MaxRotateTheta = transform.eulerAngles;
        // MaxRotateTheta.z = (MaxRotateTheta.z > 180) ? MaxRotateTheta.z-360 : MaxRotateTheta.z;
        // MaxRotateTheta.z = Mathf.Clamp(MaxRotateTheta.z,minZ,maxZ);
        this.transform.Rotate(0, 0, 100f * Time.deltaTime);   // 360도 회전   
        // this.transform.rotation = Quaternion.Euler(MaxRotateTheta);
    }
    void StartRaycasting()
    {       
        isRaycasting = true;    // Raycasting 활성화      
        Invoke("StopRaycasting", emitDuration);     // emitInterval 후에 Raycasting 비활성화
    }

    void StopRaycasting()
    {
        isRaycasting = false;   // Raycasting 비활성화

        // LineRenderer를 통해 레이저를 지우기 위해 시작점과 끝점을 동일하게 설정
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
    }

    void Update()
    {
        
        if (isRaycasting)
        {
            // Raycast를 실행하여 가장 가까운 object 감지
            RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, this.transform.right, Mathf.Infinity, LayerMask.GetMask("OuterWall", "Player"));

            // 충돌 발생 시 
            if (hitInfo.collider != null)
            {
                
                distanceDiff = hitInfo.point - (Vector2)this.transform.position;    // 거리 계산 
                Debug.Log("Laser hit: " + hitInfo.transform.name + " Distance: " + distanceDiff);
                if (hitInfo.transform.name.Equals("Player"))
                {
                    hitInfo.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
                }
                // 레이저 시작점과 끝점을 설정하여 라인을 그림
                Debug.DrawLine(this.transform.position, hitInfo.point, Color.red);
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                // 충돌이 없을 때 레이저를 무한히 그림  // 지우면 레이저 안그림 // 어차피 벽이면 collision 발생 -> 레이저 생성 ,따라서 끄는게 제대로 작동 안하는 객체 확인 가능
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, this.transform.position + this.transform.right * 100f);
                Debug.Log("No hit detected: " + this);
            }
            LaserRotation();
        }
    }
}