using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public Text[] optionArray;
    public int optionIndex = 0;
    public Color textColour;
    public Transform effects;
    public Transform selector;
    public Transform MainMenu;
    public bool startUp = false;
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (startUp)
        {
            startUp = false;
            for (int i = 1; i < optionArray.Length; i++)
            {
                if (!GameMaster.levelsAvailable[i])
                {
                    optionArray[i].color = Color.grey;
                } else { optionArray[i].color = Color.white; }
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // De-highlight previous option
            if (GameMaster.levelsAvailable[optionIndex])
            {
                optionArray[optionIndex].color = new Color(1, 1, 1, 1);
            }
            
            // Move selector up
            optionIndex--;
            // Loop selector
            if (optionIndex < 0)
            {
                optionIndex = optionArray.Length - 1;
            }
            // Highlight current option
            optionArray[optionIndex].color = textColour;
            if (!GameMaster.levelsAvailable[optionIndex])
            {
                optionArray[optionIndex].color = Color.grey;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // De-highlight previous option
            if (GameMaster.levelsAvailable[optionIndex])
            {
                optionArray[optionIndex].color = new Color(1, 1, 1, 1);
            }
            // Move selector down and loop selector if needed
            optionIndex = (optionIndex + 1) % (optionArray.Length);
            // Highlight current option
            optionArray[optionIndex].color = textColour;
            if (!GameMaster.levelsAvailable[optionIndex])
            {
                optionArray[optionIndex].color = Color.grey;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKeyDown(KeyCode.Space)))
        {
            OptionSelected();
        }


        // Move selector sprite to the y position of the currently selected option
        selector.transform.position = new Vector3(selector.transform.position.x,
            optionArray[optionIndex].transform.position.y, selector.transform.position.z);
        effects.transform.position = new Vector3(selector.transform.position.x,
            optionArray[optionIndex].transform.position.y, selector.transform.position.z);
    }

    // Method that executes our selected option
    private void OptionSelected()
    {
        GameMaster.fromLevelSelect = true;
        switch (optionIndex)
        {
            // Load scene
            case 0:
                GameMaster.LoadScene("Level1");
                break;
            case 1:
                if (GameMaster.levelsAvailable[optionIndex])
                {
                    GameMaster.LoadScene("Level2");
                }
                break;
            case 2:
                if (GameMaster.levelsAvailable[optionIndex])
                {
                    GameMaster.LoadScene("Level3");
                }
                break;
            case 3:
                if (GameMaster.levelsAvailable[optionIndex])
                {
                    GameMaster.LoadScene("Bonus");
                }
                break;
            case 4:
                optionArray[optionIndex].color = new Color(1, 1, 1, 1);
                this.gameObject.SetActive(false);
                MainMenu.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        
    }
}
