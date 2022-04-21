/* 
 * Team Gecko Cosc360 2020
 * 
 *  A child class of Trap. Used to distinguish
 *  Visually from other traps, while maintaining
 *  key inherited trap functionality
 *  See trap class for info on methods and variables
 *
 */
using UnityEngine;
public class Laser : Trap
{
    // Get a reference to this objects Laser beam Sprite
    LaserBeam laserBeam;

    new void Start()
    {
        base.Start();
        laserBeam = gameObject.GetComponentInChildren<LaserBeam>();
    }

    new void Update()
    {
        base.Update();
        laserBeam.ToggleBeam(isOn);
    }

    //No idea why these dont work a
    //new public void Deactivate()
    //{
    //    
    //    isOn = false;

    //}

    ////Turn this trap on
    //new public void Activate()
    //{
    //    laserBeam.ToggleBeam(isOn);
    //    isOn = true;

    //}


}

