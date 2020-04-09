using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public UIScript uiScript;
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
            var rand = Random.Range(0, 6);
            if(rand == 0 || rand == 1)
            {
                transform.GetChild(rand).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(2).GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 15));
                uiScript.PlusGold();
                uiScript.PlusPoints(200);
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
