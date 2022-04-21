using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    MeshRenderer mesh;
    Trap parent;
    Color trapColor;
    // Start is called before the first frame update
    void Start()
    {
        mesh = this.GetComponent<MeshRenderer>();
        parent = transform.parent.gameObject.GetComponent<Trap>();
        trapColor = parent.currentColor;
        trapColor.a = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
        mesh.material.color = trapColor;
        Vector2 offset = mesh.material.mainTextureOffset;
        mesh.sortingLayerName = "TrapLayer";
        mesh.sortingOrder = 0;
        
        if (parent.isOn)
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, Time.deltaTime);
            offset.y -= Time.deltaTime / 5f;
            mesh.material.mainTextureOffset = offset;
        }
        else if (!parent.isOn)
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 180, Time.deltaTime);
            offset.y -= Time.deltaTime / 5f;
            mesh.material.mainTextureOffset = offset;
        }
    }
}
