using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * Team Gecko 
 * COSC360 2020
 * 
 * A class that manages a pause menu and executes the players 
 * selected option
 * 
 */
public class PauseMenuManager : MonoBehaviour
{
    [Header("UI elements")]
    // Choose a colour for options text
    public Color textColour;
    // Reference to the option selector image
    public Transform selector, optionsMenu, pauseMenu;
    // The array of options available on main menu
    // Note: Represented in decending order from 0
    public Text[] optionArray;
    public Text restartText;
    public int optionIndex = 0;

    // Is the game paused?
    bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        // Set initial text option to selected colour
        optionArray[optionIndex].color = textColour;

        // The selector images original position
        optionIndex = 0;

    }

    // Update is called once per frame
    void Update()
    {
        // Player uses arrow keys to highlight option
        // Space or enter will execute the option
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            // De-highlight previous option
            optionArray[optionIndex].color = Color.white;
            // Move selector up
            optionIndex--;
            // Loop selector
            if (optionIndex < 0)
            {
                optionIndex = optionArray.Length - 1;
            }
            // Highlight current option
            optionArray[optionIndex].color = textColour;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            // De-highlight previous option
            optionArray[optionIndex].color = new Color(1, 1, 1, 1);
            // Move selector down and loop selector if needed
            optionIndex = (optionIndex + 1) % (optionArray.Length);
            // Highlight current option
            optionArray[optionIndex].color = textColour;
        }
        else if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKeyDown(KeyCode.Space)))
        {
            OptionSelected();
        }

        // Move selector sprite to the y position of the currently selected option
        selector.transform.position = new Vector3(selector.transform.position.x,
            optionArray[optionIndex].transform.position.y, selector.transform.position.z);

        if (isPaused)
        {
            Time.timeScale = 0;
        }
    }

    // Method that executes our selected option
    private void OptionSelected()
    {
        switch (optionIndex)
        {
            // ResumeGame
            case 0:
                TogglePlay();

                break;
            // options
            case 1:
                optionsMenu.gameObject.SetActive(true);
                OptionsManager optionMenuIndex = optionsMenu.gameObject.GetComponent<OptionsManager>();
                
                optionMenuIndex.optionIndex = 0;
                optionMenuIndex.ColourSelectedSection();
                OptionsManager temp = optionsMenu.GetComponent<OptionsManager>();
                
                temp.optionArray[temp.optionIndex].color = Color.white;
                temp.optionIndex = 0;
                temp.optionArray[0].color = temp.textColour;
                
                pauseMenu.gameObject.SetActive(false);
                break;
            case 2:
                GameMaster.reloaded = true;
                GameMaster.ReLoadLevel();
                break;
            case 3:
                GameMaster.LoadScene("MainMenu");
                break;
            default:
                break;
        }
    }

    // Toggle game pause screen
    public void TogglePlay()
    {
        if (isPaused)
        {
            optionArray[optionIndex].color = Color.white;
            isPaused = false;
            optionIndex = 0;
            optionArray[optionIndex].color = textColour;
            Time.timeScale = 1f;
            optionsMenu.gameObject.SetActive(false);
            restartText.gameObject.SetActive(true);
            transform.parent.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

        }
        else
        {
            //optionArray[optionIndex].color = Color.white;
            isPaused = true;
            optionIndex = 0;
            restartText.gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(true);
            this.gameObject.SetActive(true);
        }
    }
}

