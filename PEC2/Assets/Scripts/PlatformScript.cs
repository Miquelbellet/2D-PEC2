using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private bool powerUpShown = false;
    void Start()
    {

    }

    void Update()
    {

    }

    public void ShowPowerUp()
    {
        if (!powerUpShown)
        {
            powerUpShown = true;
            var obj = transform.GetChild(Random.Range(0, 2));
            obj.gameObject.SetActive(true);
        }
    }
}
