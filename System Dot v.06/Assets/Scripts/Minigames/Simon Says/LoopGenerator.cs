using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoopGenerator : MonoBehaviour {

    public Text text;

    System.Random rng;

    public string[] methods;

    public int minNumTasks, maxNumTasks;

    private SimonController simon;

    private PlayerController player;

    private SpawnController spawner;

    private int iStart;


	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        rng = new System.Random();
        simon = FindObjectOfType<SimonController>();
        player = FindObjectOfType<PlayerController>();
        spawner = FindObjectOfType<SpawnController>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void generateLoop()
    {
        switch(rng.Next(0,2))
        {
            case 0:
                generateEasyForLoop();
                break;
            case 1:
                generateEasyForLoop();
                //generateEasyWhileLoop();
                break;
            default:
                break;
        }
    }

    public void generateEasyForLoop()
    {
        int start, end, numTasks, methodIndex;
        string method;

        start = rng.Next(0, 100);
        iStart = start;
        end = rng.Next(minNumTasks, maxNumTasks) + start;

        numTasks = end - start;
        methodIndex = rng.Next(0, 3);
        
        
        switch(methodIndex)
        {
            case 0:
                break;
            case 1:
                spawner.spawnRandomEnemy();
                break;
            case 2:
                spawner.spawnRandomEnemy();
                break;
            default:
                break;
        }

        method = methods[methodIndex];

        simon.setTask(methodIndex);
        simon.numTasksRequired[methodIndex] = numTasks;

        
        text.text = "for(int i = " + start + "; i < " + end + "; i++)\n{\n\t" + method + ";\n}";
    }

    public void generateEasyWhileLoop()
    {

    }

    public int getIStart()
    {
        return iStart;
    }

   
}
