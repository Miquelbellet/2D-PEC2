using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampiScript : MonoBehaviour
{
    public float timeWalking;

    private GameObject gameController;
    private Rigidbody2D rbChampi;
    private Animator animChampi;
    private float time;
    private bool isDead = false, startWalking = false;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        rbChampi = GetComponent<Rigidbody2D>();
        animChampi = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //Faig que cada enemic començi a caminar a diferents temps i així no van tots exactament iguals
        //Tot i que també es podria canviar la variable 'timeWalking' de cada enemic per separat
        time += Time.deltaTime;
        if (!startWalking && time >= Random.Range(0.1f, 2f))
        {
            startWalking = true;
            time = 0;
        }

        //Si no esta mort i si ja ha acabat el temps d'espera que es posi en moviment
        if (!isDead && startWalking) Movement();
    }

    private void Movement()
    {
        //Que es mogui cap a la dreta durant 'timeWalking' i quan acabi que es mogui cap a l'esquerre el mateix temps
        if (time < timeWalking)
        {
            transform.position = new Vector2(transform.position.x+0.005f, transform.position.y);
        }
        else if (time > timeWalking && time < timeWalking * 2)
        {
            transform.position = new Vector2(transform.position.x-0.005f, transform.position.y);
        }
        else time = 0;
    }

    public void DeadAplastament()
    {
        //Quan mor per aplastament de Mario, posar la velocitat a 0 i desactivar el RigidBody i el Collider. 
        isDead = true;
        rbChampi.velocity = new Vector2(0, 0);
        rbChampi.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<BoxCollider2D>().enabled = false;
        //activar la animació d'aplastat i efecte sonor
        animChampi.SetTrigger("champiDead");
        gameController.GetComponent<SFXScript>().ClipChampiDead();
        //Destruir l'objecte
        Destroy(gameObject, 1.5f);
    }
    public void DeadInverse()
    {
        //Quan mor per bola de foc, girar el sprite,  desactivar el collider i aplicar una força cap amunt
        isDead = true;
        GetComponent<SpriteRenderer>().flipY = true;
        GetComponent<BoxCollider2D>().enabled = false;
        rbChampi.velocity = new Vector2(0, 0);
        rbChampi.AddForce(new Vector2(50f, 150f));
        //Aplicar l'efecte sonor i destruir l'objecte
        gameController.GetComponent<SFXScript>().ClipChampiDeadFire();
        Destroy(gameObject, 3f);
    }
}
