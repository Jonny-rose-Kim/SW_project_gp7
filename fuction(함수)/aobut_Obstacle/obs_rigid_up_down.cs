using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class obs_rigid_up_down : MonoBehaviour
{
   // Start is called before the first frame update
    Rigidbody2D rigid;
    Vector3 pos;
    Vector3 v;
    [SerializeField]
    float delta = 1.0f;
    float speed = 3.0f;
    void Awake()
    {
         v= transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        pos = new Vector3(
            0,
            delta * Mathf.Sin(Time.time * speed),
            0
        );
        

        transform.position = v + pos;
    }
}
