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
        //Un cop creada la bola de foc, que es destrueixi als 3 segons de ser llençada.
        time += Time.deltaTime;
        if (time >= 3f)
        {
            DestroyFireBall();
        }
    }

    private void DestroyFireBall()
    {
        //Desactivar el RigidBody i el  collider, posar la velocitat a 0
        rbFireBall.bodyType = RigidbodyType2D.Kinematic;
        rbFireBall.velocity = new Vector2(0, 0);
        GetComponent<CircleCollider2D>().enabled = false;
        //Activar l'animació de destruir bola i destruir l'objecte
        animatorFireBall.SetTrigger("destroyFireBall");
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si colisiona amb un enemic, que es destrueixi la bola i que mati a l'enemic.
        if(collision.gameObject.tag == "Champi")
        {
            DestroyFireBall();
            collision.gameObject.GetComponent<ChampiScript>().DeadInverse();
        }
    }
}
