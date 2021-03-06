﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

        int numObj;
        if (!PlayerStats.log_totalNumberOfObjects.TryGetValue(SceneManager.GetActiveScene().name, out numObj))
        {
            PlayerStats.log_totalNumberOfObjects[SceneManager.GetActiveScene().name] = GameObject.FindObjectsOfType<EnemyTerminal>().Length;
        }

        // if we enter a new level
        if (PlayerStats.levelName != SceneManager.GetActiveScene().name)
        {
            PlayerStats.levelName = SceneManager.GetActiveScene().name;
            PlayerStats.highestCheckpoint = 1;
            if (PlayerStats.levelName != "LVL1 BOSS")
            {
                PlayerStats.checkpoint = "Checkpoint1";
            }
            PlayerStats.deadObjects.Clear();
            PlayerStats.terminalStrings.Clear();
        }

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
        LogToFile.WriteToFile("PLAYER-DIES", "PLAYER-" + PlayerStats.playerName);
        Instantiate(deathParticle, player.transform.position, player.transform.rotation);
        player.enabled = false;
        player.GetComponent<Renderer>().enabled = false;
        player.gameObject.GetComponent<CircleCollider2D>().enabled = false;
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
        player.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        HealthManager.FullHealth();
        HealthManager.isDead = false;
        camera.isFollowing = true;
        Instantiate(respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);
    }

    public virtual void LoadLevel()
    {

    }
}
