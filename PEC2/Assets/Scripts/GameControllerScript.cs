using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public GameObject mainCamera, mario, blackImg;
    public float waitingToStartTime;

    private float time;
    private bool blackSetFalse = false;
    private void Awake()
    {
        //Quan comença el nivell, que activi la pantalla en negre d'informació del nivell i posar 'isMarioDead' a true perque no puguis moure el personatge
        blackImg.SetActive(true);
        mario.GetComponent<PlayerControllerScript>().isMarioDead = true;
    }
    void Start()
    {
        
    }

    void Update()
    {
        //Un cop passat el temps d'espera, treure la pantalla en negre i activar el moviment del personatgee
        time += Time.deltaTime;
        if (time >= waitingToStartTime && !blackSetFalse)
        {
            blackSetFalse = true;
            blackImg.SetActive(false);
            mario.GetComponent<PlayerControllerScript>().isMarioDead = false;
        }

        //Que la camera sempre estigui seguint al personatge el la aixs X.
        mainCamera.transform.position = new Vector3(mario.transform.position.x, 0, -1);
    }
}
