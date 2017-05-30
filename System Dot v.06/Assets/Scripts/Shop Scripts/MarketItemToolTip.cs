using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MarketItemToolTip : MonoBehaviour
{

    public Text txt;
    // Use this for initialization
    void Start()
    {
        txt.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void OnMouseEnter()
    {
        if(this.gameObject.name == "Health Name Text")
        {
            txt.text = "Instantly heals the player to full health. Press H to use.";
        }
        if (this.gameObject.name == "Armor Name Text")
        {
            txt.text = "Player Armor is maxed out.";
        }
        if (this.gameObject.name == "Cowboy Name Text")
        {
            txt.text = "Howdy Partners, I'm a cowboy.";
        }

     }

    public void OnMouseExit()
    {
        txt.text = "";
    }

}
