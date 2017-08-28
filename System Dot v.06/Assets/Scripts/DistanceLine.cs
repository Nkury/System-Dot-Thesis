using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceLine : MonoBehaviour {

    public float lineWidth;
    public float triggerRadius;
    public int numberOfConnectedObjects;
    public float maxDistance = 11; // in case the target has no collider, still make line disappear
    public GameObject distanceObject;
    public List<string> parameterNames = new List<string>();

    public List<GameObject> target = new List<GameObject>();
    private GameObject source;
  
    private List<LineRenderer> line = new List<LineRenderer>();
    private CircleCollider2D lineTrigger;
    private List<GameObject> distanceObjects = new List<GameObject>();
   
    private float distanceBetweenObjects;
    

	// Use this for initialization
	void Start () {

        source = this.gameObject;

        for (int i = 0; i < target.Count; i++) {
            // add line renderer to the game object
            line.Add(new GameObject().AddComponent<LineRenderer>());
            // set width of line
            line[i].startWidth = line[i].endWidth = lineWidth;
            // set the number of vertices for the line renderer
            line[i].numPositions = 2;

            // instantiate this many text objects for the lines
            distanceObjects.Add(Instantiate(distanceObject));
            // get the text to be over the object
            distanceObjects[i].AddComponent<TextOverSprite>();

            // disable distances at start 
            line[i].enabled = false;
            distanceObjects[i].GetComponent<MeshRenderer>().enabled = false;
        }

        // add a trigger zone for the line to appear and disappear
        lineTrigger = this.gameObject.AddComponent<CircleCollider2D>();
        // set the radius of circle collider and reset offsets
        lineTrigger.radius = triggerRadius;
        lineTrigger.offset = new Vector2(0, 0);
        // set the circle collider as a trigger
        lineTrigger.isTrigger = true;     

        // set the parameter for the attached EnemyTerminal script
        for(int i = 0; i < target.Count; i++) {
            if (this.transform.parent.GetComponent<EnemyTerminal>())
            {
                this.transform.parent.GetComponent<EnemyTerminal>().parameters += "string " + parameterNames[i] + " = \"" + target[i].name + "\"; ";
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null && source != null)
        {
            for (int i = 0; i < target.Count; i++)
            {
                line[i].SetPosition(0, source.transform.position);
                line[i].SetPosition(1, target[i].transform.position);

                // responsible for distance being displayed on the line
                distanceBetweenObjects = Vector2.Distance(source.transform.position, target[i].transform.position);
                distanceBetweenObjects = (float)Math.Round(distanceBetweenObjects, 2);
                distanceObjects[i].GetComponent<TextMesh>().text = distanceBetweenObjects.ToString();
                distanceObjects[i].transform.position = (source.transform.position + target[i].transform.position) / 2;
                if(distanceBetweenObjects > maxDistance)
                {                   
                    line[i].enabled = false;
                    distanceObjects[i].GetComponent<MeshRenderer>().enabled = false;
                }              
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (target.Contains(other.gameObject))
        {
            int index = target.IndexOf(other.gameObject);
            line[index].enabled = true;
            distanceObjects[index].GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (target.Contains(other.gameObject))
        {
            int index = target.IndexOf(other.gameObject);
            line[index].enabled = true;
            distanceObjects[index].GetComponent<MeshRenderer>().enabled = true;
        }  
       
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (target.Contains(other.gameObject))
        {
            int index = target.IndexOf(other.gameObject);
            line[index].enabled = false;
            distanceObjects[index].GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
