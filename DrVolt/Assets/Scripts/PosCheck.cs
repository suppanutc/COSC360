using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosCheck : MonoBehaviour
{
    public Transform scientist;
    bool timerIsActive;
    float timer = 2f;
    GameObject child;


    // Start is called before the first frame update
    void Start()
    {
        child = gameObject.transform.GetChild(0).gameObject;
        


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(scientist.transform.position);

        if (pos.x < 0.0)
        {
            timerIsActive = true;
            if (timer < 0)
            {
                child.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                timerIsActive = false;
                
            }
            else if(!timerIsActive)
            {
                timer = 2f;
                
            }
            
            child.SetActive(true);
            timer -= Time.deltaTime;
            
        }
        else
        {
            child.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            child.SetActive(false);
            


        }


    }
    
}
