using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class obs_rigid_up_down : MonoBehaviour
{
    int i =1;
    Rigidbody2D rigid;
    Vector3 pos;
    
    void Awake()
    {
        pos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
    }
    void up_and_down(){
        
        if(i%2==1){
            rigid.velocity = new Vector2(0, 100);
        }
         else{
            rigid.velocity = new Vector2(0, -100);
        }
        i++;
    }
    // Update is called once per frame
    void Start()
    {
        //StartCoroutine(up_and_down());
        InvokeRepeating("up_and_down", 0f, 1f);
    }
}
