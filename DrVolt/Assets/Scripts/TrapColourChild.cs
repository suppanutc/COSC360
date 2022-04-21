using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Team Gecko
 * 
 * COSC360 2021
 * 
 *  A class that colours the objects visual aid component so the
 *  player can easily identify related traps/pylons/platforms
 * 
 */
public class TrapColourChild : MonoBehaviour
{
    // Sprite renderer
    public Color colour;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {

        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = colour;
    }
}



//   // Method that sets the colour of the player visual aid
//   public void colourVisualAid(Color color)
//    {
        
//        sprite.color = color;

        
//    }
//}
