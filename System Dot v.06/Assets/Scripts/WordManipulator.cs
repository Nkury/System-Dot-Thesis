using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ParserAlgo;

public class WordManipulator : MonoBehaviour {

    public List<string> word = new List<string>();

    // for System.delete();
    public List<string> whatToDelete = new List<string>();

    // for System.activate();
    public float activatedIndex;

    // for System.body();
    public string wordToSet;
    public bool constructWord = false;

    private int index = 0;
    private List<string> prevWhatToDelete = new List<string>();

	// Use this for initialization
	void Start () {
        word.Clear();

       foreach(Transform child in gameObject.transform)
        {
            string wordToAdd = "";
            if (child.gameObject.name.Contains("WordBlock"))
            {
                foreach(Transform secondChild in child.transform)
                {
                    if (secondChild.transform.FindChild("Letter").GetComponent<TextMesh>().text != "")
                    {
                        wordToAdd += secondChild.transform.FindChild("Letter").GetComponent<TextMesh>().text;
                    }                                    
                }

                word.Add(wordToAdd);
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
        // always construct the word
        if (constructWord)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name.Contains("WordBlock"))
                {
                    // enable all blocks
                    foreach (Transform secondChild in child.transform)
                    {
                        DeleteBlock(secondChild.gameObject, true);
                    }
                }
            }

            ConstructWord(this.GetComponent<EnemyTerminal>().actions.Contains(keyActions.TURNLETTER));
            if (whatToDelete.Count != 0)
            {
                constructWord = true;
            }
            else
            {
                constructWord = false;
            }
        } 

        if (whatToDelete.Count != 0 && (whatToDelete != prevWhatToDelete || constructWord))
        {
            index = 0;
            prevWhatToDelete = whatToDelete;
            constructWord = false;

            foreach (Transform child in gameObject.transform)
            {              
                if (child.gameObject.name.Contains("WordBlock"))
                {
                    index++;

                    string stringToCompare = "";

                    // enable all blocks
                    foreach (Transform secondChild in child.transform)
                    {
                        DeleteBlock(secondChild.gameObject, true);
                        stringToCompare += secondChild.transform.FindChild("Letter").GetComponent<TextMesh>().text;
                    }
                    foreach (string toDelete in whatToDelete)
                    {
                        if (stringToCompare.Contains(toDelete))
                        {
                            int indexOf = stringToCompare.IndexOf(toDelete);
                            while (indexOf != -1)
                            {
                                for (int i = 0; i < toDelete.Length; i++)
                                {
                                    DeleteBlock(child.transform.GetChild(indexOf + i).gameObject, false);
                                }

                                indexOf = stringToCompare.IndexOf(toDelete[0], indexOf + 1);
                            }
                        }
                    }
                }
            }
        }

        if (this.GetComponent<EnemyTerminal>().actions.Contains(keyActions.ACTIVATE))
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name.Contains("WordBlock"))
                {
                    int i = 0;       
                    foreach (Transform secondChild in child.transform)
                    {
                        if(i == activatedIndex && secondChild.transform.FindChild("Letter").GetComponent<TextMesh>().text != "" && secondChild.GetComponent<Renderer>().enabled)
                        {
                            secondChild.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                        }
                        else
                        {
                            secondChild.gameObject.GetComponent<SpriteRenderer>().color = new Color(77/255f, 68/255f, 144/255f);
                        }
                        i++;
                    }                    
                }
            }
        }
        else
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name.Contains("WordBlock"))
                {
                    foreach (Transform secondChild in child.transform)
                    {
                      secondChild.gameObject.GetComponent<SpriteRenderer>().color = new Color(77 / 255f, 68 / 255f, 144 / 255f);
                    }
                }
            }
            activatedIndex = -1;
        }         
    }

    public void DeleteBlock(GameObject block, bool visible)
    {
        block.transform.FindChild("Letter").GetComponent<Renderer>().enabled = visible;
        block.gameObject.GetComponent<SpriteRenderer>().enabled = visible;
        block.gameObject.GetComponent<BoxCollider2D>().enabled = visible;
    }

    public void ConstructWord(bool newWord)
    {
        if (newWord)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name.Contains("WordBlock"))
                {
                    // clear all the letters before writing new ones
                    for (int i = 0; i < child.transform.childCount; i++)
                    {
                        child.transform.GetChild(i).transform.FindChild("Letter").GetComponent<TextMesh>().text = "";
                    }

                    if (wordToSet.Length <= child.transform.childCount)
                    {
                        for (int i = 0; i < wordToSet.Length; i++)
                        {
                            child.transform.GetChild(i).transform.FindChild("Letter").GetComponent<TextMesh>().text = wordToSet[i].ToString();
                        }
                    }
                }
            }
        }
        else if(!newWord && word.Count > 0)
        {
            int wordBlockIndex = 0;
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name.Contains("WordBlock"))
                {
                    for (int i = 0; i < child.transform.childCount; i++)
                    {
                        if(i < word[wordBlockIndex].Length)
                        {
                            child.transform.GetChild(i).transform.FindChild("Letter").GetComponent<TextMesh>().text = word[wordBlockIndex][i].ToString();
                        }
                        else
                        {
                            child.transform.GetChild(i).transform.FindChild("Letter").GetComponent<TextMesh>().text = "";
                        }                        
                    }

                    wordBlockIndex++;
                }
            }
        }
    }
}
