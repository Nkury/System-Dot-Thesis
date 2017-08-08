using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Variabull : MonoBehaviour {

    public string statement;    

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject.Find("Main HUD").GetComponent<TerminalWindowUI>().setVariabullCode(statement);
            PlayerStats.deadObjects.Add(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
