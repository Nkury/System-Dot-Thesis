using UnityEngine;
using System.Collections;


public class Logger : MonoBehaviour {

    public float editTime = 0;
    public bool closed = true;
    public int numOfBackSpaces = 0;
    public int numOfDeletes = 0;
    public int numOfMouseClicks = 0;

    public float inactiveTime;
    public float MouseX
    {
        set
        {
            inactiveTime = 0;           
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // if terminal window is open
        if (EnemyTerminal.globalTerminalMode > 1) {

            MouseX = Input.GetAxis("Mouse X");
            inactiveTime = Time.deltaTime; 

            closed = false;
            editTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                numOfBackSpaces++;
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                numOfDeletes++;
            }

            if (Input.GetMouseButtonDown(0))
            {
                numOfMouseClicks++;
            }


        }
        else if(!closed)
        {
            closed = true;
            if (PlayerStats.numberOfPerfectEdits > 0)
            {
                // updates longest time on editing
                if (editTime > PlayerStats.longestTimeOnEditing)
                {
                    PlayerStats.longestTimeOnEditing = editTime;
                }

                // updates average time on editing
                PlayerStats.averageTimeOnEditing += ((editTime - PlayerStats.averageTimeOnEditing) / PlayerStats.numOfEdits);

                // updates most number of backspaces
                if (numOfBackSpaces > PlayerStats.mostNumberOfBackspaces)
                {
                    PlayerStats.mostNumberOfBackspaces = numOfBackSpaces;
                }

                // updates average number of backspaces
                PlayerStats.averageNumberOfBackspaces += ((numOfBackSpaces - PlayerStats.averageNumberOfBackspaces) / PlayerStats.numOfEdits);

                // updates most number of deletes
                if (numOfDeletes > PlayerStats.mostNumberOfDeletes)
                {
                    PlayerStats.mostNumberOfDeletes = numOfDeletes;
                }

                // updates average number of deletes
                PlayerStats.averageNumberOfDeletes += ((numOfDeletes - PlayerStats.averageNumberOfDeletes) / PlayerStats.numOfEdits);

                // updates most number of mouse clicks
                if (numOfMouseClicks > PlayerStats.mostNumberofMouseClicks)
                {
                    PlayerStats.mostNumberofMouseClicks = numOfMouseClicks;
                }

                // updates average number of mouse clicks
                PlayerStats.averageNumberofMouseClicks += ((numOfMouseClicks - PlayerStats.mostNumberofMouseClicks) / PlayerStats.numOfEdits);

                // updates most time of mouse inactivity
                if (inactiveTime > PlayerStats.mostTimeofMouseInactivity)
                {
                    PlayerStats.mostTimeofMouseInactivity = inactiveTime;
                }

                // updates average mouse inactivity
                PlayerStats.averageTimeOfMouseInactivity += ((inactiveTime - PlayerStats.mostTimeofMouseInactivity) / PlayerStats.numOfEdits);


                editTime = 0;
                numOfBackSpaces = 0;
                numOfDeletes = 0;
                numOfMouseClicks = 0;
                //SaveLoad.Save();
            }
        }
	}

}
