using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ParserAlgo;

public class WordManipulator : MonoBehaviour {

    public List<string> word = new List<string>();
    public string whatToDelete;

    public float activatedIndex;

    private int index = 0;
    private string prevWhatToDelete;
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
                    wordToAdd += secondChild.transform.FindChild("Letter").GetComponent<TextMesh>().text;                                    
                }

                word.Add(wordToAdd);
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (whatToDelete != "" && whatToDelete != prevWhatToDelete)
        {
            index = 0;
            prevWhatToDelete = whatToDelete;

            foreach (Transform child in gameObject.transform)
            {              
                if (child.gameObject.name.Contains("WordBlock"))
                {
                    index++;
                    // enable all blocks
                    foreach (Transform secondChild in child.transform)
                    {
                        DeleteBlock(secondChild.gameObject, true);
                    }

                    if (word[index-1].Contains(whatToDelete))
                    {
                        int indexOf = word[index - 1].IndexOf(whatToDelete[0]);
                        while (indexOf != -1)
                        {
                            for (int i = 0; i < whatToDelete.Length; i++)
                            {
                                DeleteBlock(child.transform.GetChild(indexOf + i).gameObject, false);
                            }

                            indexOf = word[index - 1].IndexOf(whatToDelete[0], indexOf + 1);
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
                        if(i == activatedIndex)
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
}
