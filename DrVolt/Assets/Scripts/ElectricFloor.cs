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
public class ElectricFloor : Trap
{
    ParticleSystem[] electricity = new ParticleSystem[2];
    ElectricFloorEmmiter2 emmiter2;
    SpriteRenderer emmiter2Colour;
    bool emmiter2ColourNotSet = true;
    bool chargeChange;
    public AudioSource electricNoise;
    bool hasPlayed;
    new private void Start()
    {
        base.Start();
        emmiter2 = GetComponentInChildren<ElectricFloorEmmiter2>();
        emmiter2Colour = emmiter2.GetComponentInChildren<TrapColourChild>().GetComponent<SpriteRenderer>();
        electricity[0] = GetComponentInChildren<ParticleSystem>();
        electricity[1] = emmiter2.GetComponentInChildren<ParticleSystem>();
        boxCollider.offset = new Vector2(emmiter2.transform.localPosition.x/2, boxCollider.offset.y);
        boxCollider.size = new Vector2(emmiter2.transform.localPosition.x, boxCollider.offset.y);
        chargeChange = isOn;
        electricNoise = GetComponent<AudioSource>();
        if (!chargeChange)
        {
            electricity[0].Stop();
            electricity[1].Stop();
        }

        if (isOn)
        {
            electricNoise.Play();
            hasPlayed = true;
        }
    }

    new void Update()
    { 
        base.Update();
        if (isOn && !hasPlayed)
        {
            electricNoise.Play();
            hasPlayed = true;

        }
        else if (!isOn && hasPlayed)
        {
            electricNoise.Stop();
            hasPlayed = false;
        }
        if (emmiter2ColourNotSet)
        {
            emmiter2ColourNotSet = false;
            emmiter2Colour.color = currentColor;
        }
        
        if (chargeChange ^ isOn)
        {
            chargeChange = isOn;
            foreach (ParticleSystem emmiter in electricity)
            {
                // I have no idea how this works, it really shouldn't but it does so chur
                if (chargeChange)
                {
                    emmiter.Play();
                    
                }
                else
                {
                    emmiter.Stop();

                    
                }

            }
            
        }
        
    }


}
