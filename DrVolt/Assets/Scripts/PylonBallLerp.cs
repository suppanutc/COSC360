using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Team Gecko 
 * 
 * COSC360 2021
 * 
 * A class that animates the pylon's ball position depending on
 * the charge status of the pylon.
 * 
 */
public class PylonBallLerp : MonoBehaviour
{
    // Fields
    [SerializeField]
    Pylon pylonBase;
    float bobScale = 0.0025f;
    float MoveRate = 4f;
    float bobRate = 7f;
    float timeElapsed = 0; 
     Vector3 onPosition;
     Vector3 offPosition;
     Vector3 targetPositon;

    // Bools g
    bool animateBall = true;
    bool hasCharge;
    bool inNewPosition;
    bool inMotion;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure I am where I should be and set my charge to
        // that of my parents
        offPosition = pylonBase.transform.position;
        onPosition = transform.position;
        hasCharge = pylonBase.hasCharge;
        if (!hasCharge)
        {
            animateBall = false;
            transform.position = offPosition;
        }
        inNewPosition = true;
        inMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animateBall && Time.timeScale != 0)
        {
            timeElapsed -= Time.deltaTime;
            float dy = bobScale * Mathf.Sin(bobRate * timeElapsed);//Time.time);
            transform.Translate(new Vector3(0, dy, 0));
        }
        // If the pylon has toggled it's charge...
        if (hasCharge ^ pylonBase.hasCharge)
        {
            hasCharge = pylonBase.hasCharge;
            // Stop the bob anim...
            timeElapsed = 0;
            animateBall = false;
            inNewPosition = false;

            // If my parent has a charge i must be in the off position...
            // or traveling to it...
            if (!hasCharge)
            {
                // So set destination to off position.
                targetPositon = offPosition;

            }
            // else I must be in the on postion or traveling to it...
            else
            {
                // So set destination to the off postion.
                targetPositon = onPosition;
            }
        }


        // if I am not in my target destination...
        if (!inNewPosition)
        {
            // check I am not already there...
            if (transform.position == targetPositon && inMotion)
            {    
                inNewPosition = true;
            } 
            // Otherwise...
            else
            {
                // Increment to that position.
                transform.position = Vector3.MoveTowards(this.transform.position, targetPositon, MoveRate * Time.deltaTime);
                inMotion = true;
            }
        } 
        
        // If the ball has returned to the on postion...
        if (inNewPosition && hasCharge)
        {
            // Set animation state to true.
            animateBall = true;
        } 

    }
}

