using UnityEngine;
/**
 * A moving platform Trap variant that allows player
 * to "stick" to platform.
 * VERSION 3 and the final version tf, way to much time has been spent on this
 * trying to make it not jitter
 */

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : Trap
{
    [SerializeField]
    Sprite upDownSprite;
    [Header("Movement")]
    public float speed = 1;
    public float distance = 0;
     float snapFactor = 0.05f;
     Vector3 startPositon;
     Vector3 endPosition;
     Vector3 targetPosition;
    // Enable a drop down selection for user that 
    // determines which way this platform moves
    public enum Direction { up, down, left, right };
    public Direction direction;
     bool hasCharge;
     bool inMotion;
     bool atTarget;
     int directionY = 0;
     int directionX = 0;
    bool started = true;
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
        startPositon = transform.position;
        // Initialise this platforms Direction modifier
        SetDirection();
        endPosition = new Vector3(transform.position.x + (distance * directionX)
            , transform.position.y + (distance * directionY), transform.position.z);
        
        if (direction == Direction.up || direction == Direction.down)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = upDownSprite;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        if (started)
        {
           
            hasCharge = isOn;
            if (hasCharge)
            {
                started = false;
                atTarget = false;
                inMotion = true;
                targetPosition = endPosition;
            }
            else
            {
                targetPosition = startPositon;
            }
        }
        // If charge is toggled swap target destination
        if (hasCharge ^ isOn)
        {
            // set my charge
            hasCharge = isOn;
            // If I now have a charge, I must have been off so...
            if (hasCharge)
            {
                
                /**
                 * I must have been either heading to the start or at the start...
                 * So check If I am at the start, if I am, set my position to the end position
                 * other wise my target position is already the start so keep going there and I will 
                 * change any way when i reach that position, yeah good
                 */
                if (transform.position == startPositon)
                {
                    atTarget = false;
                    inMotion = true;
                    targetPosition = endPosition;
                    SwapDirection();
                } // else no change

            }
            // Otherwise I was on  and have been turned off so...
            else 
            {
                /** 
                 *  Check that my current target position is end...
                 * if it is then I must have been going there so set my position to the start position 
                 * where I will stay until I am turned on.
                 */
                if (targetPosition == endPosition)
                {
                    atTarget = false;
                    inMotion = true;
                    targetPosition = startPositon;
                    SwapDirection();
                }
            }
        }

        // I f I am on I was to oscilate between start and end so..
        if (hasCharge && targetPosition == transform.position)
        {
            atTarget = false;
            inMotion = true;
            SwapDirection();
            // Swap my target position
            // If I am at the start...
            if (targetPosition == startPositon)
            {
                // Go to the end.
                targetPosition = endPosition;  
            }
            // Otherwise I was at the end so...
            else
            {
                // Go to the start.
                targetPosition = startPositon;
            }

        }

        // Move me to where I need to go
        if (inMotion)
        {
            rb.MovePosition(transform.position + new Vector3(directionX * speed * Time.fixedDeltaTime, directionY * speed * Time.fixedDeltaTime, transform.position.z));
        }
        // if I am within snapFactor of my target position, snap to it. 
        if (transform.position.y > targetPosition.y - snapFactor && transform.position.y < targetPosition.y + snapFactor
            && transform.position.x > targetPosition.x - snapFactor && transform.position.x < targetPosition.x + snapFactor && !atTarget)
        {

            inMotion = false;
            atTarget = true;
            transform.position = targetPosition;
        }
    }

    // Method that sets the originaly direction of the platform
    void SetDirection()
    {
        switch (direction)
        {
            case Direction.up:
                directionY = 1;
                break;
            case Direction.down:
                directionY = -1;
                break;
            case Direction.left:
                directionX = -1;
                break;
            default:
                directionX = 1;
                break;
        }
    }

    // A method that swaps the direction, regardless of x and y
    void SwapDirection()
    {
        directionY *= -1;
        directionX *= -1;
    }
}