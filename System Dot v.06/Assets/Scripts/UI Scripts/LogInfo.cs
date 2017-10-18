using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogInfo : MonoBehaviour {

    public Text numAPIOpen;
    public Text numSyntaxErrors;
    public Text numPerfectEdits;
    public Text numF5s;
    public Text numLegacyCodeViewed;
    public Text numQuickDebug;
    public Text totalModifiedEdits;
    public Text totalNumLegacy;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        numAPIOpen.text = LogHelper.GetDictionaryValue(PlayerStats.log_numAPIOpen).ToString();                                                      
        numSyntaxErrors.text = LogHelper.GetDictionaryValue(PlayerStats.log_numSyntaxErrors).ToString();  
        numPerfectEdits.text = LogHelper.GetDictionaryValue(PlayerStats.log_numPerfectEdits).ToString();  
        numF5s.text = LogHelper.GetDictionaryValue(PlayerStats.log_numOfF5).ToString();               
        numLegacyCodeViewed.text = LogHelper.GetDictionaryValue(PlayerStats.log_numLegacyCodeViewed).ToString();  
        numQuickDebug.text = LogHelper.GetDictionaryValue( PlayerStats.log_numQuickDebug).ToString();    
        totalModifiedEdits.text = LogHelper.GetDictionaryValue(PlayerStats.log_totalNumberOfModifiedEdits).ToString();
        totalNumLegacy.text = LogHelper.GetDictionaryValue(PlayerStats.log_totalNumberOfEdits).ToString();  
    }
}
