using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public GameObject mainCamera, mario, blackImg;
    public float waitingToStartTime;
    private float time;
    private void Awake()
    {
        blackImg.SetActive(true);
        mario.GetComponent<PlayerControllerScript>().isMarioDead = true;
    }
    void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= waitingToStartTime && time <= waitingToStartTime+1)
        {
            blackImg.SetActive(false);
            mario.GetComponent<PlayerControllerScript>().isMarioDead = false;
        }
        mainCamera.transform.position = new Vector3(mario.transform.position.x, 0, -1);
    }
}
