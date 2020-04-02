using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampiScript : MonoBehaviour
{
    public float forceMovement, maxVelocity, timeWalking;

    private Rigidbody2D rbChampi;
    private Animator animChampi;
    private float time;
    private bool isDead = false;
    void Start()
    {
        rbChampi = GetComponent<Rigidbody2D>();
        animChampi = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(!isDead) Movement();
    }

    private void Movement()
    {
        var pos = transform.position;
        time += Time.deltaTime;
        if (time < timeWalking)
        {
            transform.position = new Vector2(pos.x+0.005f, transform.position.y);
        }
        else if (time > timeWalking && time < timeWalking * 2)
        {
            transform.position = new Vector2(pos.x-0.005f, transform.position.y);
        }
        else time = 0;
    }

    public void DeadAplastament()
    {
        isDead = true;
        rbChampi.velocity = new Vector2(0, 0);
        rbChampi.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<BoxCollider2D>().enabled = false;
        animChampi.SetTrigger("champiDead");
        Destroy(gameObject, 3f);
    }
    public void DeadInverse()
    {
        isDead = true;
        GetComponent<SpriteRenderer>().flipY = true;
        GetComponent<BoxCollider2D>().enabled = false;
        rbChampi.velocity = new Vector2(0, 0);
        rbChampi.AddForce(new Vector2(50f, 150f));
        Destroy(gameObject, 3f);
    }
}
