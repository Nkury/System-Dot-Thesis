using UnityEngine;
using System.Collections;

public class TerminalWindowUI : MonoBehaviour {

    public GameObject rightParticleSystem;
    public GameObject wrongParticleSystem;
    public GameObject noChanges;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void exitClicked()
    {
        EnemyTerminal.globalTerminalMode--;
        if (EnemyTerminal.madeChanges)
        {
            EnemyTerminal.madeChanges = false;
            noChanges.SetActive(true);
            StartCoroutine(setToFalse());
        }
    }

    public IEnumerator setToFalse()
    {
        yield return new WaitForSeconds(2.8f);
        noChanges.SetActive(false);
    }

    public void debugClicked()
    {
        EnemyTerminal[] enemies = FindObjectsOfType<EnemyTerminal>();
        foreach (EnemyTerminal e in enemies)
        {
            if (e.localTerminalMode >= 2)
            {         
                e.GetComponent<EnemyTerminal>().checkTerminalString();
                StartCoroutine(e.GetComponent<EnemyTerminal>().evaluateActions());
                Object pe;
                if (e.GetComponent<EnemyTerminal>().actions.Contains(ParserAlgo.keyActions.ERROR)){
                     pe = Instantiate(wrongParticleSystem, e.gameObject.transform.position, e.gameObject.transform.rotation);
                }
                else
                {
                     pe = Instantiate(rightParticleSystem, e.gameObject.transform.position, e.gameObject.transform.rotation);
                }

                Destroy(pe, 1);
                EnemyTerminal.globalTerminalMode--;
                if(e.gameObject.name == "TutorialEnemy2")
                {
                    GameObject.Find("clickAPI").SetActive(false);
                    GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().debugClicked();
                }
            }
        }
    }
}
