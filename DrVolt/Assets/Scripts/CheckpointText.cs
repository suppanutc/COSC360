using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * Team Gecko 
 * COSC360 2020
 * 
 * A class that displays visual text feedback when a player reaches a checkpoint
 */
public class CheckpointText : MonoBehaviour
{
    Text textBox;

    [SerializeField]
    private float timeAlive = 3f;
     
    float timeLeftAlive = 0;
    float letterTime = 0.2f;
    float letterSpawn;
    string displayText;
    char[] letters = { 'C', 'H', 'E', 'C', 'K', 'P', 'O', 'I', 'N', 'T'};
    int index = 0;
    bool textActive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        textBox = GetComponent<Text>();
        textBox.text = "";
        letterSpawn = letterTime;

    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeftAlive > 0)
        {
            timeLeftAlive -= Time.deltaTime;
            letterSpawn -= Time.deltaTime;
            // Display checkpoint text letter by letter, like oldschool games
            if (index < letters.Length && letterSpawn < 0)
            {
                textBox.text = textBox.text + letters[index];
                index++;
                letterSpawn = letterTime; 
            } 
            // flash letter for the remaining time left alive
            else if (letterSpawn < 0)
            {
                letterSpawn = letterTime;
                if (textActive)
                {
                    displayText = textBox.text;
                    textBox.text = "";
                    textActive = false;
                } else
                {
                    textBox.text = displayText;
                    textActive = true;
                }

            }
        } else { textBox.text = ""; }

    }

    // method that starts displaying the checkpoint text
    public void displayCheckpointText()
    {
        timeLeftAlive = timeAlive;
    }
}
