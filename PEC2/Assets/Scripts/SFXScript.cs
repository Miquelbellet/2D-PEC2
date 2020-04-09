using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{
    [Header("Mario GameObject")]
    public GameObject mario;

    [Header("Audio Clips")]
    public AudioClip gameOverClip;
    public AudioClip jumpClip;
    public AudioClip superJumpClip;
    public AudioClip coinClip;
    public AudioClip sowingPowerUpClip;
    public AudioClip powerUpClip;
    public AudioClip fireBall;
    public AudioClip champiDeadFire;
    public AudioClip champiDead;
    public AudioClip blockBreak;
    public AudioClip finishClip;
    public AudioClip endMusic;

    private AudioSource asMusic, asMario;
    private float timer;
    private bool playFinalClip = false;
    void Start()
    {
        asMusic = GetComponent<AudioSource>();
        asMario = mario.GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= GetComponent<GameControllerScript>().waitingToStartTime-1f && timer <= GetComponent<GameControllerScript>().waitingToStartTime) asMusic.Play();

        if (mario.GetComponent<PlayerControllerScript>().finished) FinalClip();
    }

    public void ClipGameOver()
    {
        asMario.Stop();
        asMusic.Stop();
        asMusic.PlayOneShot(gameOverClip);
    }
    public void ClipJump()
    {
        asMario.PlayOneShot(jumpClip);
    }
    public void ClipSuperJump()
    {
        asMario.PlayOneShot(superJumpClip);
    }
    public void ClipCoin()
    {
        asMario.PlayOneShot(coinClip);
    }
    public void ClipShowingPowerUp()
    {
        asMario.PlayOneShot(sowingPowerUpClip);
    }
    public void ClipPowerUp()
    {
        asMario.PlayOneShot(powerUpClip);
    }
    public void ClipFireBall()
    {
        asMario.PlayOneShot(fireBall);
    }
    public void ClipChampiDeadFire()
    {
        asMario.PlayOneShot(champiDeadFire);
    }
    public void ClipChampiDead()
    {
        asMario.PlayOneShot(champiDead);
    }
    public void ClipBlockBrake()
    {
        asMario.PlayOneShot(blockBreak);
    }
    public void ClipFinishLevel()
    {
        asMario.PlayOneShot(finishClip);
    }
    private void FinalClip()
    {
        if (!playFinalClip)
        {
            playFinalClip = true;
            asMusic.Stop();
            asMario.PlayOneShot(finishClip);
        }
        if (!asMario.isPlaying) asMario.PlayOneShot(endMusic);
    }
}
