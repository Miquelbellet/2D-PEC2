using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private GameObject gameController;
    private bool powerUpShown = false, champiPowerUpMove = false;
    private int rand;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    void FixedUpdate()
    {
        if (champiPowerUpMove)
        {
            transform.GetChild(1).position = new Vector2(transform.position.x + 0.005f, transform.position.y);
        }
    }

    public void ShowPowerUpPlatform()
    {
        if (!powerUpShown)
        {
            powerUpShown = true;
            rand = Random.Range(0, 9);
            if (rand == 0 || rand == 1)
            {
                transform.GetChild(rand).gameObject.SetActive(true);
                if (rand == 1) champiPowerUpMove = true;
                gameController.GetComponent<SFXScript>().ClipShowingPowerUp();
            }
            if (rand > 1 && rand < 5)
            {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(2).GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 15));
                gameController.GetComponent<SFXScript>().ClipCoin();
                gameController.GetComponent<UIScript>().PlusGold();
                gameController.GetComponent<UIScript>().PlusPoints(200);
            }
        }
        if (rand >= 2) Invoke("RestartAnimatorPlatform", 0.3f);
    }

    private void RestartAnimatorPlatform()
    {
        GetComponent<Animator>().SetBool("platformJump", false);
    }

    public void ShowPowerUp()
    {
        if (!powerUpShown)
        {
            powerUpShown = true;
            var rand = Random.Range(0, 6);
            if(rand == 0 || rand == 1)
            {
                transform.GetChild(rand).gameObject.SetActive(true);
                if (rand == 1) champiPowerUpMove = true;
                gameController.GetComponent<SFXScript>().ClipShowingPowerUp();
            }
            else
            {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(2).GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 15));
                gameController.GetComponent<SFXScript>().ClipCoin();
                gameController.GetComponent<UIScript>().PlusGold();
                gameController.GetComponent<UIScript>().PlusPoints(200);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isActiveAndEnabled && collision.gameObject.tag == "Gold")
        {
            Destroy(collision.gameObject);
        }
    }
}
