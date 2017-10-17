using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level2BossBehavior : MonoBehaviour
{

    public GameObject levelExit;
    public GameObject bossAttack;
    public GameObject topSpikes;
    public Slider bossHealth;

    public float intervalToSpike;
    public float rotateSpeed;
    public float attackSpeed;
    public int health;
    public float maxX; // minX = -maxX
    public float maxY; // minY = -maxY

    public bool damaged = true;
    public float angle;

    float time;

    public float zRotation = 0;
    public Vector2 locationToSpawn;

    // Use this for initialization
    void Start()
    {
        health = this.GetComponent<EnemyHealthManager>().enemyHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (health != this.GetComponent<EnemyHealthManager>().enemyHealth)
        {
            health = this.GetComponent<EnemyHealthManager>().enemyHealth;
            if (health <= 4)
            {
                SetUp();
                StartCoroutine(AttackPhase(health));
            }
        }

        if (health != 3)
        {
            this.GetComponent<Rotater>().maxRotation += 1000;
            this.GetComponent<Rotater>().pause = false;
        }

        bossHealth.value = health;
    }

    // Update at fixed frames
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= intervalToSpike)
        {
            Spike();
        }

        // equation of ellipse: x = center.x + major_axis * cos(angle)
        locationToSpawn.x = this.transform.position.x + maxX * Mathf.Cos(zRotation * Mathf.PI / 180);
        // equation of ellipse: y = center.y + minor_axis * sin(angle)
        locationToSpawn.y = this.transform.position.y + maxY * Mathf.Sin(zRotation * Mathf.PI / 180);
        zRotation = (zRotation + rotateSpeed) % 360;
    }

    public void Spike()
    {
        GameObject obj = Instantiate(bossAttack, locationToSpawn, Quaternion.Euler(new Vector3(0, 0, zRotation - 90)));
        obj.GetComponent<MoveInDirectionPointing>().speed = attackSpeed;
        time = 0;
    }

    public void SetUp()
    {
        // set the camera to zoom out
        Camera.main.GetComponent<CameraController>().enabled = false;
        Camera.main.transform.position = new Vector3(33.82f, 10.9f, -2);
        Camera.main.orthographicSize = 14;
        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        EnemyTerminal.globalTerminalMode = 0;
        Array.Clear(this.GetComponent<EnemyTerminal>().terminalString, 0, 5);
    }

    public IEnumerator AttackPhase(int phase)
    {
        switch (phase)
        {
            case 4:
                StartCoroutine(MegaAttack(2, 1, 5));
                yield return new WaitForSeconds(15);
                attackSpeed = 0.05f;
                StartCoroutine(MegaAttack(1, 2, 5));
                yield return new WaitForSeconds(15);
                intervalToSpike = .8f;
                this.GetComponent<EnemyTerminal>().terminalString[0] = "boolean b;";
                this.GetComponent<EnemyTerminal>().terminalString[1] = "boolean power = (var && !b);";
                this.GetComponent<EnemyTerminal>().terminalString[2] = "System.output(power);";
                break;
            case 3:
                topSpikes.SetActive(false);
                attackSpeed = 0.04f;
                rotateSpeed = 15;
                StartCoroutine(MegaAttack(1, 3, 5));
                yield return new WaitForSeconds(20);
                rotateSpeed = 2;
                attackSpeed = 0.045f;
                StartCoroutine(MegaAttack(1, .5f, 10));
                yield return new WaitForSeconds(15);
                intervalToSpike = .6f;
                this.GetComponent<Rotater>().maxRotation = 0;
                this.GetComponent<EnemyTerminal>().terminalString[0] = "System.rotate(Direction.DOWN);";
                this.GetComponent<EnemyTerminal>().terminalString[1] = "";
                this.GetComponent<EnemyTerminal>().terminalString[2] = "";
                this.GetComponent<EnemyTerminal>().checkTerminalString();
                this.GetComponent<EnemyTerminal>().evaluateActions();
                this.GetComponent<EnemyTerminal>().numberOfLines = 4;
                this.GetComponent<EnemyTerminal>().terminalString[0] = "double dec = .5;";
                this.GetComponent<EnemyTerminal>().terminalString[1] = "double rot = var + dec;";
                this.GetComponent<EnemyTerminal>().terminalString[2] = "System.rotate(dec);";
                this.GetComponent<EnemyTerminal>().terminalString[3] = "System.body(Color.RED);";
                break;
            case 2:
                this.GetComponent<EnemyTerminal>().numberOfLines = 1;
                this.GetComponent<EnemyTerminal>().numOfLegacy[0] = true;
                attackSpeed = 0.1f;
                rotateSpeed = 5;
                StartCoroutine(BossSmash(5, 0, ParserAlgo.keyActions.ERROR, 4));
                StartCoroutine(MegaAttack(.5f, 4, 5));
                this.GetComponent<EnemyTerminal>().actions.Clear();
                this.GetComponent<EnemyTerminal>().actions.Add(ParserAlgo.keyActions.TURNBLACK);
                this.GetComponent<EnemyTerminal>().evaluateActions();
                yield return new WaitForSeconds(22.5f);
                rotateSpeed = 10;
                attackSpeed = 0.035f;
                StartCoroutine(MegaAttack(1, .5f, 10));
                yield return new WaitForSeconds(15);
                intervalToSpike = .5f;
                this.GetComponent<Rotater>().rotationSpeed = 100;
                this.GetComponent<EnemyTerminal>().numberOfLines = 1;
                this.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(Color.BLUE);";
                this.GetComponent<EnemyTerminal>().checkTerminalString();
                this.GetComponent<EnemyTerminal>().evaluateActions();
                break;
            case 1:
                this.GetComponent<EnemyTerminal>().numberOfLines = 1;
                this.GetComponent<EnemyTerminal>().numOfLegacy[0] = true;
                this.GetComponent<EnemyTerminal>().actions.Clear();
                this.GetComponent<EnemyTerminal>().actions.Add(ParserAlgo.keyActions.TURNBLACK);
                this.GetComponent<EnemyTerminal>().evaluateActions();
                attackSpeed = 0.08f;
                rotateSpeed = 12;
                this.GetComponent<Rotater>().rotationSpeed = 500;
                this.GetComponent<EnemyTerminal>().moveSpeed = .04f;
                StartCoroutine(BossSmash(1, 2, ParserAlgo.keyActions.MOVERIGHT, 1));
                StartCoroutine(MegaAttack(1, 1, 2));
                yield return new WaitForSeconds(5f);
                StartCoroutine(MegaAttack(2, 1, 2));
                StartCoroutine(BossSmash(1, 2, ParserAlgo.keyActions.MOVELEFT, 1));
                yield return new WaitForSeconds(5f);
                StartCoroutine(MegaAttack(1, 2, 2));
                StartCoroutine(BossSmash(1, 2, ParserAlgo.keyActions.MOVERIGHT, 1));
                yield return new WaitForSeconds(5f);
                StartCoroutine(MegaAttack(1, 3, 2));
                StartCoroutine(BossSmash(1, 2, ParserAlgo.keyActions.MOVELEFT, 1));
                yield return new WaitForSeconds(10f);
                this.GetComponent<Rotater>().rotationSpeed = 100;
                this.GetComponent<EnemyTerminal>().numberOfLines = 1;
                this.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(Color.GREEN);";
                this.GetComponent<EnemyTerminal>().checkTerminalString();
                this.GetComponent<EnemyTerminal>().evaluateActions();
                Camera.main.GetComponent<CameraController>().enabled = true;
                Camera.main.orthographicSize = 7.5f;
                attackSpeed = 0.04f;
                this.GetComponent<EnemyTerminal>().moveSpeed = .04f;
                StartCoroutine(BossSmash(1, 2, ParserAlgo.keyActions.MOVERIGHT, 1));
                yield return new WaitForSeconds(5f);
                StartCoroutine(BossSmash(1, 2, ParserAlgo.keyActions.MOVELEFT, 1));
                yield return new WaitForSeconds(5f);
                StartCoroutine(BossSmash(1, 2, ParserAlgo.keyActions.MOVERIGHT, 1));
                yield return new WaitForSeconds(5f);
                StartCoroutine(BossSmash(1, 2, ParserAlgo.keyActions.MOVELEFT, 1));
                yield return new WaitForSeconds(6);
                this.GetComponent<EnemyHealthManager>().enemyHealth = 6;
                yield return new WaitForSeconds(.5f);
                this.GetComponent<EnemyHealthManager>().enemyHealth = 1;
                break;
        }

        // reset
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        attackSpeed = 0.025f;
        rotateSpeed = 1;
        Camera.main.GetComponent<CameraController>().enabled = true;
        Camera.main.orthographicSize = 6.5f;

    }

    public IEnumerator MegaAttack(float on, float off, int numTimes)
    {
        for (int i = 0; i < numTimes; i++)
        {
            intervalToSpike = 0;
            yield return new WaitForSeconds(on);
            intervalToSpike = .5f;
            yield return new WaitForSeconds(off);
        }
    }

    public IEnumerator BossSmash(float intervalToSmash, int intervalToMove, ParserAlgo.keyActions action, int numtimes)
    {
        for (int i = 0; i < numtimes; i++)
        {
            if (action != ParserAlgo.keyActions.ERROR)
            {
                this.GetComponent<EnemyTerminal>().actions.Add(action);
                this.GetComponent<EnemyTerminal>().evaluateActions();
                yield return new WaitForSeconds(1);
                this.GetComponent<EnemyTerminal>().actions.Remove(action);
            }
            this.GetComponent<Smash>().countSmash++;
            yield return new WaitForSeconds(intervalToSmash);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<KillPlayer>() && other.gameObject.tag != "Boss2Attack")
        {
            if (other.transform.parent.parent != null && health == 4 && !damaged)
            {
                damaged = true;
                this.GetComponent<EnemyHealthManager>().giveDamage(1);
                Destroy(other.transform.parent.parent.gameObject);
            }
            else if (health == 5 && damaged)
            {
                damaged = false;
                this.GetComponent<EnemyHealthManager>().giveDamage(1);
                Destroy(other.transform.parent.gameObject);
            }
        }
    }

    public void OnDestroy()
    {
        levelExit.SetActive(true);
    }
}
