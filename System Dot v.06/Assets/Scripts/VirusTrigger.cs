using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusTrigger : MonoBehaviour
{
    public GameObject closestVirus;

    public List<GameObject> viruses = new List<GameObject>();
    
    private float closestDistance = Mathf.Infinity;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (viruses.Count > 1)
        {
            FindClosestVirus();
        }
        else if(viruses.Count == 1)
        {
            closestVirus = viruses[0];
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Virus>())
        {
            viruses.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Virus>())
        {
            viruses.Remove(other.gameObject);
        }
    }

    void FindClosestVirus()
    {
        closestDistance = Mathf.Infinity;
        foreach (GameObject virus in viruses)
        {
            // this.transform.parent = "Player"
            float distanceToVirus = Vector2.Distance(this.transform.parent.transform.position, virus.transform.position);
            if(distanceToVirus < closestDistance)
            {
                closestDistance = distanceToVirus;
                closestVirus = virus;
            }
        }
    }
}


