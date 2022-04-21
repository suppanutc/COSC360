using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBob : MonoBehaviour
{
    // Rate of the 'bob' movement
    public float bobRate;

    // Scale of the 'bob' movement
    public float bobScale;


    //int restartThreshold = 0;
    // Update is called once per frame
    void Update()
    {

        if (Time.timeScale != 0)
        {
            // Change in vertical distance 
            float dy = bobScale * Mathf.Sin(bobRate * Time.time);

            // Move te game object on the vertical axis
            transform.Translate(new Vector3(0, dy/4, 0));
        }

    }

}
