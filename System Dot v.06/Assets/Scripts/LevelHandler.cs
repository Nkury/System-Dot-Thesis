using UnityEngine;
using System.Collections;

public class LevelHandler : MonoBehaviour
{

    public GameObject currentCheckpoint;

    protected PlayerController player;

    public GameObject deathParticle;
    public GameObject respawnParticle;

    public int pointsPenaltyOnDeath;

    public float respawnDelay;

    protected CameraController camera;

    protected float gravityStore;

    public HealthManager healthManager;   
    
    protected bool loadedIn = false;

    public static bool canPressTab = true;

    // Use this for initialization
    public void Start()
    {
        player = FindObjectOfType<PlayerController>();

        camera = FindObjectOfType<CameraController>();

        healthManager = FindObjectOfType<HealthManager>();
        LoadLevel();
        player.transform.position = currentCheckpoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RespawnPlayer()
    {
        StartCoroutine("RespawnPlayerCo");
    }

    public IEnumerator RespawnPlayerCo()
    {
        Instantiate(deathParticle, player.transform.position, player.transform.rotation);
        player.enabled = false;
        player.GetComponent<Renderer>().enabled = false;
        camera.isFollowing = false;
        //gravityStore = player.GetComponent<Rigidbody2D> ().gravityScale;
        //player.GetComponent<Rigidbody2D> ().gravityScale = 0f;
        //player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
        Debug.Log("Player Respawn");
        yield return new WaitForSeconds(respawnDelay);
        //player.GetComponent<Rigidbody2D> ().gravityScale = gravityStore;

        // COMMENT/UNCOMMENT THIS AFTER TESTING
        if (!loadedIn)
        {
            LoadLevel();
        }

        player.transform.position = currentCheckpoint.transform.position;

        player.knockbackCount = 0;
        player.enabled = true;
        player.GetComponent<Renderer>().enabled = true;
        HealthManager.FullHealth();
        healthManager.isDead = false;
        camera.isFollowing = true;
        Instantiate(respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);
    }

    public virtual void LoadLevel()
    {

    }
}
