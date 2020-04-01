using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public Transform groundCheck1, groundCheck2, jumpCheck1, jumpCheck2;
    public Transform groundCheck21, groundCheck22, jumpCheck21, jumpCheck22;
    public float movmentForce, maxSpeed, frictionFloor, jumpForce;

    private enum PlayerStates { MiniMario, NormalMario, SuperMario};
    private PlayerStates marioState;
    private Rigidbody2D rbPlayer;
    private Animator animatorMario;
    private bool grounded = true, multipleColisionBox = true, notHurtable = false, isMarioDead = false;
    private float movement, reduceVelocity;
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        animatorMario = GetComponent<Animator>();
        marioState = PlayerStates.MiniMario;
    }

    void FixedUpdate()
    {
        if (!isMarioDead)
        {
            HorizontalMove();
            JumpMove();
        }
    }

    private void HorizontalMove()
    {
        movement = Input.GetAxis("Horizontal");
        if (movement > 0)
        {
            animatorMario.SetBool("running", true);
            animatorMario.speed = movement * 2;
            GetComponent<SpriteRenderer>().flipX = false;
            if (rbPlayer.velocity.x < maxSpeed) rbPlayer.AddForce(Vector2.right * movmentForce);

        }
        else if (movement < 0)
        {
            animatorMario.SetBool("running", true);
            animatorMario.speed = (movement * -1) * 2;
            GetComponent<SpriteRenderer>().flipX = true;
            if (rbPlayer.velocity.x > -maxSpeed) rbPlayer.AddForce(Vector2.left * movmentForce);
        }
        else
        {
            if (rbPlayer.velocity.x < 0.5f && rbPlayer.velocity.x > -0.5f) reduceVelocity = 0;
            else if (rbPlayer.velocity.x > 0.5f) reduceVelocity = rbPlayer.velocity.x - Time.deltaTime * frictionFloor;
            else if (rbPlayer.velocity.x < -0.5f) reduceVelocity = rbPlayer.velocity.x + Time.deltaTime * frictionFloor;
            rbPlayer.velocity = new Vector2(reduceVelocity, rbPlayer.velocity.y);
            animatorMario.speed = 1f;
            animatorMario.SetBool("running", false);
        }
    }
    private void JumpMove()
    {
        if (marioState == PlayerStates.MiniMario)
        {
            if (Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                grounded = true;
                multipleColisionBox = true;
            }
        }
        else
        {
            if (Physics2D.Linecast(transform.position, groundCheck21.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, groundCheck22.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                grounded = true;
                multipleColisionBox = true;
            }
        }
        
        if (grounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)))
        {
            rbPlayer.AddForce(Vector2.up * jumpForce);
            grounded = false;
        }
        if (!grounded) animatorMario.SetBool("jumping", true);
        else animatorMario.SetBool("jumping", false);
    }
    private void UpgradeMario()
    {
        if (marioState == PlayerStates.MiniMario)
        {
            marioState = PlayerStates.NormalMario;
            animatorMario.SetTrigger("mario1-mario2");
            rbPlayer.bodyType = RigidbodyType2D.Kinematic;
            rbPlayer.velocity = new Vector2(0, 0);
            Invoke("SetMario2", animatorMario.GetCurrentAnimatorClipInfo(0).Length);
        }
        else if (marioState == PlayerStates.NormalMario)
        {
            marioState = PlayerStates.SuperMario;
            animatorMario.SetTrigger("mario2-mario3");
            rbPlayer.bodyType = RigidbodyType2D.Kinematic;
            rbPlayer.velocity = new Vector2(0, 0);
            Invoke("SetMario3", animatorMario.GetCurrentAnimatorClipInfo(0).Length);
        }
        else if (marioState == PlayerStates.SuperMario)
        {
            Debug.Log("+1000");
        }
    }
    private void ReduceMarioLife()
    {
        if (marioState == PlayerStates.MiniMario)
        {
            isMarioDead = true;
            animatorMario.SetTrigger("marioDead");
            GetComponent<BoxCollider2D>().enabled = false;
            rbPlayer.velocity = new Vector2(0, 0);
            rbPlayer.AddForce(new Vector2(0, jumpForce));
            Invoke("GameOver", 4f);
        }
        else if (marioState == PlayerStates.NormalMario || marioState == PlayerStates.SuperMario)
        {
            marioState = PlayerStates.MiniMario;
            animatorMario.SetTrigger("setMario1");
            transform.GetComponent<BoxCollider2D>().size = new Vector2(0.12f, 0.13f);
            Invoke("Hurtable", 3f);
        }
    }
    private void SetMario2()
    {
        rbPlayer.bodyType = RigidbodyType2D.Dynamic;
        transform.GetComponent<BoxCollider2D>().size = new Vector2(0.16f, 0.29f);

    }
    private void SetMario3()
    {
        rbPlayer.bodyType = RigidbodyType2D.Dynamic;
    }
    private void Hurtable()
    {
        notHurtable = false;
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (multipleColisionBox)
        {
            multipleColisionBox = false;
            if (collision.gameObject.tag == "Platform")
            {
                if (marioState == PlayerStates.NormalMario || marioState == PlayerStates.SuperMario) Destroy(collision.gameObject, 0.5f);
            }
            else if (collision.gameObject.tag == "QuestionPlat")
            {
                if (marioState == PlayerStates.MiniMario)
                {
                    if (!grounded && (Physics2D.Linecast(transform.position, jumpCheck1.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, jumpCheck2.position, 1 << LayerMask.NameToLayer("Ground"))))
                    {
                        collision.gameObject.GetComponent<Animator>().SetTrigger("questionOpened");
                        collision.gameObject.GetComponent<PlatformScript>().ShowPowerUp();
                    }
                }
                else
                {
                    if (!grounded && (Physics2D.Linecast(transform.position, jumpCheck21.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, jumpCheck22.position, 1 << LayerMask.NameToLayer("Ground"))))
                    {
                        collision.gameObject.GetComponent<Animator>().SetTrigger("questionOpened");
                        collision.gameObject.GetComponent<PlatformScript>().ShowPowerUp();
                    }
                }
                
            }
        }

        if (collision.gameObject.tag == "Champi")
        {
            if (Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Champi")) || Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Champi")) 
                || Physics2D.Linecast(transform.position, groundCheck21.position, 1 << LayerMask.NameToLayer("Champi")) || Physics2D.Linecast(transform.position, groundCheck22.position, 1 << LayerMask.NameToLayer("Champi")))
            {
                collision.transform.GetComponent<ChampiScript>().Dead();
                rbPlayer.AddForce(new Vector2(0, jumpForce));
            }
            else
            {
                if (!notHurtable)
                {
                    notHurtable = true;
                    ReduceMarioLife();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PowerUp")
        {
            UpgradeMario();
            Destroy(collision.gameObject);
        }
    }
}
