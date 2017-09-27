using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchaseMarketItem : MonoBehaviour {

    public GameObject shopKeeper;
    public int healthCost;
    public int revivePotionCost;
    public int armorCost;
    public int cowboyCost;

    public SoundController sound;
	// Use this for initialization
	void Start () {
        revivePotionCost = 500;
        armorCost = 750;
        cowboyCost = 2000;
        sound = FindObjectOfType<SoundController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void purchaseHealth()
    {
        if ((PlayerStats.bitsCollected - healthCost) < 0)
        {
            insufficientFunds();
        } else if((PlayerStats.currentHealth >= PlayerStats.maxHealth))
        {
            shopKeeper.GetComponent<Shopkeeper>().SetDialogue("healthMaxed");
        }
        else
        {
            PlayerStats.bitsCollected -= healthCost;
            PlayerStats.currentHealth += 1;
            itemBought();
        }
    }


    public void purchaseRevivePotion()
    {
        if ((PlayerStats.bitsCollected - revivePotionCost) < 0 )
        {
            insufficientFunds();
        }
        else
        {
            PlayerStats.bitsCollected -= revivePotionCost;
            PlayerStats.numRevivePotions += 1;
            itemBought();
        }
    }

    public void purchaseArmor()
    {
        if ((PlayerStats.bitsCollected - armorCost) < 0)
        {
            insufficientFunds();
        }
        else
        {
            PlayerStats.bitsCollected -= armorCost;
            PlayerStats.armorHealth += 5;
            itemBought();
        }
    }

    public void purchaseCowboyHat()
    {
        if ((PlayerStats.bitsCollected - cowboyCost) < 0)
        {
            insufficientFunds();
        }
        else
        {
            PlayerStats.bitsCollected -= cowboyCost;
            PlayerStats.hat = "cowboy";
            itemBought();
            sound.play("cowboy");
        }
        
    }

    void insufficientFunds()
    {
        shopKeeper.GetComponent<Shopkeeper>().SetDialogue("insufficientFunds");
    }

    void itemBought()
    {
        shopKeeper.GetComponent<Shopkeeper>().SetDialogue("itemBought");
    }
}
