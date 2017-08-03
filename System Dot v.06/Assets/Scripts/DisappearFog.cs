using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearFog : MonoBehaviour {

    public bool destroy = false;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (destroy) {
            destroy = false;
            //foreach (Transform child in this.gameObject.transform)
            //{
            //    int a = 255;
            //    while (child.gameObject.GetComponent<SpriteRenderer>().color.a > 0)
            //    {
            //        Color fogColor = child.gameObject.GetComponent<SpriteRenderer>().color;
            //        child.gameObject.GetComponent<SpriteRenderer>().color = new Color(fogColor.r, fogColor.g, fogColor.b, a--);
            //    }
            //    Destroy(child.gameObject);
            //}
            PlayerStats.deadObjects.Add(this.gameObject.name);
            Destroy(this.gameObject);
        }
	}


    
}
