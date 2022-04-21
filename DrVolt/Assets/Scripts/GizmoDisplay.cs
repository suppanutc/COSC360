using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDisplay : MonoBehaviour
{
    private void Update()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.8f);
    }
}
