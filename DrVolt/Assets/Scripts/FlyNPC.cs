using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyNPC : MonoBehaviour
{
    
    Vector3 endPos;
    Vector3 ogPos;
    Vector3 targetPos;
    [SerializeField]
    float moveRate;
    bool stop = false;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        ogPos = this.gameObject.transform.localPosition;
        targetPos = ogPos;
        endPos = new Vector3(transform.localPosition.x * -1, transform.localPosition.y, transform.localPosition.z);
        sprite = GetComponent<SpriteRenderer>();


        // play 
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {


            if (targetPos != transform.localPosition)
            {
                // Increment to that position.
                transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPos, moveRate * Time.deltaTime);
            }

            
        }
    }


    // Set the wanted position of the NPC
    public void GoToPosition(Vector3 target)
    {
        targetPos = target;
    }

    public void StopFly()
    {
        stop = true;
        
    }

    public void SwapPosition()
    {
        if (targetPos == ogPos)
        {
            sprite.flipX = true;
            targetPos = endPos;
        }
        else
        {
            sprite.flipX = false;
            targetPos = ogPos;
        }
    }
}
