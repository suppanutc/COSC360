/* 
 * Team Gecko Cosc360 2020
 * 
 *  A child class of Trap. Used to distinguish
 *  Visually from other traps, while maintaining
 *  key inherited trap functionality
 *  See trap class for info on methods and variables
 *
 */
using UnityEngine;
public class Piston : Trap
{

    public float speed;

    public float distance;

    public bool Up;
    BoxCollider2D bc;
    int direction = 1;
    public bool touchingGround;
    float ypos;
    public LayerMask seperator;
    bool basePanelColourSet = false;
    public TrapColourChild basePanel;

    // Start is called before the first frame update
    new void Start()
    {
        
        base.Start();
        bc = GetComponent<BoxCollider2D>();
        ypos = transform.position.y;
       
        
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        float dy = 0f;
        if (!basePanelColourSet)
        {
            basePanelColourSet = true;
            basePanel.GetComponent<SpriteRenderer>().color = currentColor;
        }
        // Controls platforms that begin by moving upwards.
        // If the platform moves up to the maximum distance, it begins moving
        // downwards.
        if (Up == true)
        {
            if (transform.position.y >= (ypos + distance))
            {
                direction = -1;
            }

            else if (transform.position.y <= (ypos))
            {
                direction = 1;
            }
        }

        // Controls platforms that begin by moving downwards.
        // If the platform moves down to the maximum distance, it begins
        // moving back upwards.
        else if (Up == false)
        {
            if (transform.position.y <= (ypos - distance))
            {
                direction = 1;
            }

            else if (transform.position.y >= (ypos))
            {
                direction = -1;
                GetComponent<AudioSource>().Play();
            }
        }


        // If the trap is active, then the platform will move.
        // Faster down, slower up.
        if (this.GetComponent<Trap>().IsOn() == true)
        {
            dy = Time.deltaTime * direction * speed;
            if (direction < 0) 
            {
                bc.enabled = true;
                transform.Translate(new Vector3(0, dy * 1.2f, 0));
            }
            else
            {
                bc.enabled = false;
                transform.Translate(new Vector3(0, dy * 0.5f, 0));
            }
            

        }

        // Returns the platform back to the starting position
        // when the trap is disabled (or very close to it).
        if (this.GetComponent<Trap>().IsOn() == false && transform.position.y != ypos)
        {

            // Checks whether the difference between current position and original
            // position is greater than 0.05. Without this check, the platforms will
            // never perfectly reach their starting position and will jitter up
            // and down continuously.
            if (transform.position.y > ypos - 0.05 && transform.position.y < ypos + 0.05)
            {
                // This should in theory stop the jittering
                transform.position = new Vector3(transform.position.x, ypos, transform.position.z);
            }
            else  if ((transform.position.y >= ypos) && ((transform.position.y - ypos) > 0.05))
            {
                direction = -1;
                dy = Time.deltaTime * direction * speed;
                transform.Translate(new Vector3(0, dy, 0));
            }
            else if (transform.position.y <= ypos)
            {
                direction = 1;
                dy = Time.deltaTime * direction * speed;
                transform.Translate(new Vector3(0, dy, 0));
            } 
        }

    }


}