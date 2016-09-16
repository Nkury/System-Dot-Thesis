using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    private float moveVelocity;

    public float jumpHeight;
    public float sprintSpeed;
    public float walkSpeed;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool grounded;

    private bool doubleJumped;
    public AudioSource jumpSound;
    public AudioSource doubleJumpSound;

    private Animator anim;

    public Transform firePoint;
    public GameObject ninjaStar;

    public float shotDelay;
    private float shotDelayCounter;

    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

    private float gravityStore;
    public AudioSource collectBit;


    private SimonController simon;
    private ActionsPerfromedManager actionManager;

    public PauseMenu pauseMenu;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        gravityStore = GetComponent<Rigidbody2D>().gravityScale;
        simon = FindObjectOfType<SimonController>();
        actionManager = FindObjectOfType<ActionsPerfromedManager>();
    }

    void FixedUpdate()
    {
        this.grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!IntelliSense.talking && !IntelliSense.wait && EnemyTerminal.globalTerminalMode < 2) 
        //{
        if(EnemyTerminal.globalTerminalMode < 2) {
            if (grounded)
            {
                doubleJumped = false;
            }
            if (Input.GetButtonDown("Jump") && this.grounded)
            {
                Jump();
                jumpSound.Play();
                if (simon != null && simon.getTask() == SimonController.JUMP)
                {
                    actionManager.addAction();
                }
            }

            if (Input.GetButtonDown("Jump") && !doubleJumped && !grounded)
            {
                Jump();
                jumpSound.Stop();
                doubleJumpSound.Play();
                doubleJumped = true;
                if (simon != null && simon.getTask() == SimonController.JUMP)
                {
                    actionManager.addAction();
                }
            }

            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }

            moveVelocity = moveSpeed * Input.GetAxisRaw("Horizontal");

            if (knockbackCount <= 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);
            }
            else {
                if (knockFromRight)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-knockback, knockback);
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(knockback, knockback);
                }

                knockbackCount -= Time.deltaTime;
            }



            anim.SetBool("Grounded", grounded);
            anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));

            if (GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }


            //if (Input.GetButtonDown("Fire1") && EnemyTerminal.terminalCount < 2)
            //{
            //    Instantiate(ninjaStar, firePoint.position, firePoint.rotation);
            //    shotDelayCounter = shotDelay;
            //}

            //if (Input.GetButton("Fire1") && EnemyTerminal.terminalCount < 2)
            //{
            //    shotDelayCounter -= Time.deltaTime;

            //    if (shotDelayCounter <= 0)
            //    {
            //        shotDelayCounter = shotDelay;
            //        Instantiate(ninjaStar, firePoint.position, firePoint.rotation);
            //    }
            //}
            //if (anim.GetBool("Sword"))
            //{
            //    anim.SetBool("Sword", false);
            //}

            //if (Input.GetButtonDown("Fire2"))
            //{
            //    anim.SetBool("Sword", true);
            //}
        }
        else
        {
            if (GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
   }

    void Jump()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
    }

    // MAY CAUSE PROBLEMS IN THE FUTURE
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "movingPlatform")
        {
            transform.root.gameObject.transform.parent = coll.transform;
        } else if(coll.gameObject.tag == "bit")
        {
            collectBit.Play();
            PlayerStats.bitsCollected++;
            Destroy(coll.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "movingPlatform")
        {
            transform.parent.gameObject.transform.parent = null;
        }
    }
}
