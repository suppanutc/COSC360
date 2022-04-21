using UnityEngine;
using UnityEngine.UI;


public class Prologuelevelmaster : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particleEffects;
    // Reference to player, Companion and pauseMenu
    public Transform player;
    public Transform player2;
    public Transform scientist;
    public GeckoNPC geckoNPC;
    public PauseMenuManager pauseMenu;
    public GameObject playerPanel;
    [SerializeField]
    SpriteRenderer open;
    [SerializeField]
    SpriteRenderer closed;
    [SerializeField]
    SpriteRenderer closedBroken;
    [SerializeField]
    SpriteRenderer openBroken;
    [SerializeField]
    DialogueTrigger dTrig1;
    [SerializeField]
    DialogueTrigger dTrig2;
    [SerializeField]
    DialogueTrigger dTrig3;
    [SerializeField]
    DialogueTrigger dTrig4;
    [SerializeField]
    DialogueTrigger dTrig5;
    bool partOne;
    bool partTwo;
    public bool startSequence;
    bool midSequence;
    bool endSequence;
    Vector3 geckoSwap = new Vector3(38f, -3.8f, 0);
    // Booleans so we dont keep calling the same external methods
    bool playerInMotion;
    bool deathStatusKnown;
    bool levelClearedStatusKnown;
    [SerializeField]
    CircleCollider2D activationCircle1;
    [SerializeField]
    CircleCollider2D activationCircle2;
    float timer = 1f;
    [SerializeField]
    AudioClip electricity;
    [SerializeField]
    AudioClip levelMusic;
    
    // the players starting x position used for setting the scientist in motion
    // as an alterantive to a timer
    float playerX;
    // Start is called before the first frame update
    void Start()
    {
        
        //playerPanel = player.gameObject.GetComponentInChildren<PlayerPanel>().gameObject;
        // Initialize volume based on changes in main menu, if there were any
        AudioListener.volume = GameMaster.volume;
        // Ensure we let the GameMaster knows the level has not yet been cleared
        GameMaster.LevelCleared = false;
        // Disable pause screen if active
        pauseMenu.gameObject.SetActive(false);
        // Ensure the game runs on start i.e we havent entered main
        // menu from a pause screen
        Time.timeScale = 1f;
        particleEffects.Stop();
        // If te player has restarted from a checkpoint, reload from there

        if (GameMaster.currentCheckpoint != Vector3.zero)
        {

            player.position = GameMaster.currentCheckpoint;
            scientist.position = new Vector3(player.position.x, scientist.position.y,
                scientist.position.z);


        }
        else // reset playercharge to default
        {
            GameMaster.playerCharge = false;
            GameMaster.hintCounter = 0;
        }
        playerX = player.transform.position.x;





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





        // Check to see if Scientist has died
        if (GameMaster.isDead)
        {
            if (!deathStatusKnown)
            {
                // Stop calling this
                deathStatusKnown = true;
                // Restrict the player movement and force the player to restart (?)
                //player.GetComponent<Controller2D>().companionDied();
            }
            if (Time.timeScale == 0)
            {
                GameMaster.deactivatePlayerPanel = true;
            }
            else
            {
                GameMaster.deactivatePlayerPanel = false;
                playerPanel.SetActive(true);
            }

        }
        // If we clear the level, carry out needed processes
        if (GameMaster.LevelCleared)
        {


            if (!levelClearedStatusKnown)
            {
                levelClearedStatusKnown = true;
                //player.GetComponent<Controller2D>().VictoryTextDisplay();
            }
            // Load Main menu when player presses space
            if (Input.GetKeyDown(KeyCode.Space))
            {

                GameMaster.LoadNextLevel();

            }

        }
        // The order of the actions in the cutscene
        if (GameMaster.hintCounter == 4 && !partOne)
        {
            partOne = true;
            activationCircle1.enabled = true;
            dTrig1.gameObject.SetActive(true);
        }
        if (GameMaster.hintCounter == 5 && !partTwo)
        {
            partTwo = true;
            activationCircle2.enabled = true;
            
        }

        if (startSequence)
        {

            geckoNPC.GoToPosition(geckoSwap);
            


                

            if (geckoNPC.transform.position == geckoSwap)
            {
                geckoNPC.StopGecko();
                player.gameObject.GetComponent<AudioSource>().clip = electricity;
                player.gameObject.GetComponent<AudioSource>().Play();

                    dTrig2.gameObject.SetActive(true);
                    open.enabled = false;
                    closed.enabled = true;
                    startSequence = false;
                particleEffects.Play();
                midSequence = true;
                timer = 13f;
                // play transfer sound here. maybe some sparks
            }
        }

        if (midSequence)
        {
            bool one = true;
            bool two = false;
            timer -= Time.deltaTime;
            if (timer <= 8 && one)
            {
                player.gameObject.GetComponent<AudioSource>().clip = levelMusic;
                
                one = false;
                //play shutdown sound here
                closed.enabled = false;
                closedBroken.enabled = true;
                particleEffects.Stop();
                //player.position = new Vector3(geckoNPC.gameObject.transform.position.x, player.position.y, player.position.z);
                //closedBroken.enabled = true;
                dTrig3.gameObject.SetActive(true);
                player.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                player = player2;
                
                
                scientist.gameObject.SetActive(true);
                scientist.gameObject.GetComponent<GravityScientist>().scientistSpeed = 0;
                player2.gameObject.SetActive(true);
                player.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                player.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                geckoNPC.gameObject.SetActive(false);
                Camera.main.GetComponent<CameraMovement>().player = player;

                Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), scientist.GetComponent<Collider2D>(), true);

                two = true;
            }
            if (timer < 4f && two) {
                two = false;
                player.gameObject.GetComponent<AudioSource>().Play();
                midSequence = false;
                player.gameObject.GetComponent<Controller2D>().playerSpeed = 8;
                player.gameObject.GetComponent<Controller2D>().jumpHeight = 15;
                scientist.gameObject.GetComponent<GravityScientist>().scientistSpeed = 2;
                dTrig4.gameObject.SetActive(true);
                openBroken.enabled = true;
                closedBroken.enabled = false;
                endSequence = true;
                timer = 5f;
            } 
        }

        if (endSequence)
        {

                dTrig5.gameObject.SetActive(true);
                endSequence = false;

        }

        if (GameMaster.LevelCleared)
        {

            player.gameObject.SetActive(false);


 
                GameMaster.LoadNextLevel();


        }
        // then wait a bit longer until scene transition into first level.

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
        // i can reuse the code elsewhere
        float difference = Mathf.Max(playerX, player.transform.position.x)
            - Mathf.Min(playerX, player.transform.position.x);
        return (Mathf.Abs(difference) > 0.01);
    }

}
