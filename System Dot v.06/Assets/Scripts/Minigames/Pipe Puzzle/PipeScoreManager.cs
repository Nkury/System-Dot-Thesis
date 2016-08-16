using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PipeScoreManager : MonoBehaviour
{

    private Text text;

    public DebrisDestroyer trueDestroyer;
    public DebrisDestroyer falseDestroyer;

    private ConditionalGenerator conditionalGenerator;
    private PipeManager pipeManager;

    private PlayerFeedbackManager feedback;

    private int score;


    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        conditionalGenerator = FindObjectOfType<ConditionalGenerator>();
        pipeManager = FindObjectOfType<PipeManager>();
        feedback = FindObjectOfType<PlayerFeedbackManager>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public int getScore()
    {
        return score;
    }

    public void scoreCheck()
    {

        if (conditionalGenerator.getAnswer() == true && pipeManager.getPipe())
        {
            score++;
            feedback.provideFeedback(true);
            
        }
        else if (conditionalGenerator.getAnswer() == false && !pipeManager.getPipe())
        {
            score++;
            feedback.provideFeedback(true);

        }
        else
        {
            score--;
            if (score <= 0)
            {
                score = 0;
            }

            feedback.provideFeedback(false);
           
        }

        text.text = "" + score;
    }
}

