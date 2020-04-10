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
    public AudioClip marioLoseLife;
    public AudioClip fireBall;
    public AudioClip champiDeadFire;
    public AudioClip champiDead;
    public AudioClip blockBreak;
    public AudioClip finishClip;
    public AudioClip endMusic;
    public AudioClip finalPoints;

    private AudioSource asMusic, asMario;
    private float timer;
    private bool playFinalClip = false, finalMusicSong = false, songStarted = false;
    void Start()
    {
        asMusic = GetComponent<AudioSource>();
        asMario = mario.GetComponent<AudioSource>();
    }

    void Update()
    {
        //Un cop s'acabi el temps d'espera perque es desactivi la pantalla en negre, activar la canço principal de mario.
        timer += Time.deltaTime;
        if(timer >= GetComponent<GameControllerScript>().waitingToStartTime && !songStarted)
        {
            songStarted = true;
            asMusic.Play();
        }

        //Un cop acaba el nivell, activar l'efecte de la barra i la canço final del  nivell
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
    public void ClipLoseLife()
    {
        asMario.PlayOneShot(marioLoseLife);
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
    public void ClipFinalPoints()
    {
        asMario.PlayOneShot(finalPoints);
    }
    private void FinalClip()
    {
        //Parar la conço principal, i activar l'efecte de la barra final
        if (!playFinalClip)
        {
            playFinalClip = true;
            asMusic.Stop();
            asMario.PlayOneShot(finishClip);
        }

        //Un cop acabat l'efecte de la barra final, activar la canço final
        if (!asMario.isPlaying && !asMusic.isPlaying && !finalMusicSong)
        {
            finalMusicSong = true;
            asMusic.PlayOneShot(endMusic);
        }
    }
}
