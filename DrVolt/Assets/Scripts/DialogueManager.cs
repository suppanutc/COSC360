using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [SerializeField]
    float dialogueTimeAlive = 3f;
    float dialogueTimer = 0;
    float letterFrequency = 0.025f;
    string currentHint;
    [SerializeField]
    Text textDisplay;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {




    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueTimer > 0)
        {
            dialogueTimer -= Time.deltaTime;
            if (index < currentHint.Length && dialogueTimer < dialogueTimeAlive + (letterFrequency * currentHint.Length) - (letterFrequency *index))
            {
                AddLetter();
                index++;
            }
        }
        else{
            
            textDisplay.text = "";
            index = 0;
            gameObject.SetActive(false);
        }

        
    }



    // Add each char in string to text one by one
    void AddLetter()
    {
        textDisplay.text += currentHint[index];
    }

    public void BeginDialogue()
    {

        
        index = 0;

        if (GameMaster.dialogue != null)
        {
            textDisplay.text = "";
            currentHint = GameMaster.dialogue[GameMaster.hintCounter];
            dialogueTimer = dialogueTimeAlive + (letterFrequency * currentHint.Length);
        }
    }
}
