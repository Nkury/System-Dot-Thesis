using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusPointer : MonoBehaviour {

    public GameObject target;

    private VirusTrigger virusTrigger;
    
	// Use this for initialization
	void Start () {
        virusTrigger = GameObject.FindObjectOfType<VirusTrigger>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (virusTrigger.closestVirus)
        {
            target = virusTrigger.closestVirus;
        }
        PointAtVirus();
	}

    void PointAtVirus()
    {
        Vector3 targetPosOnScreen = Camera.main.WorldToScreenPoint(target.transform.position);

        if (onScreen(targetPosOnScreen))
        {
            // if object is on screen, hide arrow and return
            if (!this.GetComponent<Blinking>())
            {
                this.gameObject.AddComponent<Blinking>();
            }          
        }
        else
        {
            if (this.GetComponent<Blinking>())
            {
                Destroy(this.GetComponent<Blinking>());
                this.GetComponent<Image>().enabled = true;
            }
        }

        Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        float angle = Mathf.Atan2(targetPosOnScreen.y - center.y, targetPosOnScreen.x - center.x) * Mathf.Rad2Deg;

        this.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, angle);
    }

    bool onScreen(Vector2 input)
    {
        return !(input.x > Screen.width || input.x < 0 || input.y > Screen.height || input.y < 0);
    }
}
