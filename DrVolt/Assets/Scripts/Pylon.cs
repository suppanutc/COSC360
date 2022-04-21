using UnityEngine;

/*  
 * Team Gecko Cosc360 2020
 * 
 * A class for the interactable pylons in the player world.
 * Instantiate an array of traps and drag and drop the trap
 * objects into the array in the interactions pane.
 * note: This objects required circle collider MUST be a trigger for it to work
 *
 */

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Pylon : MonoBehaviour
{
    [Header("Connected traps and pylons")]
    //A reference to any connected pylons that share traps
    public Pylon[] pylons;
    // The amount of traps we want to connect to this pylon
    // public int numberTrapsConnected;
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
    Color currentColor;
    // A reference to this traps player visual aid child
    TrapColourChild visualAid;

    // Used by Level designer to distinguish trap/pylon connections in prototyping
    public enum Colour { purple, orange, lime, blue, red, yellow, noToggle };
    public Colour colour;


    private void Awake()
    {


        // Initialise this pylons colour based on users Colour selection
        switch (colour)
        {
            case Colour.orange:
                currentColor = new Color32(255,125,0,255);
                break;
            case Colour.lime:
                currentColor = new Color32(130, 255, 0, 255);
                break;
            case Colour.blue:
                currentColor = new Color32(0, 255, 255, 255);
                break;
            case Colour.red:
                currentColor = Color.red; // Red also looks good
                break;
            case Colour.yellow:
                currentColor = Color.yellow; // Red also looks good
                break;
            case Colour.noToggle:
                currentColor = Color.black; // Red also looks good
                break;

            default:
                currentColor = new Color32(150, 0, 255, 255);
                break;
        }
        // Get a reference to this traps visual aid and colour it
        visualAid = gameObject.GetComponentInChildren<TrapColourChild>();
        visualAid.colour = currentColor;
        // Initialising trap array
        // Check if traps are connected, colour them
        if (traps.Length == 0)
        {
            
            gameObject.SetActive(false);
        }
        else
        {
            //traps = new Trap[numberTrapsConnected];
            // color each trap the same as this pylon
            foreach (Trap trap in traps)
            {
                trap.currentColor = currentColor;
            }
            // set traps on or off
           
        }
    }

    void Start()
    {

        chargeEffect = this.gameObject.GetComponentInChildren<ParticleSystem>();
        ToggleTraps();


        //sprite.color = currentColor;
        // Get a reference to this traps visual aid
        visualAid = gameObject.GetComponentInChildren<TrapColourChild>();
        visualAid.colour = currentColor;


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


