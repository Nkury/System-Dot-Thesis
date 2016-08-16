using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimonController : MonoBehaviour {

    public static int JUMP = 0;
    public static int JUMP_OVER_ENEMY = 1;
    public static int KILL_ENEMY = 2;
    public static int COLLECT_COIN = 3;

    private PlayerController player;
    private SimonTimer timer;
    private SimonScoreManager scoreManager;
    private LoopGenerator loopGenerator;
    private PlayerFeedbackManager feedback;
    private ActionsPerfromedManager actionManager;

    public Text intI;

    public int[] numTasksRequired;
    //public int[] numTasksPerformed;

    public float waitBetweenRounds;
    private float countdown;

    private bool roundOver;
    private int task;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        timer = FindObjectOfType<SimonTimer>();
        scoreManager = FindObjectOfType<SimonScoreManager>();
        loopGenerator = FindObjectOfType<LoopGenerator>();
        feedback = FindObjectOfType<PlayerFeedbackManager>();
        actionManager = FindObjectOfType<ActionsPerfromedManager>();

        loopGenerator.generateLoop();
        countdown = waitBetweenRounds;
        roundOver = false;
	}
	
	// Update is called once per frame
	void Update () {

        intI.text = "" + (loopGenerator.getIStart() + actionManager.getActions());
           if(timer.isTimeOver() && !roundOver)
        {
            if(numTasksRequired[task] == actionManager.getActions())
            {
                scoreManager.addPoint();
                feedback.provideFeedback(true);
                roundOver = true;
            }
            else
            {
                scoreManager.subtractPoint();
                feedback.provideFeedback(false);
                roundOver = true;
            }

        }

            if (countdown > 0 && roundOver)
            {
                countdown -= Time.deltaTime;
               
            }
            else if(countdown <= 0 && roundOver)
            {
                countdown = waitBetweenRounds;
                roundOver = false;
                resetRequiredTasks();
                timer.resetTime();
                //resetPlayerStats();
                actionManager.resetActions();
                loopGenerator.generateLoop();
            }
        
        }
	

    /*
     public void resetPlayerStats()
    {
        for(int i = 0; i < player.stats.Length; i++)
        {
            player.stats[i] = 0;
        }
    }
     

    public void updateTasksPerformed()
    {
        for(int i = 0; i < numTasksPerformed.Length; i++ )
        {
            numTasksPerformed[i] = player.stats[i];
        }
    }
     
     * 
     */

    public bool compareArrays(int[] array1, int[] array2)
    {
        if(array1.Length != array2.Length)
        {
            return false;
        }

        for(int i = 0; i < array1.Length; i++)
        {
            if(array1[i] != array2[i])
            {
                return false;

            }
        }

        return true;
    }

    public void resetRequiredTasks()
    {
        for(int i = 0; i < numTasksRequired.Length; i++)
        {
            numTasksRequired[i] = 0;
        }
    }
    
    public void setTask(int newTask)
    {
        task = newTask;
    }

    public int getTask()
    {
        return task;
    }

}


