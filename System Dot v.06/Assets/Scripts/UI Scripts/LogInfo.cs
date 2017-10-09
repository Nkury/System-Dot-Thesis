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
        numAPIOpen.text = PlayerStats.log_numAPIOpen.ToString();
        numSyntaxErrors.text = PlayerStats.log_numSyntaxErrors.ToString();
        numPerfectEdits.text = PlayerStats.log_numPerfectEdits.ToString();
        numF5s.text = PlayerStats.log_numOfF5.ToString();
        numLegacyCodeViewed.text = PlayerStats.log_numLegacyCodeViewed.ToString();
        numQuickDebug.text = PlayerStats.log_numQuickDebug.ToString();
        totalModifiedEdits.text = PlayerStats.log_totalNumberOfModifiedEdits.ToString();
        totalNumLegacy.text = PlayerStats.log_totalNumberOfLegacyOnly.ToString();
    }
}
