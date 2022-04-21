using UnityEngine;
using UnityEngine.UI;
/**
 * Team Gecko COSC360 2020
 * 
 * Public class used for player movement and platforming interactions
 * 
 */
public class Controller2D1 : MonoBehaviour
{
    [Header("Player Movement")]
    // Speed of the player
    public float playerSpeed = 5f;
    // Jump Height
    public float jumpHeight = 5f;
    // Used to stop player moving if companion dies (TEMPORARY)
    float movement = 1f;
    //Gravity constant
    public float gravityConstant = 1.5f;
    // Gravity fall multiplier
    public float gravityFall = 3f;
    bool isMoving;
    // Reference to animator component
    Animator anim;
    bool isJumping;
    bool inAir;
    float inAirTimer;
    // The related action/movement keys
    KeyCode jump, left, right;

    // Check to see if player is on the ground
    [Header("Collsion Masks")]
    
    float raycastGround = 0.05f;
    // works as detection for ground collision with player
    public LayerMask ground;
    public LayerMask platform;

    // This field is needed for the gizmos to work on before start
    // Please drag and drop the BoxCollider2d component of this object
    // in the inspector pane > Controller2d > bc
    public BoxCollider2D bc;

    // Pointer to the players Rigid Body component
    Rigidbody2D rb;

    //Pointer to this objects SpriteRenderer component
    SpriteRenderer sprite;

    //[Header("Textbox Display")]
    Canvas playerCanvas;
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
        playerCanvas = this.gameObject.GetComponentInChildren<Canvas>();
        playerCanvas.gameObject.SetActive(false);
        //allow player to move
        movement = 1f;

        // Get a reference to this objects Rigid body
        rb = GetComponent<Rigidbody2D>();
        // Get a reference to this objects sprite renderer
        sprite = this.gameObject.GetComponent<SpriteRenderer>();

        SetControls();
    }


    // Update is called once per frame
    void Update()
    {
       if (inAirTimer > 0)
        {
            inAirTimer -= Time.deltaTime;
        }
 
        else 
        {
            inAir = false;
        }
        if (inAirTimer < 0.4f)
        {
            isJumping = false;
        }
        // Get input from arrow keys, scales the longer they are pressed, thanks Unity ya GC
        float horizontalMovement = Input.GetAxis("Horizontal");
        // if both keys are pressed, nullify movement on ground
        if (Input.GetKey(left) && Input.GetKey(right))
        {
            
            //anim.SetBool("isRunning", false);
            horizontalMovement = 0f;
        }
        // Running animation stuff
        if (Input.GetKey(right) || Input.GetKey(left))
        {
            isMoving = true;
        }
        else //if (Mathf.Abs(horizontalMovement) < 0.05f)
        {
            isMoving = false;
        }
        // If player presses space, jump according to jump height
        if (Input.GetKeyDown(jump) && (IsGrounded() || IsPlatform()) && !isJumping)//&& (rb.velocity.y <= 0.009f && rb.velocity.y >= -0.009f))
        {
            anim.SetTrigger("isJumping");
            inAirTimer = 0.6f;
            isJumping = true;
            inAir = true;
            //rb.AddForce(Vector3.up * jumpHeight, ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight * movement);
        } 
        // Add right movement only if we are not on a right wall
        
        if (horizontalMovement > 0 && !IsOnRightWall())
        {
            float y = rb.velocity.y;

            rb.velocity = new Vector2(horizontalMovement * playerSpeed * movement, y);
            // Flip sprite and box collider to match new direction
            //bc.offset = new Vector2(bcOffsetRight, bc.offset.y);
            sprite.flipX = false;
        }
        // Add left movement only if we are not on a left wall
        else if (horizontalMovement < 0 && !IsOnLeftWall())
        {


            float y = rb.velocity.y;
         
            rb.velocity = new Vector2(horizontalMovement * playerSpeed * movement, y);
            //bc.offset = new Vector2(bcOffsetLeft, bc.offset.y);
            sprite.flipX = true;
        } 

        //// Running animation stuff
        //if (Input.GetKey(right) || Input.GetKey(left))
        //{
        //    anim.SetBool("isRunning", true);
        //} else //if (Mathf.Abs(horizontalMovement) < 0.05f)
        //{
        //    anim.SetBool("isRunning", false);
        //}
        

        // If player is mid jump, change animation
        if (rb.velocity.y < 0.2f && rb.velocity.y >  0f && inAir)
        {
            anim.SetBool("isFalling", true);
        }


        if ((IsGrounded() || IsPlatform()) && inAirTimer < 0.2f)
        {

            anim.SetBool("isFalling", false);


        }
            anim.SetBool("isRunning", isMoving);
            anim.SetBool("isJumping", isJumping);



        // Change fall gravity to give the player a more satisfying jump parabola
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityConstant;
        }
        else 
        {
            rb.gravityScale = gravityFall;
        }
    }


    // Method to check if player is on the ground
     bool IsGrounded()
    {
        // Cast a boxcast from center of player, to just 
        // beyond the height of the boxcollider2d
        RaycastHit2D raycastCheckGround = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, (bc.bounds.extents.y / 2) + raycastGround, ground);;
        return raycastCheckGround.collider != null;
    }

    // Method to see if we are on a platform
    bool IsPlatform()
    {
        RaycastHit2D raycastCheckPlatform = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, (bc.bounds.extents.y / 2) + raycastGround, platform);
        return raycastCheckPlatform.collider != null;
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

    // If the player failed to keep the scientist alive,
    // Restrict the players movement and give a visual cue to restart (?)
    // Mayb
    public void companionDied()
    {
        // Disable movement
        movement = 0f;
        // Display a helper text to show player they should restart
        //playerText.gameObject.SetActive(true);
        playerCanvas.gameObject.SetActive(true);

    }

    // A method that displays a congratulatory message for the player upon clearing a level
    public void VictoryTextDisplay()
    {
        // Disable movement
        movement = 0f;
        // Change the player Canvas text
        playerCanvas.gameObject.SetActive(true);
        Text text = this.gameObject.GetComponentInChildren<Text>();
        text.text = "Fuck yeah nice one Shagga";
        
    }

    // A method that sets the controls based on player preferences
    public void SetControls()
    {
        jump = GameMaster.jump;
        left = GameMaster.left;
        right = GameMaster.right;
        this.gameObject.GetComponent<PlayerPylon>().SetChargeControl();
    }


    //void OnDrawGizmosSelected()
    //{

    //    Gizmos.color = Color.red;
    //    // Draw BoxCast for checking ground
    //    Gizmos.DrawRay(bc.bounds.center + new Vector3(bc.bounds.extents.x, 0f), Vector2.down * (bc.bounds.size.y / 2 + raycastGround));
    //    Gizmos.DrawRay(bc.bounds.center - new Vector3(bc.bounds.extents.x, 0f), Vector2.down * (bc.bounds.size.y  / 2 + raycastGround));
    //    Gizmos.DrawRay(bc.bounds.center + new Vector3(-bc.bounds.extents.x, - bc.bounds.extents.y - raycastGround), Vector2.right * bc.bounds.size.x);
    //    // Draw box cast for checking if player is against the left wall
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(bc.bounds.center + new Vector3(-bc.bounds.extents.x - raycastGround, bc.bounds.extents.y), Vector2.right * bc.bounds.size.x / 2);
    //    Gizmos.DrawRay(bc.bounds.center + new Vector3(-bc.bounds.extents.x - raycastGround, -bc.bounds.extents.y), Vector2.right * bc.bounds.size.x / 2);
    //    Gizmos.DrawRay(bc.bounds.center + new Vector3(-bc.bounds.extents.x - raycastGround, -bc.bounds.extents.y), Vector2.down * (-bc.bounds.size.y));
    //    // Draw box cast for checking if player is against the left wall
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(bc.bounds.center + new Vector3(bc.bounds.extents.x + raycastGround, bc.bounds.extents.y), Vector2.left * bc.bounds.size.x / 2);
    //    Gizmos.DrawRay(bc.bounds.center + new Vector3(bc.bounds.extents.x + raycastGround, -bc.bounds.extents.y), Vector2.left * bc.bounds.size.x / 2);
    //    Gizmos.DrawRay(bc.bounds.center + new Vector3(bc.bounds.extents.x + raycastGround, -bc.bounds.extents.y), Vector2.down * (-bc.bounds.size.y));

    //}
}

