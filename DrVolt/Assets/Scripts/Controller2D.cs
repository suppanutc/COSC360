using UnityEngine;
using UnityEngine.UI;
/**
 * Team Gecko COSC360 2020
 * 
 * Public class used for player movement and platforming interactions
 * it also determines the current animation
 * 
 */
public class Controller2D : MonoBehaviour
{
    [Header("Player Movement")]
    // Speed of the player
    public float playerSpeed = 5f;
    // Jump Height
    public float jumpHeight = 10f;

    //Gravity constant
    public float gravityConstant = 3f;
    // Gravity fall multiplier
    public float gravityFall = 3f;

    // Reference to animator component
    Animator anim;
    bool isJumping;
    bool isFalling;
    public bool endLevel;
    // The related action/movement keys
    KeyCode jump, left, right;

    // Check to see if player is on the ground
    [Header("Collsion Masks")]
    
    float raycastGround = 0.05f;
    // works as detection for ground collision with player
    public LayerMask ground;
    public LayerMask platform;
    [SerializeField]
    AudioSource walking;
    [SerializeField]
    AudioSource jumpSound;
    public bool hasCharge;
    bool deadCheck;


    // This field is needed for the gizmos to work on before start
    // Please drag and drop the BoxCollider2d component of this object
    // in the inspector pane > Controller2d > bc
    public BoxCollider2D bc;

    // Pointer to the players Rigid Body component
    Rigidbody2D rb;

    //Pointer to this objects SpriteRenderer component
    SpriteRenderer sprite;

    //[Header("Textbox Display")]
    PlayerPanel playerCanvas;
    // Reference to this game objects text box
    //Text playerText;
   
    // Start is called before the first frame update
    private void Start()
    {
        // Initialise the reference to the Animator component
        anim = GetComponent<Animator>();
        //Instantiate player textbox
        //playerText = this.gameObject.GetComponentInChildren<Text>();
        //Disable text if active
        // NOTE: Leaving this here cause maybe we can use this for player speech later on????
        //playerText.gameObject.SetActive(false);
        //canvas instaed?
        //playerCanvas = this.gameObject.GetComponentInChildren<PlayerPanel>();
        //playerCanvas.gameObject.SetActive(false);

        // get a ref to boxcollider
        // man I need to change its name huh
        bc = gameObject.GetComponent<BoxCollider2D>();
        // Get a reference to our initial bc offsets


        // Get a reference to this objects Rigid body
        rb = GetComponent<Rigidbody2D>();
        // Get a reference to this objects sprite renderer
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        rb.gravityScale = gravityConstant;
        SetControls();
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && !GameMaster.isDead && !GameMaster.LevelCleared && !endLevel )
        {
            // Get input from arrow keys, scales the longer they are pressed, thanks Unity ya GC
            float horizontalMovement = Input.GetAxis("Horizontal");
            // if both keys are pressed, nullify movement on ground


            if (Input.GetKey(left) && Input.GetKey(right))
            {

                //anim.SetBool("isRunning", false);
                horizontalMovement = 0f;
                if (!hasCharge)
                {
                    anim.Play("GeckoStand");
                }
                else
                {
                    anim.Play("GeckoStandCharged");
                }
            }

            else if (Mathf.Abs(horizontalMovement) < 0.3f && (Input.GetKey(right) || Input.GetKey(left)) && (!isJumping && !isFalling))
            {
                if (!hasCharge)
                {
                    anim.Play("GeckoStartRun");
                }
                else
                {
                    anim.Play("GeckoStartRunCharged");
                }

            }
            else if ((Input.GetKey(right) || Input.GetKey(left)) && (!isJumping && !isFalling))
            {
                if (!hasCharge)
                {
                    anim.Play("GeckoRun");
                }
                else
                {
                    anim.Play("GeckoRunCharged");
                }

            }
            else if ((!isJumping && !isFalling) && Mathf.Abs(horizontalMovement) != 0)
            {

                if (!hasCharge)
                {
                    anim.Play("GeckoStopRun");
                }
                else
                {
                    anim.Play("GeckoStopRunCharged");
                }
            }
            else if ((!isJumping && !isFalling))
            {

                if (!hasCharge)
                {
                    anim.Play("GeckoStand");
                }
                else
                {
                    anim.Play("GeckoStandCharged");
                }
            }



            // If player presses space, jump according to jump height
            if (Input.GetKeyDown(jump) && IsGrounded() && (IsMovingPlatform() || (rb.velocity.y < 0.005 && rb.velocity.y > -0.005)))//&& (rb.velocity.y <= 0.009f && rb.velocity.y >= -0.009f))
            {
                isJumping = true;
                //rb.AddForce(Vector3.up * jumpHeight, ForceMode2D.Impulse);
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);

                jumpSound.Play();
                if (!hasCharge)
                {
                    anim.Play("GeckoJump");
                }
                else
                {
                    anim.Play("GeckoJumpCharged");
                }
            }
            // Add right movement only if we are not on a right wall

            if (horizontalMovement > 0 && !IsOnRightWall())
            {

                rb.velocity = new Vector2(horizontalMovement * playerSpeed, rb.velocity.y);
                // Flip sprite and box collider to match new direction

                sprite.flipX = false;
                if (!walking.isPlaying)
                {
                    walking.PlayOneShot(walking.clip);
                }

            }
            // Add left movement only if we are not on a left wall
            else if (horizontalMovement < 0 && !IsOnLeftWall())
            {
                rb.velocity = new Vector2(horizontalMovement * playerSpeed, rb.velocity.y);

                sprite.flipX = true;
                if (!walking.isPlaying)
                {
                    walking.PlayOneShot(walking.clip);
                }
            }

            // Running animation stuff
            if (Input.GetKey(right) || Input.GetKey(left))
            {
                // anim.SetBool("isRunning", true);
            }
            else //if (Mathf.Abs(horizontalMovement) < 0.05f)
            {
                //anim.SetBool("isRunning", false);
                walking.Stop();
            }
            // anim.SetFloat("xVelocity", Mathf.Abs(horizontalMovement));

            // Jumping anim stuff
            //anim.SetBool("isJumping", isJumping);
            // If player is mid jump, change animation
            if ((rb.velocity.y < -0.1f && isJumping) || (rb.velocity.y < -0.05f && !IsPlatform()))//&& rb.velocity.y < 0 )
            {
                isJumping = false;
                isFalling = true;
                if (!hasCharge)
                {
                    anim.Play("GeckoFall");
                }
                else
                {
                    anim.Play("GeckoFallCharged");
                }
                //anim.SetBool("isFalling", true);
            }

            if ((IsGrounded() && (rb.velocity.y < 0.005 && rb.velocity.y > -0.005)) || (IsMovingPlatform() && rb.velocity.y != 0) && !isJumping)
            {
                isFalling = false;
                isJumping = false;
                // anim.SetBool("isFalling", false);
                //anim.SetBool("isJumping", false);

            }
        } else if (GameMaster.isDead ^ deadCheck)
        {
            deadCheck = true;
            if (!hasCharge)
            {
                anim.Play("Defeat");
            }
            else
            {
                anim.Play("DefeatCharged");
            }
        }
        else if (GameMaster.LevelCleared && !endLevel)
        {
            if (!hasCharge)
            {
                anim.Play("Victory");
            }
            else
            {
                anim.Play("VictoryCharged");
            }
        } else if (endLevel && GameMaster.currentLevel !=3)
        {
            float horizontalMovement = 0.7f;
            rb.velocity = new Vector2(horizontalMovement * playerSpeed, rb.velocity.y);
            if (!hasCharge)
            {
                anim.Play("GeckoRun");
            }
            else
            {
                anim.Play("GeckoRunCharged");
            }
        }
    }


    // Method to check if player is on the ground
     bool IsGrounded()
    {
        // Cast a boxcast from center of player, to just 
        // beyond the height of the boxcollider2d
        RaycastHit2D raycastCheckGround = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, (bc.bounds.extents.y / 2) + raycastGround, ground);
        RaycastHit2D raycastCheckPlatform = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, (bc.bounds.extents.y / 2) + raycastGround, platform)
           ;
        return raycastCheckGround.collider != null ||  raycastCheckPlatform.collider != null;
    }

    // Check to see if we are on or in a platform
    bool IsPlatform()
    {
        RaycastHit2D raycastCheckPlatform = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, (bc.bounds.extents.y / 2) + raycastGround, platform);

        return raycastCheckPlatform.collider != null; 
    }

    // check to see if we are on a moving pltform, really nigly edge case here,
    // bit of a hack solve, will review in polish / Beta week 
    bool IsMovingPlatform()
    {
        RaycastHit2D raycastCheckPlatform = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, (bc.bounds.extents.y / 2) + raycastGround, platform);

        if (raycastCheckPlatform.collider != null)
        {
            return raycastCheckPlatform.collider.tag == "MovingPlatform";
        }
        return false;
    }
    // Method that checks if player is on left wall
    bool IsOnLeftWall()
    {

        RaycastHit2D raycastCheck = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.left, (bc.bounds.extents.x / 2) + raycastGround, ground);
        return raycastCheck.collider != null;
    }

    // Method that checks if player is on right wall
    private bool IsOnRightWall()
    {

        RaycastHit2D raycastCheck = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.right, (bc.bounds.extents.x / 2) + raycastGround, ground);
        return raycastCheck.collider != null;
    }



    // A method that sets the controls based on player preferences
    public void SetControls()
    {
        jump = GameMaster.jump;
        left = GameMaster.left;
        right = GameMaster.right;
        this.gameObject.GetComponent<PlayerPylon>().SetChargeControl();
    }
}

