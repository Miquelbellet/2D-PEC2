using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject mario;
    public Text pointsTxt, goldTxt, timeTxt;
    public int startTime;

    private PlayerControllerScript marioScript;
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
        if (!marioScript.isMarioDead && !marioScript.finished && timer > 0)
        {
            timer -= Time.deltaTime;
            timeTxt.text = timer.ToString("000");
        }
    }

    public void PlusGold()
    {
        countGold += 1;
        goldTxt.text = countGold.ToString("00");
    }

    public void PlusPoints(int points)
    {
        countPoints += points;
        pointsTxt.text = countPoints.ToString("000000");
    }
}
