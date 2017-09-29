using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltPlatform : MonoBehaviour {

    public string condition;

    private bool isEvaluating = true;

    ParserAlgo.Parser parser = new ParserAlgo.Parser();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        condition = this.gameObject.transform.FindChild("condition").GetComponent<TextMesh>().text;
	}

    public void evaluateCondition(char value)
    {
        List<ParserAlgo.keyActions> actions;
        if (char.IsNumber(value))
        {
            actions = parser.Parse("if(" + value + " " + condition + ") { System.gravity(true); } else { System.gravity(false); }");
        } else
        {
            actions = parser.Parse("if(\"" + value + "\" " + condition + ") { System.gravity(true); } else { System.gravity(false); }");
        }

        if (actions.Contains(ParserAlgo.keyActions.GRAVITYON)){
            this.transform.Rotate(new Vector3(0, 0, -25));
        } else if (actions.Contains(ParserAlgo.keyActions.GRAVITYOFF))
        {
            this.transform.Rotate(new Vector3(0, 0, 25));
        }
        else if (actions.Contains(ParserAlgo.keyActions.ERROR))
        {
            this.transform.Rotate(new Vector3(0, 0, 0));
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<NumberBall>() && isEvaluating)
        {
            evaluateCondition(other.gameObject.GetComponent<NumberBall>().character);
            isEvaluating = false;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<NumberBall>() && !isEvaluating)
        {
            this.transform.Rotate(new Vector3(0, 0, -this.transform.rotation.eulerAngles.z));
            isEvaluating = true;            
        }
    }
}
