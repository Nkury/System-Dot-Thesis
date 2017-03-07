using UnityEngine;
using System.Collections;


public class Logger : MonoBehaviour {

    public GameObject terminalWindow;

    public float editTime = 0;
    public bool closed = true;
    public int numOfBackSpaces = 0;
    public int numOfDeletes = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (terminalWindow.activeSelf) {
            closed = false;
            editTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                numOfBackSpaces++;
            } else if (Input.GetKeyDown(KeyCode.Delete))
            {
                numOfDeletes++;
            }

        }
        else if(!closed)
        {
            closed = true;

            // updates longest time on editing
            if(editTime > PlayerStats.longestTimeOnEditing)
            {
                PlayerStats.longestTimeOnEditing = editTime;
            }

            // updates average time on editing
            PlayerStats.averageTimeOnEditing += ((editTime - PlayerStats.averageTimeOnEditing) / PlayerStats.numOfEdits);

            // updates most number of backspaces
            if(numOfBackSpaces > PlayerStats.mostNumberOfBackspaces)
            {
                PlayerStats.mostNumberOfBackspaces = numOfBackSpaces;
            }

            // updates average number of backspaces
            PlayerStats.averageNumberOfBackspaces += ((numOfBackSpaces - PlayerStats.averageNumberOfBackspaces) / PlayerStats.numOfEdits);

            // updates most number of deletes
            if(numOfDeletes > PlayerStats.mostNumberOfDeletes)
            {
                PlayerStats.mostNumberOfDeletes = numOfDeletes;
            }

            // updates average number of deletes
            PlayerStats.averageNumberOfDeletes += ((numOfDeletes - PlayerStats.averageNumberOfDeletes) / PlayerStats.numOfEdits);




            editTime = 0;
            numOfBackSpaces = 0;
            numOfDeletes = 0;
            SaveLoad.Save();
        }
	}
}
