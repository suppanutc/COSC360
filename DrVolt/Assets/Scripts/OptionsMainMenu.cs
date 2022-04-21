using UnityEngine;
using UnityEngine.UI;


public class OptionsMainMenu : MonoBehaviour
{
    [Header("UI elements")]
    // Choose a colour for options text
    public Color textColour;
    // Reference to the option selector image
    public Transform selector, MainMenu;
    // The array of options available on main menu
    // Note: Represented in decending order from 0
    public Text[] optionArray;
    public Text[] selectionArray;
    public int optionIndex;
    [SerializeField]
    
    Slider volume;
    [SerializeField]
    public Transform electricity;
    public ParticleSystem particles;

    //string[] controlSchemeOptions = { "ArrowKeys", "WASD" };
    //string[] chargeSchemeOptions = { "Standard", "Classic" };

    void Start()
    {
        Time.timeScale = 1;
        // Set initial text option to selected colour
        // this.gameObject.SetActive(false); not needed
        // The slider for global volume
        volume = gameObject.GetComponentInChildren<Slider>();
        volume.value = GameMaster.volume;
        optionIndex = 0;
        //optionArray[1].text = GameMaster.movementControls.ToString("G");
        optionArray[2].text = GameMaster.chargeControls.ToString("G");
        //electricity.gameObject.GetComponent<ParticleSystem>().Stop();
        HUDSelectorTextToggle();
        particles.Play();

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
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)
          && optionIndex == 3)
        {
            particles.Clear();
            particles.Stop();
            this.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
            MainMenu.gameObject.GetComponent<MainMenuManager>().particleEffects.Play();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            HorizontalSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            HorizontalSelection(-1);
        }
        electricity.position = new Vector3(electricity.position.x,
    optionArray[optionIndex].gameObject.transform.position.y, electricity.position.z);




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
                if (!GameMaster.deactivateHUD)
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
        GameMaster.ChangeControlsKeyboard( GameMaster.chargeControls.ToString("G"));

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
}