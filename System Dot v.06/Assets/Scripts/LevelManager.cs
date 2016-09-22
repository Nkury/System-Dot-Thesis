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

    public GameObject checkpoint1;
    public GameObject checkpoint2;
    public GameObject checkpoint3;
    public GameObject checkpoint4;
    public GameObject checkpoint5;
    public GameObject intelliSense;

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
        LoadLevel();
		player.transform.position = currentCheckpoint.transform.position;

		player.knockbackCount = 0;
		player.enabled = true;
		player.GetComponent<Renderer>().enabled = true;
		healthManager.FullHealth ();
		healthManager.isDead = false;
		camera.isFollowing = true;
		Instantiate (respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);
	}

    public void LoadLevel()
    {
       // set it to false so we don't have to go through the tutorial

        Debug.Log(PlayerStats.checkpoint);
        switch (PlayerStats.checkpoint)
        {
            case "Checkpoint1":
                currentCheckpoint = checkpoint1;
                intelliSense.SetActive(true);
                intelliSense.SendMessage("startTutorialPart");
                break;
            case "Checkpoint2": 
                currentCheckpoint = checkpoint2;
                break;
            case "Checkpoint3":
                currentCheckpoint = checkpoint3;
                break;
            case "Checkpoint4":
                currentCheckpoint = checkpoint4;
                break;
            case "Checkpoint5":
                currentCheckpoint = checkpoint5;
                break;
        }
    }
}
