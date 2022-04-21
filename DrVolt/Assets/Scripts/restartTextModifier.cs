using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class restartTextModifier : MonoBehaviour
{
    // Reference to this objects text box
    Text restartText;
    // Start is called before the first frame update
    void Start()
    {
        // sets local text var equal to the compenent text of the game object
        restartText = this.gameObject.GetComponent<Text>();
        // defines how many restarts have occured
        restartText.text = "RESTARTS : " + GameMaster.restarts;
        
    }
    
}
