using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeToBlack : MonoBehaviour {

    public float speed;
    public float startFade;
    public string loadLevel;

    public bool beginFade = false;

    private float num;
    private float fadeCount = 0;
	
	// Update is called once per frame
	void Update () {
        if (beginFade)
        {
            num += Time.deltaTime;
            if (num >= startFade)
            {
                fadeCount += Time.deltaTime;
                this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r,
                                                            this.GetComponent<Image>().color.g,
                                                            this.GetComponent<Image>().color.b,
                                                            speed * fadeCount);
            }

            if (num >= (startFade + (1 / speed)))
            {
                // load the level
                if (SceneManager.GetActiveScene().name == "ESB Crash")
                {
                    SceneManager.LoadScene("LVL2");
                }
            }
        }
	}
}
