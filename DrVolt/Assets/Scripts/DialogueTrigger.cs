using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Team Gecko 
 * COSC360 2021
 * 
 * A class that takes care of disabling a dialogue trigger if the 
 * player has already read the text it contains
 */
public class DialogueTrigger : MonoBehaviour
{

    SpriteRenderer mySprite;
    CircleCollider2D bc;
    bool lastHUDOption;
    private void Start()
    {
        lastHUDOption = GameMaster.deactivateHUD;
        bc = GetComponent<CircleCollider2D>();
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.enabled = false;
        if (lastHUDOption)
        {
           // DisableTrigger();
        }

    }

    private void Update()
    {
        if (GameMaster.deactivateHUD ^ lastHUDOption)
        {
            lastHUDOption = GameMaster.deactivateHUD;
            if (GameMaster.deactivateHUD)
            {
               // DisableTrigger();
            }
            else
            {
                //EnableTrigger();
            }
        }
    }
    //Disable this dialogue trigger so it cannot be used again
    public void DisableTrigger()
    {


        bc.enabled = false;
        
    }

    //Disable this dialogue trigger so it cannot be used again
    public void EnableTrigger()
    {
        bc.enabled = true;
        
    }
}

