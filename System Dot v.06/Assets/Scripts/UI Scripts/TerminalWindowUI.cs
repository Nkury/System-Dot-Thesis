using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class TerminalWindowUI : MonoBehaviour {

    public GameObject rightParticleSystem;
    public GameObject wrongParticleSystem;
    public GameObject noChanges;
    public GameObject F5Ref;
    public GameObject variabullRef;
    public GameObject variaCode;

    public GameObject logInformation;
    public APISystem API;

    // Use this for initialization
    void Start () {
        Slider healthBar = transform.Find("Player Info Panel/Health Bar").GetComponent<Slider>();
        API = GameObject.FindObjectOfType<APISystem>();
	}
	
	// Update is called once per frame
	void Update () {

        if(F5Ref.activeSelf && EnemyTerminal.globalTerminalMode < 2)
        {
            F5Ref.SetActive(false);
        }    

        if(EnemyTerminal.globalTerminalMode >= 2)
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                LogToFile.WriteToFile("DEBUG-F5", "TERMINAL_WINDOW");
                LogHelper.SetDictionaryValue(PlayerStats.log_numOfF5, LogHelper.GetDictionaryValue(PlayerStats.log_numOfF5) + 1);   
                debugClicked();
            } 

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                pressArrowKeys(true);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                pressArrowKeys(false);
            }            
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                logInformation.SetActive(!logInformation.activeSelf);
            }
        }        
	}

    public void exitClicked()
    {
        if (API.APImenu.activeSelf)
        {
            API.APIButtonClicked();
        }
        LogToFile.WriteToFile("CLOSE-TERMINAL-WINDOW", "TERMINAL-WINDOW");
        EnemyTerminal.globalTerminalMode--;
        if (EnemyTerminal.madeChanges)
        {
            // LOGGER INFO
            LogHelper.SetDictionaryValue(PlayerStats.log_totalNumberOfModifiedEdits, LogHelper.GetDictionaryValue(PlayerStats.log_totalNumberOfModifiedEdits) + 1);

            if (EnemyTerminal.APIUsed)
            {
                LogHelper.SetDictionaryValue(PlayerStats.log_numAPIOpen, LogHelper.GetDictionaryValue(PlayerStats.log_numAPIOpen) + 1);
                EnemyTerminal.APIUsed = false;
            }
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
        LogToFile.WriteToFile("DEBUG-CLICKED", "TERMINAL_WINDOW");

        LogHelper.SetDictionaryValue(PlayerStats.log_totalNumDebugs, LogHelper.GetDictionaryValue(PlayerStats.log_totalNumDebugs) + 1);
       
        // LOGGER INFO
        if (EnemyTerminal.madeChanges)
        {          
            if (EnemyTerminal.timeBetweenCodeChangeAndDebug < PlayerStats.timeToDebugThreshhold)
            {
                LogHelper.SetDictionaryValue(PlayerStats.log_numQuickDebug, LogHelper.GetDictionaryValue(PlayerStats.log_numQuickDebug) + 1);       
            }

            if (EnemyTerminal.APIUsed) {
                LogHelper.SetDictionaryValue(PlayerStats.log_numAPIOpen, LogHelper.GetDictionaryValue(PlayerStats.log_numAPIOpen) + 1);
                EnemyTerminal.APIUsed = false;
            }

            LogHelper.SetDictionaryValue(PlayerStats.log_totalNumberOfModifiedEdits, LogHelper.GetDictionaryValue(PlayerStats.log_totalNumberOfModifiedEdits) + 1);
        }

        if (API.APImenu.activeSelf)
        {
            API.APIButtonClicked();
        }

        EnemyTerminal[] enemies = FindObjectsOfType<EnemyTerminal>();
        foreach (EnemyTerminal e in enemies)
        {
            if (e.localTerminalMode >= 2)
            {         
                e.GetComponent<EnemyTerminal>().checkTerminalString();
                StartCoroutine(e.GetComponent<EnemyTerminal>().evaluateActions());
                UnityEngine.Object pe;
                if (e.GetComponent<EnemyTerminal>().actions.Contains(ParserAlgo.keyActions.ERROR)){
                    GameObject.Find("Sound Controller").GetComponent<SoundController>().play("wrong");
                    pe = Instantiate(wrongParticleSystem, e.gameObject.transform.position, e.gameObject.transform.rotation);

                    /* LOGGER INFORMATION */
                    e.isPerfect = false;
                    PlayerStats.mostNumberofAttempts++;
                    if (EnemyTerminal.madeChanges)
                    {
                        LogHelper.SetDictionaryValue(PlayerStats.log_numSyntaxErrors, LogHelper.GetDictionaryValue(PlayerStats.log_numSyntaxErrors) + 1);                   
                        EnemyTerminal.madeChanges = false;
                    }
                    LogToFile.WriteToFile("WRONG-SYNTAX", "CODE");
                }
                else
                {
                    GameObject.Find("Sound Controller").GetComponent<SoundController>().play("right");
                    pe = Instantiate(rightParticleSystem, e.gameObject.transform.position, e.gameObject.transform.rotation);

                    /* LOGGER INFORMATION */
                    if (EnemyTerminal.madeChanges)
                    {                        
                        if (e.isPerfect)
                        {
                            LogToFile.WriteToFile("CORRECT-SYNTAX", "CODE");
                            PlayerStats.numberOfPerfectEdits++;
                            LogHelper.SetDictionaryValue(PlayerStats.log_numPerfectEdits, LogHelper.GetDictionaryValue(PlayerStats.log_numPerfectEdits) + 1);                        
                        }
                    }
                }

                Destroy(pe, 1);
                EnemyTerminal.globalTerminalMode--;
                if(e.gameObject.name == "TutorialEnemy2")
                {
                    if (GameObject.Find("clickAPI"))
                    {
                        GameObject.Find("clickAPI").SetActive(false);
                    }
                    GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().SetDialogue("colorChanged");
                }
            }
        }
        
        EnemyTerminal.madeChanges = false;
    }

    // when the player presses on an input field, it will go to the next one below it
    // the parameter takes in the current line number
    public void pressEnter(int lineNo)
    {
        LogToFile.WriteToFile("PRESSED-ENTER", "TERMINAL_WINDOW");
        switch (lineNo)
        {
            case 1:
                if (transform.Find("Terminal Window/line 2"))
                {
                    InputField line2 = transform.Find("Terminal Window/line 2").GetComponent<InputField>();
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(line2.gameObject, null);
                    StartCoroutine(MoveTextEnd_NextFrame(line2));
                }
                break;
            case 2:
                if (transform.Find("Terminal Window/line 3"))
                {
                    InputField line3 = transform.Find("Terminal Window/line 3").GetComponent<InputField>();
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(line3.gameObject, null);
                    StartCoroutine(MoveTextEnd_NextFrame(line3));
                }
                break;
            case 3:
                if (transform.Find("Terminal Window/line 4"))
                {
                    InputField line4 = transform.Find("Terminal Window/line 4").GetComponent<InputField>();
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(line4.gameObject, null);
                    StartCoroutine(MoveTextEnd_NextFrame(line4));
                }
                break;
            case 4:
                if (transform.Find("Terminal Window/line 5"))
                {
                    InputField line5 = transform.Find("Terminal Window/line 5").GetComponent<InputField>();
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(line5.gameObject, null);
                    StartCoroutine(MoveTextEnd_NextFrame(line5));
                }
                break;

        }
    }

    public void pressArrowKeys(bool isDownPressed)
    {
        if (isDownPressed)
        {
            LogToFile.WriteToFile("PRESSED-DOWN-KEY", "TERMINAL_WINDOW");
            string selectedObjectName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            pressEnter(Int32.Parse(selectedObjectName.Substring(selectedObjectName.Length - 1, 1)));
        }
        else
        {
            LogToFile.WriteToFile("PRESSED-UP-KEY", "TERMINAL_WINDOW");
            string selectedObjectName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            if (selectedObjectName != "line 1" && selectedObjectName != "line 2")
            {
                pressEnter(Int32.Parse(selectedObjectName.Substring(selectedObjectName.Length - 1, 1)) -2);
            } else if(selectedObjectName == "line 2")
            {
                InputField line1 = transform.Find("Terminal Window/line 1").GetComponent<InputField>();
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(line1.gameObject, null);
                StartCoroutine(MoveTextEnd_NextFrame(line1));
            }
        }
    }

    IEnumerator MoveTextEnd_NextFrame(InputField lineNO)
    {
        yield return 0; // Skip the first frame in which this is called.
        lineNO.MoveTextEnd(false); // Do this during the next frame.
    }

    public void F5(bool appear)
    {
        //LogToFile.WriteToFile("MOUSE-OVER-DEBUG-BUTTON", "TERMINAL_WINDOW"); not needed
        F5Ref.SetActive(appear);
    }

    public void setVariabullCode(string statement)
    {
        variabullRef.SetActive(true);
        variaCode.GetComponent<Text>().text = statement;     
    }
}
