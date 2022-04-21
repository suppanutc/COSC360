 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Team Gecko COSC360
 * 
 * Public class that handles all the players
 * pylon/charge interactions and now also dialogue
 */
public class PlayerPylon : MonoBehaviour
{
    // Checking if the player is grounded or not
    // Boolean value stating if the player is carrying a charge or not
    // set to off by default
    bool hasCharge;
    // Reference to animator component
    Animator anim;
    // Stores a reference to a pylon we are in range of,
    // Used to toggle this objects hasCharge
    Pylon pylon;

    // A reference to the charge eefect prefab
    ParticleSystem chargeEffect;
    // The button player presses to swap a charge
    KeyCode chargeKey;
    [SerializeField]
    AudioSource activate;
    [SerializeField]
    AudioSource deativate;
    
    Controller2D controller;
    DialogueManager dialogueBox;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        // initialise particle effect
        chargeEffect = this.gameObject.GetComponentInChildren<ParticleSystem>();
        chargeEffect.Stop();
        // Initialise the reference to the Animator component
        anim = GetComponent<Animator>();
        // set charge to whatever the gamemaster dictates and colour player
        hasCharge = GameMaster.playerCharge;
        dialogueBox = GetComponentInChildren<DialogueManager>();
        dialogueBox.gameObject.SetActive(false);

        //SetKeyBindings();
        SetChargeControl();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player presses E key in range of a pylon
        if (Input.GetKeyDown(chargeKey) && pylon != null && !GameMaster.isDead)
        {
            // if player swapped a charge
            if (pylon.ToggleCharge(hasCharge))
            {
                
                // Toggle charge, colour player, accordingly
                toggleCharge();

            }
        }

    }
    // Triiger collider handler
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Pylon":
                pylon = collision.GetComponent<Pylon>();
                break;
            case "CheckPoint":
                GameMaster.checkPointHintCounter = GameMaster.hintCounter;
                
                
                hasCharge = false;
                controller.hasCharge = hasCharge;
                chargeEffect.Stop();
                
                
                collision.gameObject.GetComponent<CheckpointPylon>().disableCheckpoint();
                GameMaster.currentCheckpoint = collision.transform.position;
                GameMaster.playerCharge = hasCharge;
                break;
            case "Collectable":
                collision.gameObject.GetComponent<Collectable>().Collect();
                break;
            case "DialogueTrigger":
                collision.gameObject.GetComponent<DialogueTrigger>().DisableTrigger();

                if (!GameMaster.deactivateHUD)
                {
                    dialogueBox.gameObject.SetActive(true);
                    dialogueBox.BeginDialogue();
                }
                GameMaster.hintCounter++;
                break;
            case "SpeedUp":
                GameMaster.scientistSpeedUp = true;
                collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                break;
            case "Vent":
                GameMaster.LevelCleared = true;
                break;
            case "endPipe":
                GameMaster.LoadNextLevel();
                break;
            default:

                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If we are exiting range of pylon, player can no longer press E
        if (collision.tag == "Pylon")
        {
            pylon = null;
        }
    }

    //Method for toggling the players charge on and off
    private void toggleCharge()
    {

        // if i had a charge, i must've given a charge so remove charge
        if (hasCharge)
        {
            deativate.Play();
            hasCharge = false;
            chargeEffect.Stop();
            //string s = anim.GetCurrentAnimatorStateInfo(0).ToString();
            //anim.Play((s.Substring(0,s.Length-7)));
        }
        // if i had no charge, i must've taken a charge so add a charge
        else
        {
            activate.Play();
            hasCharge = true;
            chargeEffect.Play();
            //anim.Play(anim.GetCurrentAnimatorStateInfo(0).ToString() + "Charged");
        }
        controller.hasCharge = hasCharge;

    }



    // A method to set the key used for charge
    public void SetChargeControl()
    {
        chargeKey = GameMaster.chargeKey;
    }
}


