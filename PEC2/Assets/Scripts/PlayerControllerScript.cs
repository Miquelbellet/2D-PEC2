using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("Scripts")]
    public UIScript uiScript;
    public SFXScript sfxScript;

    [Header("Mario Movement")]
    public float movmentForce;
    public float maxSpeed;
    public float frictionFloor;
    public float jumpForce;

    [Header("Mario Movement")]
    public GameObject FireBallPrefab;
    public GameObject BallContainerR;
    public GameObject BallContainerL;
    public float fireBallForce;

    [Header("Mini Mario Checkers")]
    public Transform groundCheck1;
    public Transform groundCheck2;
    public Transform jumpCheck1;
    public Transform jumpCheck2;

    [Header("Big Mario Checkers")]
    public Transform groundCheck21;
    public Transform groundCheck22;
    public Transform jumpCheck21;
    public Transform jumpCheck22;

    [HideInInspector] public bool isMarioDead = true, finished = false;

    private enum PlayerStates { MiniMario, NormalMario, SuperMario};
    private PlayerStates marioState;
    private Rigidbody2D rbPlayer;
    private Animator animatorMario;
    private bool grounded = true, multipleColisionBox = true, notHurtable = false, jumpDone = false;
    private float movement, reduceVelocity;
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        animatorMario = GetComponent<Animator>();
        marioState = PlayerStates.MiniMario;
    }

    void FixedUpdate()
    {
        //si Marioo no esta mort, que es pugui moure
        if (!isMarioDead)
        {
            if(!finished) HorizontalMove();
            JumpMove();
        }

        //Si ja ha arribat a la barra final, activar l'animació final
        if (finished) AnimationFinish();
    }

    private void Update()
    {
        //Detectar les axis horizontals i permetre disparar a Mario si está en SuperMario
        movement = Input.GetAxis("Horizontal");
        if (marioState == PlayerStates.SuperMario && !finished) Shooting();
    }

    private void HorizontalMove()
    {
        //Si el moviment és cap a la dreta, posar 'running' a true i aplicar una força positiva a Mario si no supera la velocitat máxima.
        if (movement > 0)
        {
            animatorMario.SetBool("running", true);
            animatorMario.speed = movement * 2;
            GetComponent<SpriteRenderer>().flipX = false;
            if (rbPlayer.velocity.x < maxSpeed) rbPlayer.AddForce(Vector2.right * movmentForce);

        }
        //Si el moviment és cap a la esquerre, girar l'sprite, posar 'running' a true i aplicar una força negativa a Mario si no supera la velocitat máxima.
        else if (movement < 0)
        {
            animatorMario.SetBool("running", true);
            animatorMario.speed = (movement * -1) * 2;
            GetComponent<SpriteRenderer>().flipX = true;
            if (rbPlayer.velocity.x > -maxSpeed) rbPlayer.AddForce(Vector2.left * movmentForce);
        }
        //Quan no es detecta que l'usuari mogui al personatge, que vagi reduint la velocitat depenent de 'frictionFloor' ja que el terra no te fricció i no pararia.
        else
        {
            if (rbPlayer.velocity.x < 0.5f && rbPlayer.velocity.x > -0.5f) reduceVelocity = 0;
            else if (rbPlayer.velocity.x > 0.5f) reduceVelocity = rbPlayer.velocity.x - Time.deltaTime * frictionFloor;
            else if (rbPlayer.velocity.x < -0.5f) reduceVelocity = rbPlayer.velocity.x + Time.deltaTime * frictionFloor;
            animatorMario.speed = 1f;
            rbPlayer.velocity = new Vector2(reduceVelocity, rbPlayer.velocity.y);
            animatorMario.SetBool("running", false);
        }
    }
    private void JumpMove()
    {
        //Detectar si el Mario está tocant a terra per posar el grounded a true.
        //Depen de si es el MiniMarrio o els altres ha d'utilitzar punts diferents ja que l'sprite es més gran.
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

        //Si está al terra i es presiona una d'aquestes tecles, se li aplica una força vertical a Mario i un efecte sonor.
        if (grounded && !finished && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)))
        {
            grounded = false;
            rbPlayer.AddForce(Vector2.up * jumpForce);
            if (marioState == PlayerStates.MiniMario) sfxScript.ClipJump();
            else sfxScript.ClipSuperJump();
        }

        //Si está saltant, que s'activi l'animacio de saltar.
        if (!grounded) animatorMario.SetBool("jumping", true);
        else animatorMario.SetBool("jumping", false);
    }
    private void Shooting()
    {
        //Si es presiona la 'C' que s'activi l'animacio de disparar i l'efecte sonor.
        if (Input.GetKeyDown(KeyCode.C))
        {
            animatorMario.SetBool("shooting", true);
            sfxScript.ClipFireBall();

            //Si Mario está mirant cap a la dreta, que se li apliqui una força positiva a la bola i s'inicialitzi a l'objecte de la dreta de l'sprite.
            if (!GetComponent<SpriteRenderer>().flipX)
            {
                var ball = Instantiate(FireBallPrefab, BallContainerR.transform.position, BallContainerR.transform.rotation);
                ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(fireBallForce, 0));
            }
            //Si Mario está mirant cap a l'esquerre, que se li apliqui una força negativa a la bola i s'inicialitzi a l'objecte de l'esquerre de l'sprite
            else
            {
                var ball = Instantiate(FireBallPrefab, BallContainerL.transform.position, BallContainerL.transform.rotation);
                ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(-fireBallForce, 0));
            }
        }
        else animatorMario.SetBool("shooting", false);
    }
    private void UpgradeMario()
    {
        //Pujar de nivell al Mario.
        //Si está en MiniMario, passar a l'estat NormalMario, desactivar el rigidbody mentres duri l'animacio.
        if (marioState == PlayerStates.MiniMario)
        {
            marioState = PlayerStates.NormalMario;
            animatorMario.speed = 1f;
            animatorMario.SetTrigger("mario1-mario2");
            rbPlayer.bodyType = RigidbodyType2D.Kinematic;
            rbPlayer.velocity = new Vector2(0, 0);
            //Quan acabi l'animació fer la funció 'SetMario2'
            Invoke("SetMario2", animatorMario.GetCurrentAnimatorClipInfo(0).Length);
        }
        //Si está en NormalMario, passar a l'estat SuperMario, desactivar el rigidbody mentres duri l'animacio.
        else if (marioState == PlayerStates.NormalMario)
        {
            marioState = PlayerStates.SuperMario;
            animatorMario.speed = 1f;
            animatorMario.SetTrigger("mario2-mario3");
            rbPlayer.bodyType = RigidbodyType2D.Kinematic;
            rbPlayer.velocity = new Vector2(0, 0);
            //Quan acabi l'animació fer la funció 'SetMario3'
            Invoke("SetMario3", animatorMario.GetCurrentAnimatorClipInfo(0).Length);
        }
    }
    private void ReduceMarioLife()
    {
        //Si esta a l'estat MiniMario i es tocat per un enemic, activar l'animacio de mort, desactivar el collider i aplicar una força vertical.
        if (marioState == PlayerStates.MiniMario)
        {
            isMarioDead = true;
            animatorMario.SetTrigger("marioDead");
            GetComponent<BoxCollider2D>().enabled = false;
            rbPlayer.velocity = new Vector2(0, 0);
            rbPlayer.AddForce(new Vector2(0, jumpForce));
            //Activar el Clip de mort
            sfxScript.ClipGameOver();
            //Tornar a la escena del Menu
            Invoke("GameOver", 4f);
        }
        //Si esta a l'estat de NormalMario o SuperMario, passar a MiniMario.
        //Canviar la mida del colider ja que el MiniMario és més petit i  activar el clip de 'Vida Perduda'
        else if (marioState == PlayerStates.NormalMario || marioState == PlayerStates.SuperMario)
        {
            marioState = PlayerStates.MiniMario;
            animatorMario.SetTrigger("setMario1");
            transform.GetComponent<BoxCollider2D>().size = new Vector2(0.12f, 0.13f);
            sfxScript.ClipLoseLife();
            Invoke("Hurtable", 2f);
        }
    }
    private void SetMario2()
    {
        //Tornar a activar el RigidBody i canviar la mida del Collider ja que els sprites són més grans.
        rbPlayer.bodyType = RigidbodyType2D.Dynamic;
        transform.GetComponent<BoxCollider2D>().size = new Vector2(0.16f, 0.29f);

    }
    private void SetMario3()
    {
        //Tornar a activar el RigidBody
        rbPlayer.bodyType = RigidbodyType2D.Dynamic;
    }
    private void Hurtable()
    {
        //Tornar a ser vulnerable
        notHurtable = false;
    }
    private void GameOver()
    {
        //Tornar a la escena del Menu
        SceneManager.LoadScene("MenuScene");
    }
    private void JumpFinish()
    {
        //Quan arribes a la barra del final, posar la velocitat a 0.
        finished = true;
        rbPlayer.velocity = new Vector2(0, 0);
    }
    private void AnimationFinish()
    {
        //Quan toques el terra al acabar el nivell, aplicar una força positiva i invocar 'Finish' als 3 segons
        if (grounded)
        {
            rbPlayer.velocity = new Vector2(maxSpeed/3, rbPlayer.velocity.y);
            animatorMario.SetBool("running", true);
            Invoke("Finish", 3f);
        }
    }
    private void Finish()
    {
        //Sumar els segons restants als punts finals.
        uiScript.EndLevelPoints();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //La varibale 'multipleColisionBox' fa que no es puguin activar dos plataformes a la vegada.
        if (multipleColisionBox)
        {
            //Si toques una plataforma especial '?' amb els objectes que estan assobre dee l'sprite de Mario mentres estas saltant, activar el PowerUp
            if (collision.gameObject.tag == "QuestionPlat")
            {
                multipleColisionBox = false;
                //Si s'està amb l'estat MiniMario es comproben els objectes del MiniMario per detectar colisió superior amb la plataforma '?'
                if (marioState == PlayerStates.MiniMario)
                {
                    if (!grounded && (Physics2D.Linecast(transform.position, jumpCheck1.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, jumpCheck2.position, 1 << LayerMask.NameToLayer("Ground"))))
                    {
                        collision.gameObject.GetComponent<Animator>().SetTrigger("questionOpened");
                        collision.gameObject.GetComponent<PlatformScript>().ShowPowerUp();
                    }
                }
                //Si s'està amb l'estat NormalMario o SuperMario, es compbroben els altres objectes per detectar colisió superior amb la plataforma '?'
                else
                {
                    if (!grounded && (Physics2D.Linecast(transform.position, jumpCheck21.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, jumpCheck22.position, 1 << LayerMask.NameToLayer("Ground"))))
                    {
                        collision.gameObject.GetComponent<Animator>().SetTrigger("questionOpened");
                        collision.gameObject.GetComponent<PlatformScript>().ShowPowerUp();
                    }
                }
                
            }
            //Si detecta collisió amb una plataforma normal, si s'està en MiniMario fa l'animació de saltar la plataforma i sinó la trenca
            else if (collision.gameObject.tag == "Platform")
            {
                multipleColisionBox = false;
                if (marioState == PlayerStates.MiniMario)
                {
                    if (!grounded && (Physics2D.Linecast(transform.position, jumpCheck1.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, jumpCheck2.position, 1 << LayerMask.NameToLayer("Ground"))))
                    {
                        collision.transform.GetComponent<Animator>().SetBool("platformJump", true);
                        collision.gameObject.GetComponent<PlatformScript>().ShowPowerUpPlatform();
                    }
                }
                //Si estas en l'estat NormalMario o SuperMario, trencas la plataforma
                else if (marioState == PlayerStates.NormalMario || marioState == PlayerStates.SuperMario)
                {
                    if (!grounded && (Physics2D.Linecast(transform.position, jumpCheck21.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, jumpCheck22.position, 1 << LayerMask.NameToLayer("Ground"))))
                    {
                        sfxScript.ClipBlockBrake();
                        Destroy(collision.gameObject, 0.2f);
                    }
                }
            }
        }

        //Si colisiones amb un enemic
        if (collision.gameObject.tag == "Champi")
        {
            //Si el trepitjes per sobre, que l'enemic es mori per aplastament, li apliqui una forç vertical i et sumi punts.
            if (Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Champi")) || Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Champi")) 
                || Physics2D.Linecast(transform.position, groundCheck21.position, 1 << LayerMask.NameToLayer("Champi")) || Physics2D.Linecast(transform.position, groundCheck22.position, 1 << LayerMask.NameToLayer("Champi")))
            {
                collision.transform.GetComponent<ChampiScript>().DeadAplastament();
                rbPlayer.AddForce(new Vector2(0, jumpForce));
                uiScript.PlusPoints(100);
            }
            //Si colisiónes amb un enemic però no per sobre, que mati al enemic, que li baixi una vida al Mario i el torni invulnerrable durant un temps.
            else
            {
                if (!notHurtable)
                {
                    notHurtable = true;
                    collision.transform.GetComponent<ChampiScript>().DeadInverse();
                    ReduceMarioLife();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si toques un poowerUp, que li fci un Upgrade al Mario, posi l'efecte sonor, sumi punts i destueixi l'objecte.
        if (collision.tag == "PowerUp")
        {
            UpgradeMario();
            sfxScript.ClipPowerUp();
            uiScript.PlusPoints(1000);
            Destroy(collision.gameObject);
        }
        //Si el Mario colisiona amb un collider de sota el mon vol dir que ha cigut i s'ha de morir.
        //Es posa al Mario a l'estat MiniMario per assegurar-nos que mori i se li redueix una vida.
        else if (collision.tag == "FallDead")
        {
            marioState = PlayerStates.MiniMario;
            ReduceMarioLife();
        }
        //Si colisiona amb la part superior de la barra final del nivell.
        //Que li sumi molts punts i posi la seva velocitat a 0.
        else if (collision.tag == "ExtraFinish")
        {
            if (!jumpDone)
            {
                jumpDone = true;
                uiScript.PlusPoints(5000);
                JumpFinish();
            }
        }
        //Si colisiona amb la part superior de la barra final del nivell.
        //Que li sumi punts i posi la seva velocitat a 0.
        else if (collision.tag == "NormalFinish")
        {
            if (!jumpDone)
            {
                jumpDone = true;
                uiScript.PlusPoints(500);
                JumpFinish();
            }
        }
    }
}
