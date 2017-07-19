using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System;
using System.Xml.Linq;
using UnityEngine.UI;

public class TrainIntelliSense : IntelliSense {

    public float startAutoScrollingAtPosition;
    public float maxDistance;
    public float decrement;

    public GameObject background;
    public GameObject soundController;
    public GameObject explosion;
    public GameObject flyAround;
    public GameObject loopExplosions;
    public GameObject bigExplosion;
    public GameObject fadeToBlack;

    private bool initiateDoom = false;
    private bool end = false;
    private AudioSource song;

    public void Start()
    {
        startDifferent = true;
        isAutoScroll = true;
        nextDialogue = true;

        base.Start();
        song = soundController.GetComponents<AudioSource>()[0];
        
        SetDialogue("startTrain");   
    }

    // Update is called once per frame
    public void Update()
    {      
        base.Update();
        allowZooming = false;

        if (background.transform.position.x > maxDistance)
        {
            if (background.transform.position.x < startAutoScrollingAtPosition)
            {
                startAutoScrollingAtPosition -= decrement;
                dialogueIndex++;
                index = 0;            
                nextDialogue = true;
            }
        }
        else if(!initiateDoom)
        {
            initiateDoom = true;
            Doom();            
        }

        if (song.time > 37.5f)
        {
            if (end)
            {
                end = false;
                EndProtocol(true);
            }
        }
        else if(song.time > 20.25)
        {
            if (!end) {
                end = true;
                dialogueIndex++;
                index = 0;
                nextDialogue = true;
                EndProtocol(false);               
            }

            flyAround.GetComponent<PerlinShake>().magnitude += .01f;
        }  

        if(song.time > 23)
        {
            talking = false;
        }
    }


    // looks in dictionary and sets the dialogue to certain keyword passed in
    public override void SetDialogue(string message)
    {
        base.SetCharacterIcon(this.GetComponent<SpriteRenderer>().sprite);
        base.SetDialogue(message);
    }

    public override void AutoScroll()
    {
        nextDialogue = false;
    }

    void Doom()
    {
        Camera.main.GetComponent<PerlinShake>().PlayShake();
        song.Play();
        setDoomText();
        StartCoroutine(explosions());
        this.GetComponent<Animator>().enabled = true;
    }

    void setDoomText()
    {
        dialogueBox.GetComponentInChildren<Text>().color = Color.red;
        dialogueBox.transform.Find("dialogue box").GetComponent<Image>().enabled = false;
        characterIcon.transform.parent.gameObject.SetActive(false);
        dialogueBox.GetComponentInChildren<Text>().gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200.63f, 258.2f);
        dialogueBox.GetComponentInChildren<Text>().gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 258.2f);
    }

    void EndProtocol(bool trueEnd)
    {
        if (!trueEnd)
        {
            loopExplosions.SetActive(true);
            flyAround.GetComponent<PerlinShake>().PlayShake();
        }
        else
        {
            fadeToBlack.GetComponent<FadeToBlack>().beginFade = true;
            bigExplosion.SetActive(true);            
        }
    }

    public IEnumerator explosions(){
        Instantiate(explosion, new Vector3(-3.100536f, -2.223616f, 0), Quaternion.identity);

        dialogueIndex++;
        index = 0;
        nextDialogue = true;

        yield return new WaitForSeconds(.5f);
        Instantiate(explosion, new Vector3(-1.025536f, -1.166616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(explosion, new Vector3(-1.425536f, -1.666615f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.25f);
        Instantiate(explosion, new Vector3(-1.125536f, -1.466616f, 0), Quaternion.identity);

        dialogueIndex++;
        index = 0;
        nextDialogue = true;

        yield return new WaitForSeconds(1.25f);
        Instantiate(explosion, new Vector3(-2.126f, -1.466616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Instantiate(explosion, new Vector3(-1.125536f, -2.262f, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(-3.100536f, -2.223616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Instantiate(explosion, new Vector3(-1.025536f, -1.166616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(explosion, new Vector3(-1.425536f, -1.666615f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.25f);
        Instantiate(explosion, new Vector3(-1.125536f, -1.466616f, 0), Quaternion.identity);

        dialogueIndex++;
        index = 0;
        nextDialogue = true;

        yield return new WaitForSeconds(1.25f);
        Instantiate(explosion, new Vector3(-2.126f, -1.466616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Instantiate(explosion, new Vector3(-1.125536f, -2.262f, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(-3.100536f, -2.223616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.5f);

        dialogueIndex++;
        index = 0;
        nextDialogue = true;

        Instantiate(explosion, new Vector3(-1.025536f, -1.166616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(explosion, new Vector3(-1.425536f, -1.666615f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.25f);
        Instantiate(explosion, new Vector3(-1.125536f, -1.466616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.25f);

        dialogueIndex++;
        index = 0;
        nextDialogue = true;

        Instantiate(explosion, new Vector3(-2.126f, -1.466616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Instantiate(explosion, new Vector3(-1.125536f, -2.262f, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(-3.100536f, -2.223616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Instantiate(explosion, new Vector3(-1.025536f, -1.166616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(explosion, new Vector3(-1.425536f, -1.666615f, 0), Quaternion.identity);
        yield return new WaitForSeconds(.25f);
        Instantiate(explosion, new Vector3(-1.125536f, -1.466616f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.25f);
        Instantiate(explosion, new Vector3(-2.126f, -1.466616f, 0), Quaternion.identity);   
        yield return new WaitForSeconds(.5f);
        Instantiate(explosion, new Vector3(-1.125536f, -2.262f, 0), Quaternion.identity);

        dialogueIndex++;
        index = 0;
        nextDialogue = true;

    }
}


