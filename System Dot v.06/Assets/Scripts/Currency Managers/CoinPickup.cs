using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour {

    public int pointsToAdd;
    public GameObject coinParticle;

    public AudioSource coinSoundEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerController>() == null)
        {
            return;
        }

        Instantiate(coinParticle, gameObject.transform.position, gameObject.transform.rotation);

        coinSoundEffect.Play();

        Destroy(gameObject);
    }
}
