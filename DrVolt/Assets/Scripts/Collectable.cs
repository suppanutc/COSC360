using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Team Gecko 
 * COSC360
 * 
 * A class that disables the collectable if the player has 
 * restarted more times than is allowed
 */
public class Collectable : MonoBehaviour
{
    // Rate of the 'bob' movement
    public float bobRate;

    // Scale of the 'bob' movement
    public float bobScale;

    
    int restartThreshold = 0;
    // Update is called once per frame
    void Update()
    {

        if (Time.timeScale != 0)
        {
            // Change in vertical distance 
            float dy = bobScale * Mathf.Sin(bobRate * Time.time);

            // Move the game object on the vertical axis
            transform.Translate(new Vector3(0, dy, 0));
        }
        // Disable the collectable if the player has restarted more times
        // than is allowed
        if (GameMaster.restarts > restartThreshold)
        {
            this.gameObject.SetActive(false);
        }
    }

    // When this collectable has been collected, disable it and report to gameMaster
    public void Collect()
    {
        GameMaster.collectablesCollected++;
        this.gameObject.SetActive(false);
    }
}
