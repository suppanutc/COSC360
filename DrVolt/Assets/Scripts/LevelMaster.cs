using UnityEngine;
using UnityEngine.UI;
/**
 * Team Gecko 
 * COSC360 2020
 * 
 * A class that reloads/pauses the game and takes care of level oriented actions
 */
public class LevelMaster : MonoBehaviour

{
    // Reference to player, Companion and pauseMenu
    public Transform player;
    public Transform scientist;
    public PauseMenuManager pauseMenu;
    public EndGamePanelMaster endGamePanel;
    [SerializeField]
    PlayerPanel restartCanvas;
    // Booleans so we dont keep calling the same external methods
    bool playerInMotion;
    bool deathStatusKnown;
    bool levelClearedStatusKnown;
    [SerializeField]
    Sound soundManager;
    float endTimer = 2f;
    bool spacePressed;
    [SerializeField]
    Transform endPipe;
    // the players starting x position used for setting the scientist in motion
    // as an alterantive to a timer
    float playerX;
    // Start is called before the first frame update
    void Start()
    {
        soundManager.startGameMusic();
        if (GameMaster.reloaded || GameMaster.restarts > 0)
        {
            soundManager.stopLoopMusic();
            Destroy(soundManager.gameObject);

        }

        //playerPanel = player.gameObject.GetComponentInChildren<PlayerPanel>().gameObject;
        // Initialize volume based on changes in main menu, if there were any
        AudioListener.volume = GameMaster.volume;
        
        // Disable pause screen if active
        pauseMenu.gameObject.SetActive(false);
        // Ensure the game runs on start i.e we havent entered main
        // menu from a pause screen
        Time.timeScale = 1f;
        // If te player has restarted from a checkpoint, reload from there

        if (GameMaster.currentCheckpoint != Vector3.zero)
        {
            
            player.position = GameMaster.currentCheckpoint;
            scientist.position = new Vector3(player.position.x, scientist.position.y,
                scientist.position.z);
           

        }
        else // reset playercharge to default
        { GameMaster.playerCharge = false;
            GameMaster.hintCounter = 0;
        }
        playerX = player.transform.position.x;

        // Ensure we let the GameMaster knows the level has not yet been cleared
        GameMaster.LevelCleared = false;
        GameMaster.reloaded = false;


    }

    // Update is called once per frame
    void Update()
    {
        // move the scientist only when the player moves so they 
        //can study the upcoming traps


        if (!playerInMotion)
        { 
            if (PlayerHasMoved())
            {
                playerInMotion = true;
                scientist.gameObject.GetComponent<GravityScientist>().playerMoved = true;
            }
        }

        
        // Check to see if player wishes to reload the current scene
        if (Input.GetKeyDown(KeyCode.R) && Time.timeScale != 0)
        {

            GameMaster.hintCounter = GameMaster.checkPointHintCounter;
            
            GameMaster.RestartLevel();

        }
        // Pause / un-pause game, show pauseMenu
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.TogglePlay();
        }


        // Check to see if Scientist has died
        if (GameMaster.isDead)
        {
            if (!deathStatusKnown)
            {
                // Stop calling this
                deathStatusKnown = true;
                // Restrict the player movement and force the player to restart (?)
                //player.GetComponent<Controller2D>().companionDied();
                companionDied();
            }
            if (Time.timeScale == 0)
            {
                GameMaster.deactivatePlayerPanel = true;
            }
            else
            {
                GameMaster.deactivatePlayerPanel = false;
                restartCanvas.gameObject.SetActive(true);
            }
        
        }
        // If we clear the level, carry out needed processes
        if (GameMaster.LevelCleared)
        {
            if (spacePressed && GameMaster.currentLevel != 3)
            {
                endTimer -= Time.deltaTime;
            }
            if (!levelClearedStatusKnown)
            {
                endPipe.gameObject.GetComponent<CircleCollider2D>().enabled = true;
                levelClearedStatusKnown = true;
                //player.GetComponent<Controller2D>().VictoryTextDisplay();
                StopAllAudioSource();
                VictoryTextDisplay();
                Camera.main.GetComponent<CameraMovement>().scientist = Camera.main.GetComponent<CameraMovement>().player;
            }
            // Load Main menu when player presses space
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // this fixes a very unique edge case
                if (GameMaster.currentLevel != 3 || GameMaster.currentLevel != 4)
                {
                    Destroy(soundManager.gameObject);
                }
                player.gameObject.GetComponent<Controller2D>().endLevel = true;
                spacePressed = true;
                
            }
            if (endTimer <= 0 && spacePressed)
            {
                GameMaster.LoadNextLevel();
            }

        }

    }

    // a method to compare the postions of  the player
    // so that the Scientist "knows" it can move
    bool PlayerHasMoved()
    {
        //if (Mathf.Round(playerX) != Mathf.Round(player.transform.position.x))
        //{
        //    return true;
        //}
        //return false;

        // alternative: doesnt work
        // return Mathf.Approximately(playerX, player.transform.position.x);

        // Alternative 2: This is far better, keeping the others incase
        // i can reuse the code elswhere
        float difference = Mathf.Max(playerX, player.transform.position.x)
            - Mathf.Min(playerX, player.transform.position.x);
        return (Mathf.Abs(difference) > 0.01);
    }
    // If the player failed to keep the scientist alive,
    // Restrict the players movement and give a visual cue to restart (?)
    // Mayb
    public void companionDied()
    {

        // Display a helper text to show player they should restart
        //playerText.gameObject.SetActive(true);

        restartCanvas.gameObject.SetActive(true);


    }

    // A method that displays a congratulatory message for the player upon clearing a level
    public void VictoryTextDisplay()
    {

        // Change the player Canvas text
        endGamePanel.gameObject.SetActive(true);
        endGamePanel.UpdateText();

        


    }
    private void StopAllAudioSource()
    {
        Sound[] temp = FindObjectsOfType<Sound>();
        foreach(Sound sc in temp)
        {

            sc.stopLoopMusic();
        }
        if (soundManager == null)
        {
            soundManager = GameObject.Find("SoundManager").GetComponent<Sound>();
        }
        soundManager.winningMusic.Play();
    }

    public void setControls()
    {
        player.GetComponent<Controller2D>().SetControls();
    }
}




