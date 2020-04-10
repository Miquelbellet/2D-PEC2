using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Text bestScoreTxt;
    private int bestScore;
    void Start()
    {
        //Posar la millor puntuació guardada a la pantalla
        bestScore = PlayerPrefs.GetInt("bestScore",  0);
        bestScoreTxt.text = "BEST SCORE - " + bestScore.ToString("000000");
    }

    void Update()
    {
        
    }

    public void StartGame()
    {
        //Començar el joc i anar a l'estena "GameScene".
        SceneManager.LoadScene("GameScene");
    }
}
