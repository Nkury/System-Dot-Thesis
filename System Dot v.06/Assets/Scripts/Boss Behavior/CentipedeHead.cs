using UnityEngine;
using System.Collections;

public class CentipedeHead : MonoBehaviour {


    public int speed = 3;
    public bool down = false;
    public bool coll = true;
    public bool canShootSpike = true;

    public GameObject spike;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (canShootSpike)
        {
            StartCoroutine(shootSpike());
        }
        this.transform.Translate(new Vector3(-.01f * speed, 0, 0));
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground" && coll) {
            StartCoroutine(moveDown());
        }
    }

    public IEnumerator moveDown()
    {
        this.transform.Rotate(new Vector3(0, 0, 90));
        coll = false;
        yield return new WaitForSeconds(2);
        this.transform.Rotate(new Vector3(0, 0, 90));
        yield return new WaitForSeconds(.5f);
        coll = true;
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
