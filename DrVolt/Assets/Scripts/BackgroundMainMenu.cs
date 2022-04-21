using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Team Gecko
 * 
 * COSC360 2021
 * 
 * A class that is used to loop the two backrounds elements of the game.
 * 
 */

public class BackgroundMainMenu : MonoBehaviour
{
    MeshRenderer mesh;
    [SerializeField]
    int sortingOrder = 0;
    [SerializeField]
    float speed;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.sortingLayerName = "Background";
        mesh.sortingOrder = sortingOrder;
    }

    private void Update()
    {

        Vector2 offset = mesh.material.mainTextureOffset;
        offset.x += Time.deltaTime / speed;
        mesh.material.mainTextureOffset = offset;


    }
}