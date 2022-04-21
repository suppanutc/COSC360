using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Team Gecko
 * 
 * COSC360 2021
 * 
 * A class that is used to loop the two backrounds elements of the game.
 * 
 */

public class Background : MonoBehaviour
{
    MeshRenderer mesh;
    [SerializeField]
    int sortingOrder = 0;
    [SerializeField]
    int parralaxModifier = 18;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.sortingLayerName = "Background";
        mesh.sortingOrder = sortingOrder;
    }

    private void Update()
    {
 
        Vector2 offset = mesh.material.mainTextureOffset;
        offset.x = gameObject.GetComponentInParent<Camera>().transform.position.x / parralaxModifier;
        mesh.material.mainTextureOffset = offset;
        
        
    }

}
