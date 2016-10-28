using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchaseMarketItem : MonoBehaviour {

    public int healthPotionCost;
    public int armorCost;
    public int cowboyCost;
    public Text txt;

    public SoundController sound;
	// Use this for initialization
	void Start () {
        healthPotionCost = 500;
        armorCost = 750;
        cowboyCost = 2000;
        sound = FindObjectOfType<SoundController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void purchaseHealthPotion()
    {
        resetText();
        if ((PlayerStats.bitsCollected - healthPotionCost) < 0 )
        {
            insufficientFunds();
        }
        else
        {
            PlayerStats.bitsCollected -= healthPotionCost;
            PlayerStats.numHealthPotions += 1;
        }
    }

    public void purchaseArmor()
    {
        resetText();
        if ((PlayerStats.bitsCollected - armorCost) < 0)
        {
            insufficientFunds();
        }
        else
        {
            PlayerStats.bitsCollected -= armorCost;
            PlayerStats.armorHealth += 5;
        }
    }

    public void purchaseCowboyHat()
    {
        
        resetText();
        if ((PlayerStats.bitsCollected - cowboyCost) < 0)
        {
            insufficientFunds();
        }
        else
        {
            PlayerStats.bitsCollected -= cowboyCost;
            PlayerStats.hat = "cowboy";
            sound.play("cowboy");
        }
        
    }

    void insufficientFunds()
    {
        txt.text = "Sorry! You don't have enough bits!";
    }

    void resetText()
    {
        txt.text = "";
    }
}
