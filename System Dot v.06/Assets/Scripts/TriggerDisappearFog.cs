using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDisappearFog : MonoBehaviour {

    public GameObject fog;
	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            fog.GetComponent<DisappearFog>().destroy = true;
            PlayerStats.deadObjects.Add(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
