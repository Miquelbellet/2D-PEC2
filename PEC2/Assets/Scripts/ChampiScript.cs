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

    void Update()
    {
        if(!isDead) Movement();
    }

    private void Movement()
    {
        time += Time.deltaTime;
        if (time < timeWalking)
        {
            if (rbChampi.velocity.x <= maxVelocity) rbChampi.AddForce(new Vector2(forceMovement, 0));
        }
        else if (time > timeWalking && time < timeWalking * 2)
        {
            if (rbChampi.velocity.x >= -maxVelocity) rbChampi.AddForce(new Vector2(-forceMovement, 0));
        }
        else time = 0;
    }

    public void Dead()
    {
        isDead = true;
        rbChampi.velocity = new Vector2(0, 0);
        rbChampi.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<BoxCollider2D>().enabled = false;
        animChampi.SetTrigger("champiDead");
        Destroy(gameObject, 3f);
    }
}
