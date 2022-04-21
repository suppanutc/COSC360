using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoNPC : MonoBehaviour
{
    Vector3 ogPos;
    Vector3 newPos;
    Vector3 targetPos;
    [SerializeField]
    float distance;
    [SerializeField]
    float moveRate;
    bool stop= false;
    SpriteRenderer sprite;
    FlyNPC fly;
    Animator anim;
    bool goToNewPos;
    // Start is called before the first frame update
    void Start()
    {
        ogPos = this.gameObject.transform.position;
        newPos = new Vector3(ogPos.x + distance, ogPos.y, ogPos.z);
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        targetPos = newPos;
        fly = GetComponentInChildren<FlyNPC>();
        
        // play running anim
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            if (!goToNewPos)
            {
                if (transform.position == targetPos)
                {
                    // check I am not already there...
                    if (ogPos == targetPos)
                    {
                        sprite.flipX = false;
                        targetPos = newPos;
                        fly.SwapPosition();
                    }
                    else if (newPos == targetPos)
                    {
                        sprite.flipX = true;
                        targetPos = ogPos;
                        fly.SwapPosition();
                    }
                }
                // Otherwise...
                else
                {
                    // Increment to that position.
                    transform.position = Vector3.MoveTowards(this.transform.position, targetPos, moveRate * Time.deltaTime);

                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(this.transform.position, targetPos, moveRate * Time.deltaTime);
            }
        }
    }


    // Set the wanted position of the NPC
    public void GoToPosition(Vector3 target)
    {
        targetPos = target;
        transform.position = transform.position;
        goToNewPos = true;
    }

    public void StopGecko()
    {
        stop = true;
        anim.Play("NPCGeckoIdle");
        // Play idle anim
    }
}
