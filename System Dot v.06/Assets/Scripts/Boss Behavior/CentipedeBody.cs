using UnityEngine;
using System.Collections;

public class CentipedeBody : MonoBehaviour {


    public int speed = 3;
    public int numTurns = 1;
    public bool coll = true;
    public int id; // what # life is this

	// Use this for initialization
	void Awake () {
        id = CentipedeHead.life;
        id = CentipedeHead.life++;
        int random;
        if(id < 10)
        {
            random = Random.Range(0, 3);
        }
        else
        {
            random = Random.Range(0, 4);
        }

        switch (random)
        {
            case 0:
                this.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(Color.BLUE);";
                this.GetComponent<EnemyTerminal>().numOfLegacy[0] = true;
                break;
            case 1:
                this.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(Color.RED);";
                this.GetComponent<EnemyTerminal>().numOfLegacy[0] = true;
                break;
            case 2:
                this.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(Color.GREEN);";
                this.GetComponent<EnemyTerminal>().numOfLegacy[0] = true;
                break;
            case 3:
                this.GetComponent<EnemyTerminal>().numOfLegacy[0] = false;
                break;
        }

        this.GetComponent<EnemyTerminal>().checkTerminalString();
        this.GetComponent<EnemyTerminal>().evaluateActions();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (BossIntellisense.startBoss)
        {
            this.transform.Translate(new Vector3(-.01f * speed, 0, 0));

            if (CentipedeHead.lives < 9)
            {
                speed = 10;
            }
            if (CentipedeHead.lives < 15)
            {
                speed = 6;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Centipede Trigger" && other.gameObject.name.Substring(0, 1) == "C")
        {
            this.transform.position = new Vector2(39.87f, .88f);
        }
        //if(other.gameObject.tag == "Centipede Trigger" && other.gameObject.name.Substring(0, 1) == "C")
        //{
        //    if (!coll)
        //    {
        //        this.transform.Rotate(0, 0, -90);
        //        coll = true;
        //        numTurns++;
        //    }
        //    else
        //    {
        //        if(numTurns % 4 == 1 || numTurns % 4 == 3)
        //        {
        //            this.transform.Rotate(0, 0, -180);
        //            numTurns++;
        //        } else if (numTurns % 4 == 2 || numTurns % 4 == 0)
        //        {
        //            this.transform.Rotate(0, 0, 180);
        //            numTurns++;
        //        }
        //    }
        //}
        if (coll)
        {
            if(other.gameObject.tag == "Centipede Trigger" && other.gameObject.name.Substring(0, 1) == "A")
            {
                if(numTurns % 4 == 1 || numTurns % 4 == 2)
                {
                    this.transform.Rotate(0, 0, 90);
                    numTurns++;
                    coll = false;
                } else if(numTurns % 4 == 3 || numTurns % 4 == 0)
                {
                    this.transform.Rotate(0, 0, -90);
                    numTurns++;
                    coll = false;
                }
            }
        }
        else
        {
            if (other.gameObject.tag == "Centipede Trigger" && other.gameObject.name.Substring(0, 1) == "B")
            {
                if (numTurns % 4 == 1 || numTurns % 4 == 2)
                {
                    this.transform.Rotate(0, 0, 90);
                    numTurns++;
                    coll = true;
                }
                else if (numTurns % 4 == 3 || numTurns % 4 == 0)
                {
                    this.transform.Rotate(0, 0, -90);
                    numTurns++;
                    coll = true;
                }
            }
        }
    }
}
