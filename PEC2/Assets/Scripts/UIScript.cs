using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public GameObject mario;
    public Text pointsTxt, goldTxt, timeTxt;
    public int startTime;

    private PlayerControllerScript marioScript;
    private bool endLevelPoits = false;
    private int countGold, countPoints;
    private float timer;
    void Start()
    {
        marioScript = mario.GetComponent<PlayerControllerScript>();
        timeTxt.text = startTime.ToString();
        timer = startTime;
    }

    void Update()
    {
        //Mentre el usuari esigui jugant que vagi baixant la variable del temps fins a 0
        if (!marioScript.isMarioDead && !marioScript.finished && !endLevelPoits && timer > 0)
        {
            timer -= Time.deltaTime;
            timeTxt.text = timer.ToString("000");
        }

        //Un cop acabt el nivell, que resti el 1 segon al temps i sumi per cada segon 50 punts.
        //Restar un segon per frame fins arribar a 0.
        if (marioScript.finished && endLevelPoits)
        {
            if (timer > 0)
            {
                timer -= 1;
                countPoints += 50;
                timeTxt.text = timer.ToString("000");
                pointsTxt.text = countPoints.ToString("000000");
                GetComponent<SFXScript>().ClipFinalPoints();
            }
            else
            {
                timer = 0;
                timeTxt.text = timer.ToString("000");
                Invoke("GoToMenu", 2f);
            }
        }
    }

    private void GoToMenu()
    {
        //Comparar si la puntuació final es més gran que la bestScore guardada i anar a l'escena del Menu.
        var bestScore = PlayerPrefs.GetInt("bestScore", 0);
        if(countPoints > bestScore) PlayerPrefs.SetInt("bestScore", countPoints);
        SceneManager.LoadScene("MenuScene");
    }

    public void PlusGold()
    {
        //Sumar 1 de Or i posar-ho a la pantalla
        countGold += 1;
        goldTxt.text = countGold.ToString("00");
    }

    public void PlusPoints(int points)
    {
        //Sumar 1 Punt i posar-ho a la pantalla
        countPoints += points;
        pointsTxt.text = countPoints.ToString("000000");
    }

    public void EndLevelPoints()
    {
        //Un cop entrat al castell, activar que vagi sumant els punts dels segons restants.
        endLevelPoits = true;
    }
}
