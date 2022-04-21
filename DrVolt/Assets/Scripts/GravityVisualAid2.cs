using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityVisualAid2 : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {

        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = gameObject.GetComponentInParent<Trap>().currentColor;
    }
}
