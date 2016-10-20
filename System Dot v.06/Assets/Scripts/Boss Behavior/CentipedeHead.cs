using UnityEngine;
using System.Collections;

public class CentipedeHead : MonoBehaviour {


    public int speed = 3;
    public bool down = false;
    public bool coll = true;
    public bool canShootSpike = true;
    public static int lives = 18;
    public static int life = 1;

    public GameObject spike;
    public GameObject body;

    private bool spawnOne = true;
    private bool death = true;

    private int prevLives;

    [Header("Environment Objects")]
    public GameObject ground1;
    public GameObject ground2;
    public GameObject exitHallway;

	// Use this for initialization
	void Start () {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (BossIntellisense.startBoss)
        {
            if (canShootSpike)
            {
                StartCoroutine(shootSpike());
            }
            this.transform.Translate(new Vector3(-.01f * speed, 0, 0));

            if (lives <= 0 && death)
            {
                death = false;
                GameObject[] remainingBodies = GameObject.FindGameObjectsWithTag("Centipede Body");
                foreach (GameObject body in remainingBodies)
                {
                    if (body.name != "Centipede Head")
                        Destroy(body);
                }
                ground1.SetActive(false);
                ground2.SetActive(false);
                exitHallway.SetActive(true);

                Camera.main.GetComponents<AudioSource>()[0].mute = true;
                Camera.main.GetComponents<AudioSource>()[2].Play(); // play victory theme

                //// return camera to normal
                //Camera.main.orthographicSize = 3.675071f;
                //Camera.main.GetComponent<CameraController>().yOffset = 0;
                //Camera.main.GetComponent<CameraController>().xOffset = 0;
                //Camera.main.GetComponent<CameraController>().isFollowing = true;

                this.GetComponent<EnemyHealthManager>().giveDamage(7);
            }
            else if (lives < 5)
            {
                speed = 10;
                if (prevLives != lives)
                {
                    Instantiate(body, new Vector3(Random.Range(28.74f, 39.094f), 15.655f, 0), body.transform.rotation);

                }
            }
            else if (lives < 12)
            {
                speed = 8;
                if (prevLives != lives)
                {
                    Instantiate(body, new Vector3(Random.Range(28.74f, 39.094f), 15.655f, 0), body.transform.rotation);

                }
            }
            else if (lives < 15 && spawnOne)
            {
                spawnOne = false;
                speed = 5;
                GameObject[] bodies = new GameObject[4];
                bodies[0] = (GameObject)Instantiate(body, new Vector3(35.714f, 15.655f, 0), body.transform.rotation);
                bodies[1] = (GameObject)Instantiate(body, new Vector3(36.8344f, 15.655f, 0), body.transform.rotation);
                bodies[2] = (GameObject)Instantiate(body, new Vector3(37.954f, 15.655f, 0), body.transform.rotation);
                bodies[3] = (GameObject)Instantiate(body, new Vector3(39.094f, 15.655f, 0), body.transform.rotation);
                foreach (GameObject g in bodies)
                {
                    g.GetComponent<EnemyTerminal>().checkTerminalString();
                    g.GetComponent<EnemyTerminal>().evaluateActions();
                }
            }

            prevLives = lives;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (coll && other.gameObject.tag == "Centipede Trigger" && other.gameObject.name.Substring(0, 1) == "A")
        {
            this.transform.Rotate(0, 0, 90);
            coll = false;
        }
        else if(!coll && other.gameObject.tag == "Centipede Trigger" && other.gameObject.name.Substring(0, 1) == "B")
        {
            this.transform.Rotate(0, 0, 90);
            coll = true;               
        }
    }

    public IEnumerator shootSpike()
    {
        canShootSpike = false;
        float random = Random.Range(0, 2.0f);
        yield return new WaitForSeconds(.5f + random);
        canShootSpike = true;
        Instantiate(spike, this.transform.position + new Vector3(0, -2), spike.transform.rotation);
    }
}
