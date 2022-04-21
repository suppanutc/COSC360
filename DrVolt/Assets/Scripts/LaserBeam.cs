using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * TG C360 21
 * 
 * A class to deactuivate the deadly laser beam so big glases man doesn't kick the bucket
 * I've written to many class headers now a
 * 
 */
public class LaserBeam : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer beamSprite;
    void Start()
    {

        // Set them bad boys up
        beamSprite = GetComponent<SpriteRenderer>();
    }

    // Deactivate as needed g lessgo
    public void ToggleBeam( bool isOn)
    {
        if (isOn)
        {

            beamSprite.enabled = true;
        }
        else
        {

            beamSprite.enabled = false;
        }
    }
}
