using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    private float moveVelocity;

    public float jumpHeight;
    public float sprintSpeed;
    public float walkSpeed;
    public float maxVerticalVelocity;

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

    private bool pauseMovement = false;

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
        if(!pauseMovement && EnemyTerminal.globalTerminalMode < 2) {
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
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.y, -maxVerticalVelocity, Mathf.Infinity));
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

            // responsible for flipping the character left and right
            if (GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }

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

    
    void OnCollisionEnter2D(Collision2D coll)
    {
        // if we collide with a moving platform, our transform position
        // will be attached to that platform (so we can move when it moves)
        if(coll.gameObject.tag == "movingPlatform")
        {
            transform.root.gameObject.transform.parent = coll.transform;
        }
        // if we collide with a bit, we collect it. If it's an environment bit,
        // then we add it to the list of items that need to be destroyed when we
        // load the level
        else if(coll.gameObject.tag == "bit")
        {
            // avoides knockback from getting bits
            Physics2D.IgnoreCollision(this.transform.parent.GetComponent<Collider2D>(), coll.gameObject.GetComponent<Collider2D>(), false);
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), coll.gameObject.GetComponent<Collider2D>(), false);
            collectBit.Play();
            PlayerStats.bitsCollected++;
            if (coll.gameObject.name.Contains("enviroBit"))
            {
                PlayerStats.deadObjects.Add(coll.gameObject.name);
            }
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

    public void IntelliSenseTalking(bool set)
    {
        pauseMovement = set;
    }
}
