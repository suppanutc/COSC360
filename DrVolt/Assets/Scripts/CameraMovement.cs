using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // variabale for speed of scientist
    public float cameraSpeed;
    //scientist object position
    public Transform scientist;
    //player postion
    public Transform player;
    //offset for the scientist for when the camera will start following
    // based off the centre, 8 == scientist at the end of camera
    [Range(0,8)]
    public float scientistOffset;
    // easy way to reverse the mixed camera movement
    public bool reversed;

    Transform rb;

  

    void Start()
    {
        // sets a variable named rb to the rigidbody of a specified object
        rb = gameObject.GetComponent<Transform>();
        transform.position = new Vector3(GameMaster.currentCheckpoint.x, transform.position.y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {

        if (!reversed)
        {
            //changes the camera postiion according to the who is further behind the other
            if (player.position.x + scientistOffset > scientist.position.x && player.position.x >0f)
            {
                Vector3 camPos = new Vector3(player.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, camPos, cameraSpeed * Time.deltaTime);
            }
            else if (player.position.x + scientistOffset < scientist.position.x && scientist.position.x > (0f + scientistOffset))
            {
                //changes position of camera to scientist
                Vector3 camPos = new Vector3(scientist.position.x - scientistOffset , transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, camPos, cameraSpeed * Time.deltaTime);
            }

        }
        
    }

}



