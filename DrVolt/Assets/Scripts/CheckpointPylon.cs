using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Team Gecko
 * COSC360 2020
 * 
 * Class that holds all methods for the checkpoint pylons
 * mainly used as of 3/2/2020 to deactivate if player has used this checkpoint
 */
[RequireComponent(typeof(CircleCollider2D))]
public class CheckpointPylon : MonoBehaviour
{

    // Has this checkpoint been used before?
   // bool checkpointUsed;

    // Reference to this objects collider
    CircleCollider2D circleCollider;
    // Start is called before the first frame update
    void Start()
    {
        // instantiate reference to this object collider
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (checkpointUsed)
    //    {
    //        circleCollider.enabled = false;
    //    }
    //}

    // if player has already used this checkpoint, disable it
    public void disableCheckpoint()
    {
        //checkpointUsed = true;
        circleCollider.enabled = false;
        gameObject.GetComponentInChildren<CheckpointText>().displayCheckpointText();

    }
}
