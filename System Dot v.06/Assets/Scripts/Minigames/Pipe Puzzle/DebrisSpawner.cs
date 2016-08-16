using UnityEngine;
using System.Collections;

public class DebrisSpawner : MonoBehaviour
{

    public GameObject debris;
    public ConditionalGenerator conditionalGen;
    public DebrisDestroyer trueDestroyer;
    public DebrisDestroyer falseDestroyer;


    public float spawnDelay;
    private float countdown;


    // Use this for initialization
    void Start()
    {
        conditionalGen = FindObjectOfType<ConditionalGenerator>();
       
        countdown = spawnDelay;

        Instantiate(debris, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        /*
               if(countdown == spawnTimer)
               {
                   Instantiate(debris, transform.position, transform.rotation);
                   conditionalGen.generateStatement();
               }

               countdown -= Time.deltaTime;
            
               if(countdown <= 0)
               {
                   countdown = spawnTimer;
               }
        }
         */

        if(trueDestroyer.destroyed == true || falseDestroyer.destroyed == true)
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0)
            {
                Instantiate(debris, transform.position, transform.rotation);
                conditionalGen.generateStatement();
                countdown = spawnDelay;
                trueDestroyer.destroyed = false;
                falseDestroyer.destroyed = false;
            }
            

        }
    }
}

