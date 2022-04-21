using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour
{
    [Header("UI elements")]
    // Choose a colour for options text
    public Color textColour;
    // Reference to the option selector image
    public Transform selector, pauseMenu;
    // The array of options available on main menu
    // Note: Represented in decending order from 0
    public Text[] optionArray;
    public Text[] selectionArray;
    public int optionIndex;
    [SerializeField]
    Text[] controlsTextField;
    Slider volume;
    [SerializeField]
    LevelMaster levelMaster;
    
    //string[] controlSchemeOptions = { "ArrowKeys", "WASD" };
    //string[] chargeSchemeOptions = { "Standard", "Classic" };

    void Start()
    {
        // Set initial text option to selected colour
        // this.gameObject.SetActive(false); not needed
        // The slider for global volume
        volume = gameObject.GetComponentInChildren<Slider>();
        volume.value = GameMaster.volume;
        optionIndex = 0;
        optionArray[1].text = GameMaster.chargeControls.ToString("G");

        HUDSelectorTextToggle();

    }

    private void Update()
    {
        // Player uses arrow keys to highlight option
        // Space or enter will execute the option

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            // Move selector up
            optionIndex--;
            // Loop selector
            if (optionIndex < 0)
            {
                optionIndex = optionArray.Length - 1;
            }
            ColourSelectedSection();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            // Move selector down and loop selector if needed
            optionIndex = (optionIndex + 1) % (optionArray.Length);
            ColourSelectedSection();
        } else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) 
            && optionIndex == 3)
        {
                
                this.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(true);
        } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            HorizontalSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            HorizontalSelection(-1);
        }





        // Move selector sprite to the y position of the currently selected option
        selector.transform.position = new Vector3(selector.transform.position.x,
            optionArray[optionIndex].transform.position.y, selector.transform.position.z);

    }

    // Colour the correct section area
    public void ColourSelectedSection()
    {
        // Decoulour what ever one was previoulsy selected
        foreach (Text button in selectionArray)
        {
            button.color = Color.white;
        }
        // Colour the new one
        switch (optionIndex)
        {
            case 0:
                selectionArray[0].color = textColour;
                break;
            case 1:
                selectionArray[1].color = textColour;
                break;
            case 2:
                selectionArray[2].color = textColour;
                break;
            case 3:
                selectionArray[3].color = textColour;
                break;
            default:
                //selectionArray[1].color = textColour;
                break;

        }
    }

    // Cycle through our selections
    void HorizontalSelection(int direction)
    {
        switch (optionIndex)
        {
            // Volume control
            case 0:
                if (volume.value <= 1 && volume.value >= 0)
                {
                    // Increase/Decrease slider volume by 10 percent
                    volume.value += 0.1f * direction;
                }
                // set global volume 
                AudioListener.volume = volume.value;
                GameMaster.SetVolume(volume.value);
                
                break;
            // Movement
            
            // Jump/Charge
            case 1:
                if (GameMaster.chargeControls == GameMaster.ChargeEnum.Classic)
                {
                    GameMaster.chargeControls = GameMaster.ChargeEnum.Standard;
                }
                else
                {
                    GameMaster.chargeControls = GameMaster.ChargeEnum.Classic;
                }
                optionArray[optionIndex].text = GameMaster.chargeControls.ToString("G");
                ChangeControls();
                break;
            case 2:
                if(!GameMaster.deactivateHUD)
                {
                    GameMaster.deactivateHUD = true;
                    
                }
                else
                {
                    GameMaster.deactivateHUD = false;
                }
                
                HUDSelectorTextToggle();
                break;
            default:
                break;
        }
    }

    // Change the controls in game master
    void ChangeControls()
    {
        
        GameMaster.ChangeControlsKeyboard(
        GameMaster.chargeControls.ToString("G"));
         levelMaster.setControls();  
    }

    // Toggle HUD selector text 
    private void HUDSelectorTextToggle()
    {
        if (!GameMaster.deactivateHUD)
        {
            optionArray[2].text = "On";

        }
        else
        {
            optionArray[2].text = "Off";
        }
    }
}// end










// Sorry b had to rewrite, i dont understand coroutines,
// I did manage to solve all edge cases and you should be able 
// to use this exact code in a new class, MainMenuManagaer 
// with a few tweaks for the main menus Option panel

    /**
     * A method that determines what happens when a particular option is selected. 
//     * Calls Coroutines to handle changing of volume and control scheme.
//     */
    //    private void OptionIsSelected()
    //    {
    //        switch (optionIndex)
    //        {
    //            // volume slider case
    //            case 0:

    //                if (optionIndex > 0)
    //                {
    //                    Debug.Log("exit loop");
    //                    break;
    //                }
    //                if (Input.GetKeyDown(KeyCode.RightArrow) && volume.value < 1)
    //                {
    //                    // increases slider volume by 10 percent
    //                    volume.value += 0.1f;
    //                }
    //                if (Input.GetKeyDown(KeyCode.LeftArrow) && volume.value > 0)
    //                {
    //                    // decerases slider volume by 10 percent
    //                    volume.value -= 0.1f;

    //                }
    //                break;
    //            // control scheme case
    //            case 1:
    //                StartCoroutine(onIterator(controlSchemeOptions, 0));
    //                //Debug.Log("control");
    //                break;
    //            case 2:
    //                //charge scheme
    //                StartCoroutine(onIterator(chargeSchemeOptions, 1));
    //                break;
    //            case 3:
    //                if (Input.GetKeyDown(KeyCode.Return))
    //                {
    //                    if (SceneManager.GetActiveScene().name == "Options")
    //                    {
    //                        //LOAD MAIN MENU CANVAS
    //                        break;
    //                    }
    //                    else
    //                    {
    //                        this.gameObject.SetActive(false);
    //                        pauseMenu.gameObject.SetActive(true);

    //                        //Debug.Log("optionsManager");
    //                    }

    //                }
    //                break;


    //            default:
    //                break;
    //        }
    //        GameMaster.ChangeControlsKeyboard(controlSchemeOptions[GameMaster.prevControlIndx],
    //                            chargeSchemeOptions[GameMaster.prevChargeIndx]);
    //    }
    //    IEnumerator onIterator(string[] stringSchemes, int textObjectPos)
    //    {
    //        // a while loop that changes control scheme based of user input.
    //        // still needs some refining. Alot of refining I mean
    //        int prevOption = optionIndex;
    //        controlsTextField[textObjectPos].text = (textObjectPos == 0) ?
    //            stringSchemes[GameMaster.prevControlIndx] : stringSchemes[GameMaster.prevChargeIndx];
    //        int i = (textObjectPos == 0) ? GameMaster.prevControlIndx : GameMaster.prevChargeIndx;
    //        while (true)
    //        {
    //            if (prevOption != optionIndex)
    //            {
    //                Debug.Log("I finally broke free!!");
    //                break;
    //            }
    //            if (Input.GetKeyDown(KeyCode.LeftArrow))
    //            {
    //                i--;
    //                if (i < 0)
    //                {
    //                    i = stringSchemes.Length - 1;
    //                }
    //            }
    //            if (Input.GetKeyDown(KeyCode.RightArrow))
    //            {
    //                i = (i + 1) % (stringSchemes.Length);
    //            }
    //            controlsTextField[textObjectPos].text = stringSchemes[i];

    //            yield return null;

    //        }
    //        GameMaster.prevControlIndx = (textObjectPos == 0) ? i : GameMaster.prevChargeIndx = i;
    //        yield break;
    //    }

