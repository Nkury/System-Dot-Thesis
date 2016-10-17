using UnityEngine;
using System.Collections;

public class CentipedeBody : MonoBehaviour {


    public int speed = 1;
    public int numTurns = 1;

	// Use this for initialization
	void Start () {
        int random = Random.Range(0, 4);
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
	}
	
	// Update is called once per frame
	void Update () {
            this.transform.Translate(new Vector3(-.01f * speed, 0, 0));
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(this.gameObject.name + " I got here");
        if(other.gameObject.tag == "Centipede Trigger" && (numTurns % 4 == 1 || numTurns % 4 == 2)) {
            this.transform.Rotate(0, 0, 90);
            numTurns++;
        } else if(other.gameObject.tag == "Centipede Trigger" && (numTurns % 4 == 3 || numTurns % 4 == 0))
        {
            this.transform.Rotate(0, 0, -90);
            numTurns++;
        }

        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Centipede Trigger") { }
           // numTurns++;
    }
}
