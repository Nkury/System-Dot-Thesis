using UnityEngine;
using System.Collections;

public class LevelTitle : MonoBehaviour {

    bool once = true;
	// Use this for initialization
	void Start () {
        StartCoroutine(disappear());
	}

    // Update is called once per frame
    void Update() {

        if (transform.localScale.x < 1f)
            transform.localScale += Vector3.one * .0051f;

    }

    public IEnumerator disappear()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
