using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Laser : MonoBehaviour
{
    [SerializeField] private float emitInterval = 3.0f;
    [SerializeField] private float emitDuration = 3.0f;
    [SerializeField] private float LineWidht_start = 0.5f;
    [SerializeField] private float LineWidht_end = 0.5f;
    [SerializeField]private bool isRotate = false;
    [SerializeField]private bool randomInit = false;
    [SerializeField]private float laserDistance = 100f;
    private Vector2 distanceDiff = Vector2.zero;
    private bool isRaycasting = false;
    private LineRenderer lineRenderer;
    private RaycastHit2D hitInfo;
    private float randomRotation;
    new AudioSource audio = new AudioSource();

public SpriteRenderer getDamaged;

    void Start()
    {
        audio = this.GetComponent<AudioSource>();

        lineRenderer = GetComponent<LineRenderer>();        
        lineRenderer.startWidth = LineWidht_start;
        lineRenderer.endWidth = LineWidht_end;
        lineRenderer.startColor = lineRenderer.startColor;
        lineRenderer.endColor = lineRenderer.startColor;
        
        if(randomInit) {
            randomRotation = UnityEngine.Random.Range(0f, 360f); // 0도에서 360도 사이의 랜덤 회전값
        }
        else{
            randomRotation = 0.0f;
        }
        
        this.transform.Rotate(0, 0, randomRotation); // Z 축 기준 회전
        
        Invoke("StartRaycasting",emitInterval);
    }

    void LaserRotation(){   // 360도 회전   
        this.transform.Rotate(0, 0, 100f * Time.deltaTime);
    }
    void StartRaycasting()
    {       
        audio.Play();
        isRaycasting = true;    // Raycasting 활성화      
        Invoke("StopRaycasting",emitDuration);     // emitDuration 후에 Raycasting 비활성화
    }

    void StopRaycasting()
    {
        audio.Stop();
        isRaycasting = false;   // Raycasting 비활성화

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        Invoke("StartRaycasting",emitInterval);
    }
    private bool isHandlingHit = false; // 중복 호출 방지

    private IEnumerator HandlePlayerHitWithDelay(Player_alt player){
        isHandlingHit = true; 
        player.HandleHitCounter();
        yield return new WaitForSecondsRealtime(1f); // 실시간 1초 대기
        isHandlingHit = false; // 처리 완료 후 플래그 해제
    }

    void Update()
    {
        
        if (isRaycasting)
        {
            hitInfo = Physics2D.Raycast(this.transform.position, this.transform.right, laserDistance);
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.gameObject.name == "Player"){
                    Player_alt player = hitInfo.collider.GetComponent<Player_alt>();

                    if (player != null){
                        if (!isHandlingHit){
                            StartCoroutine(HandlePlayerHitWithDelay(player));
                        }
                    }
                }

                distanceDiff = hitInfo.point - (Vector2)this.transform.position;
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                // 충돌이 없을 때 레이저를 무한히 그림  // 지우면 레이저 안그림 // 어차피 벽이면 collision 발생 -> 레이저 생성 ,따라서 끄는게 제대로 작동 안하는 객체 확인 가능
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, this.transform.position + this.transform.right * laserDistance);
            }
        }
        if(isRotate){
            LaserRotation();    // 상시 회전
        }
    }

    
}
