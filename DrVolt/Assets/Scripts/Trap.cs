/**
 * Team Gecko Cosc360  2020
 * 
 * A parent class for all of our trap objects
 *  All Trap objects should inherit these properties
 *  NOTE: Can also be used for moving platforms.
 */
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class Trap : MonoBehaviour
{
    //  Is this trap active? Off by default.
     public bool isOn = false;
    // A reference to this traps player visual aid child
    TrapColourChild visualAid;
    // A reference to this objects spriterenderer
    protected SpriteRenderer sprite;
    // Reference to this traps box collider
    protected BoxCollider2D boxCollider;
    bool colourNotSet = true;
    public Color currentColor;
    //bool oldOn;

    private void Awake()
    {
        // Get a reference to this traps visual aid
        visualAid = gameObject.GetComponentInChildren<TrapColourChild>();
        visualAid.colour = currentColor;

    }

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
        if (gameObject.name == "GravityField")
        {
            GetComponent<AudioSource>().Play();

        }
        if (!isOn && gameObject.tag != "GravityField")
        {
            boxCollider.enabled = false;
        }
        //oldOn = isOn;


    }

    protected void Update()
    {
        // Give player some visual feedback for the on/off state.
        // If traps are enabled, they become triggers which allow the
        // companion to die when he touches them.
        if (colourNotSet)
        {
            colourNotSet = false;
            visualAid.GetComponent<SpriteRenderer>().color = currentColor;
        }
        
        if (isOn)
        {
            if (!GetComponent<AudioSource>().isPlaying && gameObject.tag == "GravityField")
            {
                GetComponent<AudioSource>().Play();
            }
            // temporary colour change to see if script works
            //sprite.color = currentColor;
            if (gameObject.tag != "MovingPlatform" && gameObject.tag != "GravityField")
            {
                boxCollider.isTrigger = true;
            }
        }
        //else if (gameObject.tag == "MovingPlatform")
        //{
        //    boxCollider.enabled = true;
        //    boxCollider.isTrigger = false;
        //}




        else if (!isOn && gameObject.tag == "GravityField")
        {
            GetComponent<AudioSource>().Stop();
        }

        // If traps are disabled, they stop being triggers which allows the
        // companion to safely pass them.
        //else
        //{
        //    // return to normal colour of sprite
        //    //sprite.color = normalColor;
        //    if (gameObject.tag != "MovingPlatform" && gameObject.tag != "GravityField")
        //    {
        //        boxCollider.isTrigger = false;
        //    }
        //}
    }

    // Turn this trap off
    public void Deactivate()
    {
        isOn = false;
        if (boxCollider != null && (gameObject.tag != "MovingPlatform" && gameObject.tag != "GravityField"))
        {
            boxCollider.enabled = false;
        }
    }

    //Turn this trap on
    public void Activate()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
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
        // Remove this when visual aids are instantiated
        currentColor = color;
        // replace with this....
        // Colour the inside of the trap to suit the connected pylon(s)
       
    }
}