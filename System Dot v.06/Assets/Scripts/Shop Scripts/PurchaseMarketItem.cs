using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchaseMarketItem : MonoBehaviour {

    public GameObject shopKeeper;

    public Text maxHealthText;
    public Text healthCostText;
    public Text reviveCostText;
    public Text armorCostText;
    public Text maxHealthCostText;
    public Text hatCostText;

    public int healthCost;
    public int revivePotionCost;
    public int armorCost;
    private int maxHealthCost;
    public int cowboyCost;
    public int versionNumber;

    public SoundController sound;
	// Use this for initialization
	void Start () {       
        sound = FindObjectOfType<SoundController>();
	}
	
	// Update is called once per frame
	void Update () {

        versionNumber = PlayerStats.maxHealth - 2;

        if (maxHealthText)
        {
            maxHealthText.text = "v" + versionNumber + " Upgrade";
        }

        maxHealthCost = (PlayerStats.maxHealth) * 125; // if maxHealth = 4, then cost of upgrading is 500

        if(healthCostText)
            healthCostText.text  = healthCost.ToString();

        if(reviveCostText)
            reviveCostText.text  = revivePotionCost.ToString();

        if(armorCostText)
            armorCostText.text = armorCost.ToString();

        if(maxHealthCostText)
            maxHealthCostText.text = maxHealthCost.ToString();

        if(hatCostText)
            hatCostText.text = cowboyCost.ToString();  
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
            PlayerStats.armorHealth += 1;
            itemBought();
        }
    }

    public void purchaseUpgrade()
    {
        if ((PlayerStats.bitsCollected - maxHealthCost) < 0)
        {
            insufficientFunds();
        }
        else
        {
            PlayerStats.bitsCollected -= maxHealthCost;
            PlayerStats.maxHealth += 1;
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
