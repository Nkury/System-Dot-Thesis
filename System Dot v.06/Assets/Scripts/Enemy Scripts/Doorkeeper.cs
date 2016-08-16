using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Doorkeeper : MonoBehaviour
{

    public int targetNum;

    public string speech;
    public Text speechText;
    public GameObject dialogue;
    public GameObject door;
    public InputField answer;

    public int index = 0;
    public bool showBox = false;
    public int inputValue = -9999;

    // Use this for initialization
    void Start()
    {
        if (speech != "What's the password?")
        {
            targetNum = UnityEngine.Random.Range(2, 8);
            int intermNum = targetNum;
            speech = "lldskj lkjs " + Convert.ToString(targetNum, 2) + " ladks lkjs dlskj alk";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (index <= speech.Length && showBox)
            speechText.text = speech.Substring(0, index);
        else if(showBox)
        {
            index = speech.Length;
        }

        if (showBox)
            index++;

        if(inputValue == targetNum)
        {
            door.SetActive(false);
            dialogue.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else if(inputValue != -9999 && speech != "What's the password?")
        {
            index = 0;
            targetNum = UnityEngine.Random.Range(0, 8);
            speech = "lldskj lkjs " + Convert.ToString(targetNum, 2) + " ladks lkjs dlskj alk";
            inputValue = -9999;
            answer.text = "";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        showBox = true;
        dialogue.SetActive(true);
        answer.text = "";
    }

    void OnTriggerExit2D(Collider2D other)
    {
        showBox = false;
        dialogue.SetActive(false);
    }
}
