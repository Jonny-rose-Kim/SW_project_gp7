using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [SerializeField] private float emitInterval = 3.0f;
    [SerializeField] private float emitDuration = 3.0f;
    [SerializeField] private float LineWidht_start = 0.05f;
    [SerializeField] private float LineWidht_end = 0.05f;
    [SerializeField] private float RotationSpeed = 200f;
    [SerializeField] private Color setColor = new Color(255f,0,0,0.5f);
    [SerializeField] private bool isRotate = true;  // 기본 회전
    private Vector2 distanceDiff = Vector2.zero;
    private bool isRaycasting = false;
    private LineRenderer lineRenderer;
    private float randomRotation;
    void Start()
    {
        // LineRenderer 추가 및 설정
        //lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer = GetComponent<LineRenderer>();        
        lineRenderer.startWidth = LineWidht_start;
        lineRenderer.endWidth = LineWidht_end;
        lineRenderer.startColor = setColor;
        lineRenderer.endColor = setColor;
        
        if(isRotate){
           randomRotation = UnityEngine.Random.Range(0f, 360f); // 0도에서 360도 사이의 랜덤 회전값 
        }
        else{
            randomRotation = 0.0f;
        }
        Invoke("StartRaycasting",emitInterval);
    }

    void LaserRotation(){   // 360도 회전   
        this.transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
    }
    void StartRaycasting(){       
        isRaycasting = true;    // Raycasting 활성화      
        Invoke("StopRaycasting",emitDuration);     // emitDuration 후에 Raycasting 비활성화
    }

    void StopRaycasting(){
        isRaycasting = false;   // Raycasting 비활성화

        // LineRenderer를 통해 레이저를 지우기 위해 시작점과 끝점을 동일하게 설정
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        Invoke("StartRaycasting",emitInterval);
    }

    void Update(){

        if (isRaycasting)
        {
            // Raycast를 실행하여 가장 가까운 object 감지
            RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, this.transform.right, Mathf.Infinity);
            // 레이어 감지
            //RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, this.transform.right, Mathf.Infinity, LayerMask.GetMask("LaserHit"));

            // 충돌 발생 시 
            if (hitInfo.collider != null){
                distanceDiff = hitInfo.point - (Vector2)this.transform.position;    // 거리 계산 
                Debug.Log("Laser hit: " + hitInfo.transform.name + " Distance: " + distanceDiff);

                if (hitInfo.transform.name.Equals("Player"))    // 플레이어랑 충돌 시
                {
                    hitInfo.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);    // 플레이어 상태 변경
                }
                
                // 레이저 정보 지정
                lineRenderer.startColor = setColor;
                lineRenderer.endColor = setColor;
                lineRenderer.startWidth = LineWidht_start;
                lineRenderer.endWidth = LineWidht_end;
                // 레이저 시작점과 끝점을 설정하여 라인을 그림
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                // 충돌이 없을 때 레이저를 무한히 그림  // 지우면 레이저 안그림 // 어차피 벽이면 collision 발생 -> 레이저 생성 ,따라서 끄는게 제대로 작동 안하는 객체 확인 가능
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, this.transform.position + this.transform.right * 100f);
            }
            // LaserRotation(); // Raycasting 중에만 회전됨
        }
        LaserRotation();    // 상시 회전
    }
}