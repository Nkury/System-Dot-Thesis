using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

    public AudioSource music;

    [Header("Sound Effects")]
    public AudioSource chestOpen;
    public AudioSource death;
    public AudioSource destroyBlock;
    public AudioSource openTerminal;
    public AudioSource hurtEnemy;
    public AudioSource hitPlayer;
    public AudioSource health;
    public AudioSource rightSound;
    public AudioSource wrongSound;

    [Header("CPU Sounds")]
    public AudioSource cowboyHat;

    
    public void play(string input)
    {        
        if(input.Contains("cowboy"))
        {
            cowboyHat.Play();
        }
        else if (input.Contains("chest"))
        {
            chestOpen.Play();
        }
        else if (input.Contains("death"))
        {
            death.Play();
        }
        else if (input.Contains("block"))
        {
            destroyBlock.Play();
        }
        else if (input.Contains("terminal"))
        {
            openTerminal.Play();
        }
        else if (input.Contains("enemy"))
        {
            hurtEnemy.Play();
        } else if (input.Contains("player"))
        {
            hitPlayer.Play();
        } else if (input.Contains("health"))
        {
            health.Play();
        } else if (input.Contains("right"))
        {
            rightSound.Play();
        } else if (input.Contains("wrong"))
        {
            wrongSound.Play();
        }

    }
}
