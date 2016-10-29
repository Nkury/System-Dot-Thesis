 using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpoint;

	private PlayerController player;

	public GameObject deathParticle;
	public GameObject respawnParticle;

	public int pointsPenaltyOnDeath;

	public float respawnDelay;

	private CameraController camera;

	private float gravityStore;

	public HealthManager healthManager;

    [Header("Level 1 Checkpoints")]
    public GameObject checkpoint1;
    public GameObject checkpoint2;
    public GameObject checkpoint3;
    public GameObject checkpoint4;
    public GameObject checkpoint5;

    [Header("Level 1 Boss Checkpoint")]
    public GameObject checkpoint6;

    [Header("Miscellaneous")]
    public GameObject intelliSense;
    public GameObject APIButton;
    public GameObject directionHelpButton;
    public GameObject chestHelpButton;
    public GameObject DebugButton;

    public static bool canPressTab = true;


    private bool loadedIn = false;
    // Use this for initialization
    void Start () {
		player = FindObjectOfType<PlayerController> ();

		camera = FindObjectOfType<CameraController> ();

		healthManager = FindObjectOfType<HealthManager> ();
        LoadLevel();
        player.transform.position = currentCheckpoint.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RespawnPlayer()
	{
		StartCoroutine ("RespawnPlayerCo");
	}

	public IEnumerator RespawnPlayerCo()
	{
		Instantiate (deathParticle, player.transform.position, player.transform.rotation);
		player.enabled = false;
		player.GetComponent<Renderer>().enabled = false;
		camera.isFollowing = false;
		//gravityStore = player.GetComponent<Rigidbody2D> ().gravityScale;
		//player.GetComponent<Rigidbody2D> ().gravityScale = 0f;
		//player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		Debug.Log ("Player Respawn");
		yield return new WaitForSeconds (respawnDelay);
        //player.GetComponent<Rigidbody2D> ().gravityScale = gravityStore;

        // COMMENT/UNCOMMENT THIS AFTER TESTING
        if (!loadedIn) { 
            LoadLevel();
        }

        player.transform.position = currentCheckpoint.transform.position;

		player.knockbackCount = 0;
		player.enabled = true;
		player.GetComponent<Renderer>().enabled = true;
		HealthManager.FullHealth ();
		healthManager.isDead = false;
		camera.isFollowing = true;
		Instantiate (respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);
	}

    public void LoadLevel()
    {
        // set it to false so we don't have to go through the tutorial
        loadedIn = true;
        switch (PlayerStats.checkpoint)
        {
            case "Checkpoint1":
                currentCheckpoint = checkpoint1;
                intelliSense.SetActive(true);
                APIButton.SetActive(false);
                DebugButton.SetActive(false);
                directionHelpButton.SetActive(false);
                chestHelpButton.SetActive(false);
                IntelliSenseTest.clickOnce = false;
                APISystem.clicked = false;
                canPressTab = false; // cannot press tab to switch boots to serve the tutorial
                break;
            case "Checkpoint2":
                APIButton.SetActive(false);
                DebugButton.SetActive(false);
                IntelliSenseTest.clickOnce = true;
                directionHelpButton.SetActive(false);
                chestHelpButton.SetActive(false);
                APISystem.clicked = false;
                currentCheckpoint = checkpoint2;
                break;
            case "Checkpoint3":
                IntelliSenseTest.clickOnce = true;
                directionHelpButton.SetActive(false);
                currentCheckpoint = checkpoint3;
                break;
            case "Checkpoint4":
                IntelliSenseTest.clickOnce = false;
                currentCheckpoint = checkpoint4;
                break;
            case "Checkpoint5":
                currentCheckpoint = checkpoint5;
                break;
            case "Checkpoint6":
                currentCheckpoint = checkpoint6;
                break;
        }
    }
}
