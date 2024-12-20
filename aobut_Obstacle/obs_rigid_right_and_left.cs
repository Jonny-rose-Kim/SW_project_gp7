using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class obs_rigid_right_and_left : MonoBehaviour
{
   // Start is called before the first frame update
    Rigidbody2D rigid;
    Vector3 pos;
    Vector3 v;
    [SerializeField]
    float delta = 1.0f;
    [SerializeField]
    float speed = 3.0f;
    void Awake()
    {
         v= transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        pos = new Vector3(
            delta * Mathf.Cos(Time.time * speed),
            0,
            0
        );
        

        transform.position = v + pos;
    }
}
