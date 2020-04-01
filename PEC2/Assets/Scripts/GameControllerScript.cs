using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public GameObject camera, mario;
    void Start()
    {
        
    }

    void Update()
    {
        camera.transform.position = new Vector3(mario.transform.position.x, 0, -1);
    }
}
