using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private GameObject gameController;
    private bool powerUpShown = false;
    private int rand;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    public void ShowPowerUpPlatform()
    {
        //S'activa només 1 cop quan saltes per sota d'un bloc de plataforma normal.
        if (!powerUpShown)
        {
            powerUpShown = true;
            //Fer un número random del 0 al 8
            rand = Random.Range(0, 9);
            if (rand == 0 || rand == 1)
            {
                //si toca un 0 o 1 que activi el PowerUp i el efecte sonor
                transform.GetChild(rand).gameObject.SetActive(true);
                gameController.GetComponent<SFXScript>().ClipShowingPowerUp();
            }
            //Si toca un numero del 2 al 5, activar l'or i sumar els punts i el contador
            if (rand > 1 && rand <= 5)
            {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(2).GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 15));
                gameController.GetComponent<SFXScript>().ClipCoin();
                gameController.GetComponent<UIScript>().PlusGold();
                gameController.GetComponent<UIScript>().PlusPoints(200);
            }
            //Si toca del 6 al 8 no fa res, nomes l'animació que la plataforma salta.
        }
        //Si ha tocat Or, que la plataforma pugui seguir fent l'animació de saltar.
        //Esperar un temps a posar l'animació a false perque detecti el true i la faci.
        if (rand >= 2) Invoke("RestartAnimatorPlatform", 0.3f);
    }

    private void RestartAnimatorPlatform()
    {
        GetComponent<Animator>().SetBool("platformJump", false);
    }

    public void ShowPowerUp()
    {
        //S'activa només 1 cop quan saltes per sota d'un bloc de especial '?'.
        if (!powerUpShown)
        {
            powerUpShown = true;
            //Fer un número random del 0 al 5
            var rand = Random.Range(0, 6);
            //si toca un 0 o 1 que activi el PowerUp i el efecte sonor
            if (rand == 0 || rand == 1)
            {
                transform.GetChild(rand).gameObject.SetActive(true);
                gameController.GetComponent<SFXScript>().ClipShowingPowerUp();
            }
            //Si toca un numero del 2 al 5, activar l'or i sumar els punts i el contador
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
        //Quan toca un Or, se li aplica una força cap amunnt i quan cau i  toca la plataforma s'elimina l'objecte
        if (collision.isActiveAndEnabled && collision.gameObject.tag == "Gold")
        {
            Destroy(collision.gameObject);
        }
    }
}
