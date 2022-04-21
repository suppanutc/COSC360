using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Team Gecko 
 * COSC360 2020
 * 
 * A class that controls Scientist movement and all scientist related interactions
 * 
 */

public class GravityScientist : MonoBehaviour
{
    public float scientistSpeed;
    [SerializeField]
    FlyBob fly;
 
    bool flipped = false;
    bool inField = false;
    GameObject gravityfield;
    public static bool leftWall;
    public bool isWall;
    // Acts as a movement buffer so player can prepare to continue after respawn
    public float respawnTimer;
    public bool playerMoved = false;
    bool audioNotPlayed;
    float time;
    // Reference to animator component
    Animator anim;
    [SerializeField]
    AudioClip[] trapSounds;
    [SerializeField]
    int speedMultiplier = 2;
    float ogSpeed;
    bool changeInSpeed;
    void Start()
    {
        GameMaster.isDead = false;

        time = respawnTimer;
        anim = GetComponent<Animator>();
        audioNotPlayed = true;
        changeInSpeed = GameMaster.scientistSpeedUp;
        ogSpeed = scientistSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (changeInSpeed ^ GameMaster.scientistSpeedUp)
        {
            changeInSpeed = GameMaster.scientistSpeedUp;
            if (changeInSpeed)
            {
                scientistSpeed *= speedMultiplier;
            }
            else
            {
                scientistSpeed = ogSpeed;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            GameMaster.scientistSpeedUp = true;
        }
        else { GameMaster.scientistSpeedUp = false; }

        //if timer is still active, reduce timer...
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        // Otherwise Scientist can move
        else if (playerMoved)
        {
            anim.Play("ScientistWalk");
            transform.position += Vector3.right * Time.deltaTime * scientistSpeed;


            if (inField == false)
            {
                gravityfield = null;
                //trapAudioSource.Stop();
            }

            if (gravityfield != null)
            {
                if (gravityfield.GetComponent<Trap>().IsOn() && flipped == false)
                {
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = -1;
                    var rotationVector = transform.rotation.eulerAngles;
                    rotationVector.x = 180;
                    transform.rotation = Quaternion.Euler(rotationVector);
                    flipped = true;



                }
                else if (gravityfield.GetComponent<Trap>().IsOn() == false && flipped == true)
                {
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    var rotationVector = transform.rotation.eulerAngles;
                    rotationVector.x = 180;
                    transform.rotation = Quaternion.Euler(rotationVector);
                    flipped = false;
                }
            }
        }
    }


    // If he hits a trap, kill him
    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject trigger = other.gameObject;
        gravityfield = trigger;
        string filtered = System.Text.RegularExpressions.Regex.Replace(other.name, "[^A-Za-z]", "");

        if (trigger.tag == "Trap" || trigger.tag == "Laser" || trigger.tag == "Fence")
        {
            if (audioNotPlayed)
            {
                AudioSource.PlayClipAtPoint(trapSound(filtered + "Hit"), Vector3.zero);
                audioNotPlayed = false;
            }
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            KillScientist(trigger.tag);
            
        }


        if (trigger.tag == "GravityField")
        {
            // If scientist enters an active gravity field, he will be flipped and flags
            // will be set to show that he is both flipped and in an active field.
            if (flipped == false && trigger.GetComponent<Trap>().IsOn() && inField == false)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = -1;
                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.x = 180;
                transform.rotation = Quaternion.Euler(rotationVector);
                flipped = true;
                inField = true;
            }

            // If the scientist exits an active gravity field while not flipped, flag will be set to show that
            // he has left the field and the reference to that field is set back to null.
            else if (flipped == false && trigger.GetComponent<Trap>().IsOn() && inField == true)
            {
                inField = false;
                gravityfield = null;
                
            }

            // If the scientist exits an active gravity field while flipped, he will be unflipped
            // and a flags/references are set to show that he is no longer in that field
            else if (flipped == true && trigger.GetComponent<Trap>().IsOn() && inField == true)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.x = 180;
                transform.rotation = Quaternion.Euler(rotationVector);
                flipped = false;
                inField = false;
                gravityfield = null;
                
            }

            else if (trigger.GetComponent<Trap>().IsOn() == false && inField == false)
            {
                inField = true;
            }
            else if (trigger.GetComponent<Trap>().IsOn() == false && inField == true)
            {
                inField = false;


            }

        }
        else if (other.gameObject.tag == "Win")
        {
            // Load completed level related UI elements
            GameMaster.LevelCleared = true;
        }
        else if (other.gameObject.tag == "endSwap"){
            GameMaster.finalMindSwap = true;
        }

    }

    // If he enters an active gravity field, reverse gravity
    void OnTriggerStay2D(Collider2D other)
    {
        GameObject trigger = other.gameObject;
        if (trigger.tag == "Trap" || trigger.tag == "Laser" || trigger.tag == "Fence")
        {
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            
            KillScientist(trigger.tag);
            
        }
        else if (other.tag == "DoorButton")
        {
            
            other.gameObject.GetComponent<DoorControl>().DisableDoor();
            GameMaster.scientistSpeedUp = false;
        }

    }

    // Prevents the scientist from getting stuck on colliders of inactive traps.
    // And kills him if he hits an enemy
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Trap")
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());

        }

        else if (other.gameObject.tag == "Enemy")
        {
            KillScientist("Floor");
            anim.Play("ScientistElectrocuted");
            SmartEnemy enemy = other.gameObject.GetComponent<SmartEnemy>();
            enemy.move = false;
            
        }



    }

    // A method for killing the scientist, will eventually be used to play 
    // the correct death animation via a switch case
    private void KillScientist(string trap)
    {
        
        GameMaster.isDead = true;
        //audioNotPlayed = true;
        // This will change to a deatrh animation that loops? A thing for Beta when it is ready
        playerMoved = false;

        switch (trap)
        {
            case "Laser":
                anim.Play("ScientistDeathLaser");
                break;
            default:
                anim.Play("ScientistElectrocuted");
                break;
        }
    }

    private AudioClip trapSound(string trap)
    {
        foreach(AudioClip temp in trapSounds)
        {
            

            if (trap == temp.name.ToString())
            {
                return temp;
            }
            
        }
        
        return null;
    }
}
