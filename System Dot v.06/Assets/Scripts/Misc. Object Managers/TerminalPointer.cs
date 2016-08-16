using UnityEngine;
using System.Collections;

public class TerminalPointer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(Screen.width, Screen.height * 5/6, 10);
        this.transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}
