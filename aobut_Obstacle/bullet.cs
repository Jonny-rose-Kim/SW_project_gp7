using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Rigidbody2D rigid;
    private Vector2 lastVelocity;
    public int hit_count;
    // Start is called before the first frame update
    void Start()
    {
        hit_count=5;
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if(rigid.velocity.magnitude<20f){
            rigid.velocity = rigid.velocity.normalized * 20f;
        }
        lastVelocity = rigid.velocity;
    }
    void FixedUpdate()
    {
        if(rigid.velocity.magnitude<20f){
            rigid.velocity = rigid.velocity.normalized * 20f;
        }
        lastVelocity = rigid.velocity;
        
    }
    void colide_to__Wall(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        Vector2 reflectDirection = Vector2.Reflect(lastVelocity, normal);

        if (reflectDirection.magnitude < 20f)
        {
            reflectDirection = reflectDirection.normalized * 20f;
        }

        rigid.velocity = reflectDirection * 1f;
    }
    // 충돌 감지
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")|| collision.gameObject.CompareTag("Obstacle"))
        {
            colide_to__Wall(collision);
            hit_count--;
            if(hit_count<=0){
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
