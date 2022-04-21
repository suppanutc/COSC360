using UnityEngine;

/**
 * Team gecko 
 * 
 * COSC360 2021
 * 
 * A class that controls a "forcefield" door that stops the player from going 
 * to far forward. When the scientist steps on the button, the door will deactivate
 * 
 */
public class DoorControl : MonoBehaviour
{
    SpriteRenderer sprite;
    BoxCollider2D boxCollider;
    [SerializeField]
    Transform doorLaser;
    [SerializeField]
    CircleCollider2D cCollider;
    bool hasCharge = true;
    float moverate = 5f;
    float distance = 5f;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!hasCharge)
        {
            boxCollider.enabled = false;
            sprite.enabled = false;
            
            if( cCollider.enabled == true)
            {
                cCollider.enabled = false;
            }

            // Move off screen if button pressed
            doorLaser.transform.position = Vector3.MoveTowards(doorLaser.transform.position,
                doorLaser.transform.position + Vector3.down * distance, moverate * Time.deltaTime);
            if (doorLaser.transform.position.y <= -distance*2)
            {
                doorLaser.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                doorLaser.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                
            }
        }
    }


    // Disables the door laser, and the disables this button to give the illusion of it being pressed
    public void DisableDoor()
    {
        hasCharge = false;
    }
}
