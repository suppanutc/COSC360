using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI elements")]
    // Choose a colour for options text
    public Color textColour;
    // Reference to the option selector image
    public Transform selector;
    // The array of options available on main menu
    // Note: Represented in decending order from 0
    public Text[] optionArray;
    int optionIndex = 0;
    bool particlePlay = false;
    
    public ParticleSystem particleEffects;
    public Transform effects;
    // Start is called before the first frame update
    public OptionsMainMenu optionsMenu;
    public LevelSelectManager LevelSelect;
    [SerializeField]
    Transform scientist;
    Animator amin;
    void Start()
    { 
        // Set initial text option to selected colour
        optionArray[optionIndex].color = textColour;
        // The selector images original position
       
        particleEffects.Play();
        amin = scientist.GetComponent<Animator>();
        Time.timeScale = 1f;
        GameMaster.fromLevelSelect = false;
        GameMaster.fromNextLevel = false;
        GameMaster.reloaded = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (scientist != null)
        {
            amin.Play("PrologueScientistWalk");
        }
        {

        }
        if (!particlePlay)
        {
            particlePlay = true;
            particleEffects.Play();
        }
        // Player uses arrow keys to highlight option
        // Space or enter will execute the option
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // De-highlight previous option
            optionArray[optionIndex].color = new Color(1, 1, 1, 1);
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
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // De-highlight previous option
            optionArray[optionIndex].color = new Color(1,1,1,1);
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
        effects.transform.position = new Vector3(selector.transform.position.x,
            optionArray[optionIndex].transform.position.y, selector.transform.position.z);



    }

    // Method that executes our selected option
    private void OptionSelected()
    {
        switch (optionIndex)
        {
            // Play Game
            case 0:
                particlePlay = false;
                GameMaster.LoadScene("Prologue");
                break;
            // Toggle LevelSelectManager ?
            case 1:
                LevelSelect.gameObject.SetActive(true);
                LevelSelect.optionIndex = 0;
                LevelSelect.optionArray[LevelSelect.optionIndex].color = textColour;
                LevelSelect.startUp = true;
                LevelSelect.optionIndex = 0;
                LevelSelect.optionArray[0].color = LevelSelect.textColour;
                this.gameObject.SetActive(false);
                break;
            case 2:
                particlePlay = false;
                particleEffects.Clear();
                particleEffects.Stop();
                
                optionsMenu.gameObject.GetComponent<OptionsMainMenu>().particles.Play();
                optionsMenu.gameObject.SetActive(true);
                optionsMenu.gameObject.GetComponent<OptionsMainMenu>().particles.Play();
                
                optionsMenu.optionIndex = 0;
                optionsMenu.ColourSelectedSection();
     

                optionsMenu.optionArray[optionsMenu.optionIndex].color = Color.white;
                optionsMenu.optionIndex = 0;
                optionsMenu.optionArray[0].color = optionsMenu.textColour;
                this.gameObject.SetActive(false);

                break;
            case 3:
                 Quit(); 
                break;
            default:
                break;
        }
    }
    public void Quit()
    {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
    Application.Quit();
#elif (UNITY_WEBGL)
    Application.OpenURL("");
#endif
    }
}
