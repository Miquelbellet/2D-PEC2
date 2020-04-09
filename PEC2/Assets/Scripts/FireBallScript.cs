using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour
{
    private Animator animatorFireBall;
    private Rigidbody2D rbFireBall;
    private float time;
    void Start()
    {
        animatorFireBall = GetComponent<Animator>();
        rbFireBall = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 3f)
        {
            DestroyFireBall();
        }
    }

    private void DestroyFireBall()
    {
        animatorFireBall.SetTrigger("destroyFireBall");
        rbFireBall.bodyType = RigidbodyType2D.Kinematic;
        rbFireBall.velocity = new Vector2(0, 0);
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Champi")
        {
            DestroyFireBall();
            collision.gameObject.GetComponent<ChampiScript>().DeadInverse();
        }
    }
}
