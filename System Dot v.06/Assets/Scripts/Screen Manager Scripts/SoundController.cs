using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

    public AudioSource[] sounds;
    public AudioSource startup_noise;
    public AudioSource battle_music;

    public AudioSource cowboyHat;


    public float noise_delay;
    private float countdown;

    System.Random rng;

    public int battleMusicDelay;

	// Use this for initialization
	void Start () {
        countdown = noise_delay;
        if (startup_noise != null)
        {
            startup_noise.Play();
        }
        battle_music.PlayDelayed(battleMusicDelay);
        battle_music.loop = true;

        rng = new System.Random();
        

	}
	
	// Update is called once per frame
	void Update () {

        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            if (sounds.Length > 0)
            {
                playRandomSound();
            }
            countdown = noise_delay;
        }

      
	}

    public void playRandomSound()
    {
        AudioSource randomSound = sounds[rng.Next(0, sounds.Length)];
        randomSound.Play();
    }

    public void play(string sound)
    {
        
        if(sound.Equals("cowboy"))
        {
            cowboyHat.Play();
        }

    }
}
