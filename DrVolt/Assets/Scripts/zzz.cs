using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zzz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
/**
 * Team Gecko Cosc360  2020
 * 
 * A parent class for all of our trap objects
 *  All Trap objects should inherit these properties
 *  NOTE: Can also be used for moving platforms.


[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Trap : MonoBehaviour
{
    //  Is this trap active? Off by default.
    public bool isOn = false;

    // A reference to this objects spriterenderer
    protected SpriteRenderer sprite;
    // Reference to this traps box collider
    protected BoxCollider2D boxCollider;

    // Colour ranges for visually connecting traps during prototyping
    Color normalColor = new Color(1, 1, 1, 1);
    Color currentColor;
    // Start is called before the first frame update
    protected void Start()
    {
        // get a reference to this objects Sprite renderer
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        // Get a reference to this objects box collider
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        // Set box collider to always be a trigger
        boxCollider.isTrigger = true;
        // Prevents moving platforms from ever becoming triggers
        if (gameObject.tag == "MovingPlatform")
        {
            boxCollider.isTrigger = false;
        }

    }

    protected void Update()
    {
        // Give player some visual feedback for the on/off state.
        // If traps are enabled, they become triggers which allow the
        // companion to die when he touches them.
        if (isOn)
        {

            // temporary colour change to see if script works
            sprite.color = currentColor;
            if (gameObject.tag != "MovingPlatform" && gameObject.tag != "GravityField")
            {
                boxCollider.isTrigger = true;
            }
            //Debug.Log("ACTIVE");


        }

        // If traps are disabled, they stop being triggers which allows the
        // companion to safely pass them.
        else
        {
            // return to normal colour of sprite
            sprite.color = normalColor;
            if (gameObject.tag != "MovingPlatform" && gameObject.tag != "GravityField")
            {
                boxCollider.isTrigger = false;
            }
        }
    }

    // Turn this trap off
    public void Deactivate()
    {
        isOn = false;

    }

    //Turn this trap on
    public void Activate()
    {
        isOn = true;

    }

    // Accessor method for isOn boolean 
    public bool IsOn()
    {
        return isOn;
    }

    //Set color of trap
    public void SetColor(Color color)
    {
        currentColor = color;
    }
}
*/

//using UnityEngine;

/*  
 * Team Gecko Cosc360 2020
 * 
 * A class for the interactable pylons in the player world.
 * Instantiate an array of traps and drag and drop the trap
 * objects into the array in the interactions pane.
 * note: This objects required circle collider MUST be a trigger for it to work
 *
 

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Pylon : MonoBehaviour
{
    // A reference to this objects spriterenderer
    SpriteRenderer sprite;
    [Header("Connected traps and pylons")]
    //A reference to any connected pylons that share traps
    public Pylon[] pylons;
    // The amount of traps we want to connect to this pylon
    //public int numberTrapsConnected;
    // Drag and drop references to the traps connected
    // This script will will use a for-loop to cycle through and toggle.
    public Trap[] traps;
    // Is the pylon charged? Off by default
    public bool hasCharge;
    // A reference to the charge eefect prefab
    ParticleSystem chargeEffect;
    [Header("The colour of the traps/pylons")]
    // Reference to this objects circle collider
    CircleCollider2D chargeField;
    // Start is called before the first frame update
    // Colour ranges for visually connecting traps during prototyping
    Color normalColor = new Color(1, 1, 1, 1);
    Color currentColor;

    // Used by Level designer to distinguish trap/pylon connections in prototyping
    public enum Colour { red, blue, green, yellow };
    public Colour colour;
    void Start()
    {
        // Get a reference to this objects sprite renderer
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        // initialise particle effect
        chargeEffect = this.gameObject.GetComponentInChildren<ParticleSystem>();

        // Initialise this pylons colour based on users Colour selection
        switch (colour)
        {
            case Colour.blue:
                currentColor = Color.blue;
                break;
            case Colour.green:
                currentColor = Color.green;
                break;
            case Colour.yellow:
                currentColor = Color.yellow;
                break;
            default:
                currentColor = Color.red;
                break;
        }

        // Initialising trap array
        // Check if traps are connected
        if (traps.Length == 0)
        {
            Debug.Log("There are no traps connected, disabling this object.");
            gameObject.SetActive(false);
        }
        else
        {
            //traps = new Trap[numberTrapsConnected];
            // color each trap the same as this pylon
            foreach (Trap trap in traps)
            {
                trap.SetColor(currentColor);
            }
            // set traps on or off
            ToggleTraps();

        }

        // Get a reference to this objects box collider
        chargeField = this.gameObject.GetComponent<CircleCollider2D>();
        // Set box collider to always be a trigger
        chargeField.isTrigger = true;
        ColourPylon();
    }

    // Method use to  check if a charge swap is possible
    // If possible carries out swap and indicates player script
    // That it occurred by returning a boolean value
    public bool ToggleCharge(bool playerCharge)
    {

        // Use an XOR operator to first define if we can swap
        bool swap = hasCharge ^ playerCharge;
        if (swap)
        {
            // Ifplayer is giving a charge, take charge
            if (playerCharge)
            {
                hasCharge = true;
            }
            // Else, give player charge
            else
            {
                hasCharge = false;
            }
            // Toggle connected traps accordingly
            ToggleTraps();

        }
        if (pylons.Length > 0)
        {
            foreach (Pylon pylon in pylons)
            {
                pylon.hasCharge = hasCharge;
                pylon.ToggleTraps();
                pylon.ColourPylon();
            }
        }

        // Colour our pylon accordingly
        ColourPylon();
        // Returns a bool indicating if a swap occured
        return swap;
    }


    // Depending on if the pylon is charged, toggle the traps connected 
    // to this pylon accordingly
    public void ToggleTraps()
    {
        // If no charge, power down traps
        if (!hasCharge)
        {
            foreach (Trap trap in traps)
            {
                trap.Deactivate();
            }
        }
        // Else activate each trap connected to this
        else
        {
            foreach (Trap trap in traps)
            {
                trap.Activate();
            }

        }
    }
    // Give a visual que that we have a charge or not for prototyping purposes
    // Change this to animation and glow for final game
    private void ColourPylon()
    {
        sprite.color = currentColor;
        if (hasCharge)
        {
            // temporary colour change to see if script works
            chargeEffect.Play();

        }
        else
        {

            chargeEffect.Stop();
        }
    }

    //// Ensure the pylon updates its colour each frame
    //private void Update()
    //{
    //    ColourPylon();

    //}
}

using UnityEngine;
/**
 * A moving platform Trap variant that allows player
 * to "stick" to platform
 * add slash back here g

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : Trap
{

    [Header("Movement")]
    //The speed of the platform
    public float speed;
    float range = 0.05f;

    // How long we want it to move for
    public float time;
    float timer = 0;
    // Enable a drop down selection for user that 
    // determines which way this platform moves
    public enum Direction { up, down, left, right };
    public Direction direction;
    int directionY = 0;
    int directionX = 0;
    bool moving;
    // The x and y position of this platform
    float ypos;
    float xpos;

    // rigid body pointer
    Rigidbody2D rb;

    // Start is called before the first frame update
    new void Start()
    {
        // Set tag to moving platform so parent Start()
        // can disable isTrigger
        gameObject.tag = "MovingPlatform";
        // Call parent start method
        base.Start();
        // Get a reference to this objects rigid body
        rb = GetComponent<Rigidbody2D>();
        // Initialise this platforms original position
        ypos = transform.position.y;
        xpos = transform.position.x;
        // Initialise this platforms Direction modifier
        SetDirection();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //reduce time and swap directions if our timer is up
        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
        }
        if (timer <= 0 && IsOn())
        {

            directionX *= -1;
            directionY *= -1;
            timer = time;
            moving = false;
        }
        // Move the platform if charged...
        if (IsOn())
        {
            // if i have changed positions, add velocity 
            if (!moving)
            {
                moving = true;
                rb.velocity = new Vector2(speed * directionX, speed * directionY);
                //rb.velocity = Vector3.Lerp(new Vector2(speed * directionX, speed * directionY), Vector3.zero, 0.1f *Time.fixedDeltaTime);
            }// else { rb.velocity = rb.velocity; }
        }
        // otherwise return it to its OG position...
        // if My x coord is not in it's original place
        else if (!(xpos - range < transform.position.x && xpos + range > transform.position.x))
        {

            // and now moving away from it
            if (direction == Direction.right && directionX != -1)
            {
                timer = time - timer;
                directionX = -1;
            }
            else if (direction == Direction.left && directionX != 1)
            {
                timer = time - timer;
                directionX = 1;
            }
            rb.velocity = new Vector2(speed * directionX, speed * directionY);
        }
        // if My y coord is not in it's original place
        else if (!(ypos - range < transform.position.y && ypos + range > transform.position.y))
        {
            // and now moving away from it
            if (direction == Direction.down && directionY != 1)
            {
                timer = time - timer;
                directionY = 1;
            }
            else if (direction == Direction.up && directionY != -1)
            {
                timer = time - timer;
                directionY = -1;
            }
            rb.velocity = new Vector2(speed * directionX, speed * directionY);
        }
        // or stop moving.
        else
        {
            // Remove force
            rb.velocity = rb.velocity * 0.95f * Time.deltaTime;
            // Reset OG position, and direction
            transform.position = new Vector3(xpos, ypos, transform.position.z);
            timer = 0f;
            SetDirection();
        }
    }

    // Method that sets the originaly direction of the platform
    void SetDirection()
    {
        switch (direction)
        {
            case Direction.up:
                directionY = -1;
                break;
            case Direction.down:
                directionY = 1;
                break;
            case Direction.left:
                directionX = 1;
                break;
            default:
                directionX = -1;
                break;
        }
    }
}
*/


