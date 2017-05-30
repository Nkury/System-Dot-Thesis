using UnityEngine;
using System.Collections;

public class HoverOverDebug : MonoBehaviour {

    public GameObject F5Ref;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseOver()
    {
        F5Ref.SetActive(true);
    }

    public void OnMouseExit()
    {
        F5Ref.SetActive(false);
    }
}
