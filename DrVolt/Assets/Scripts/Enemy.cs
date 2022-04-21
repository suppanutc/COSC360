using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemySpeed;
    Transform rb;

    void Start()
    {
        rb = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * enemySpeed;

    }
    void OnTriggerEnter2D(Collider2D other)

    {
        GameObject trigger = other.gameObject;

        // If the trigger is an active trap, the enemy dies
        if (trigger.tag == "Trap" && trigger.GetComponent<Trap>().IsOn())
        {
            gameObject.SetActive(false);
        }
    }

}