
using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * Team Gecko 
 * COSC360 2020
 * 
 * A static class used to keep track of global variables and load needed scenes
 * 
 */
public class GameMaster : MonoBehaviour
{
    // enum that refers to horizontal movement scheme
    public enum MovementEnum { ArrowKeys, WASD };
    public static MovementEnum movementControls;
    // enum that refers to the jump and charge scheme
    public enum ChargeEnum { Standard, Classic };
    public static ChargeEnum chargeControls;
    // Int for keeping track of player restarts
    // Will need one for each level? maybe an array?
    public static int restarts = 0;
    public static int totalRestarts = 0;
    public static int collectablesCollected = 0;
    public static int prevControlIndx;
    public static int prevChargeIndx;
    // Location of the last checkpoint
    public static Vector3 currentCheckpoint;
    public static int currentLevel = 0;
    // Used by the LevelMaster to check if the level has been cleared
    public static bool LevelCleared;
    // Has the player completed the game?
    public static bool gameComplete = false;
    public static Vector3 globalfirst = Vector3.zero;
    public static Vector3 globalSecond = Vector3.zero;
    public static float volume = 1;
    // Boolean that states if the player currently has a charge
    // Used for checkpoint restarts
    public static bool playerCharge;
    public static bool deactivatePlayerPanel = false;
    public static bool deactivateHUD = false;
    // is the scientist dead?
    public static bool isDead;
    public static bool fromLevelSelect;
    public static bool scientistSpeedUp;
    // The public variables to set the players controls
    // Set to the default values
    public static KeyCode jump = KeyCode.UpArrow;
    public static KeyCode chargeKey = KeyCode.Space;
    public static KeyCode left = KeyCode.LeftArrow;
    public static KeyCode right = KeyCode.RightArrow;
    public static int hintCounter = 0;
    public static int checkPointHintCounter;
    public static bool reloaded, fromNextLevel;
    public static bool finalMindSwap;
    // Method for loading various scenes when called upon
    // toggle comments below when scenes are created
    public static string[] dialogue;

    public static bool[] levelsAvailable = { true, false, false, false, true };
    public static void LoadScene(string sceneName)
    {

        switch (sceneName)
        {
            case "Prologue":
                currentLevel = 0;
                QCurrentLevelHints();
                SceneManager.LoadScene("Prologue");
                break;
            case "Level1":
                currentLevel = 1;
                QCurrentLevelHints();
                SceneManager.LoadScene("TutorialLevel");
                break;
            case "Level2":
                currentLevel = 2;
                QCurrentLevelHints();
                fromNextLevel = true;
                SceneManager.LoadScene("MediumLevel");
                break;
            case "Level3":
                currentLevel = 3;
                QCurrentLevelHints();
                fromNextLevel = true;
                SceneManager.LoadScene("FinalLevel");
                break;
            case "Bonus":
                currentLevel = 4;
                QCurrentLevelHints();
                SceneManager.LoadScene("Bonus");
                break;
            case "MainMenu":
                ResetLevelVariables();
                totalRestarts = 0;
                SceneManager.LoadScene("MainMenu");
                break;
            case "Options":
                SceneManager.LoadScene("Options");
                break;

            default:
                break;

        }
    }

    // method to reload the current scene
    public static void RestartLevel()
    {
        // increment restarts
        restarts++;
        // Get this  a reference to this scene and reload it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // Method that reloads the current level for those chasing the collectables
    public static void ReLoadLevel()
    {
        ResetLevelVariables();
        // Get this  a reference to this scene and reload it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }



    /**
     * A method to change controls, should be accessed only by
     * Pause menu options and mainMenu options
     *
     * PARAMS: movementScheme: changes the movement key bindings
     *                         OPTIONS ARE: WASD, ArrowKeys (by default as well)
     *          chargeScheme: changes the jump and charge buttons
     *                         OPTIONS ARE: Classic, standard (by default as well)
     */
    public static void ChangeControlsKeyboard(string chargeScheme)
    {


        switch (chargeScheme)
        {
            case "Classic":

                jump = KeyCode.Space;
                chargeKey = KeyCode.UpArrow;
                break;

            case "Standard":
                jump = KeyCode.UpArrow;
                chargeKey = KeyCode.Space;

                break;
            // set to standard if wrong input given and let Dev know
            default:
                jump = KeyCode.UpArrow;
                chargeKey = KeyCode.Space;
                left = KeyCode.LeftArrow;
                right = KeyCode.RightArrow;
                break;

        }
        //GameObject.Find("PlayerNew").GetComponent<Controller2D>().SetControls();

    }


    // Method to switch to controller, when we get that hooked up. Use the same
    // KeyCode variables in this class
    public static void ChangeControlsController()
    {

    }

    // A method to load the next level
    public static void LoadNextLevel()
    {
        if (currentLevel == 0)
        {

            currentLevel++;
            ResetLevelVariables();
            LoadScene("Level1");
        }
        else if (currentLevel == 1)
        {
            levelsAvailable[currentLevel] = true;
            currentLevel++;

            ResetLevelVariables();
            LoadScene("Level2");
        }
        else if (currentLevel == 2)
        {
            levelsAvailable[currentLevel] = true;
            currentLevel++;

            ResetLevelVariables();
            LoadScene("Level3");
        }
        else if (currentLevel == 3)
        {
            levelsAvailable[currentLevel] = true;
            currentLevel++;

            LoadScene("MainMenu");
        }
        else if (currentLevel == 4)
        {
            LoadScene("MainMenu");
        }
    }

    static void ResetLevelVariables()
    {
        // Reset level cleared bool
        finalMindSwap = false;
        LevelCleared = false;
        // Reset restart count
        totalRestarts += restarts;
        restarts = 0;
        // Reset checkpoint load position to Zero
        // Used to see if player was at a checkpoint
        currentCheckpoint = Vector3.zero;
        // Reset the locations of the backgrounds
        globalfirst = Vector3.zero;
        globalSecond = Vector3.zero;
    }

    public static void SetVolume(float newVolume)
    {
        volume = newVolume;
    }

    // A method for loading all the required text / hints needed for each level
    static void QCurrentLevelHints()
    {

        switch (currentLevel)
        {
            case 0:
                dialogue = new string[10];
                dialogue[0] = ("Ah, another day in paradise. Todays the big day Volts! just one more adjustment to the MindSwapO'Matic Machine!");
                dialogue[1] = ("Finally they'll all believe that I can transfer the mind of one being into another! No longer will I be the Mad hermit scientist of DuneVille!");
                dialogue[2] = ("Ah my favourite Genetically engineered electric Gecko-dile. You know you were created to help me round the lab not chase flies all day, Sparks...");
                dialogue[3] = ("Hmmm... Best I stay out of here. These traps are too dangerous to ever see the light of day. Got my moustache trimmed last I was in here..");
                dialogue[4] = ("Right, off to work! I need to inspect the machine... (Press " + chargeKey.ToString("G") + "  to interact)");
                dialogue[5] = ("Ahhh... Sparks? Hey, open the door, mate... WOOOOAAAAH");
                dialogue[6] = ("Well, that didn't sound good... And my tail itches.. wait a second");
                dialogue[7] = ("Machines stuffed and I'm trapped in this body! Its gonna take me ages to fix with these paws..."); // temp
                dialogue[8] = ("Woah, not down there, Sparks! He's gonna waste my old body on everytrap in there!");
                dialogue[9] = ("That Vent up there could get me under the floor where I can disable the traps in his path...");
                break;
            case 1:
                dialogue = new string[6];
                dialogue[0] = ("Hmmm... I could disable that PURPLE Electric Fence above with this PURPLE PYLON ");
                dialogue[1] = ("You know Sparks, these doors were to keep you away from the Pylons, not slow me down!!");
                dialogue[2] = ("How convenient I left these Moving Platforms down here.");
                dialogue[3] = ("I wonder if those failed Smart Mobile Robotic Traps are down here...");
                dialogue[4] = ("Another S.M.R.T Bot!! Why did I even make these things??");
                dialogue[5] = ("Ah my old Gravity Fields... Fun, but they never mixed well with Whiskey...");
                break;
            case 2:
                dialogue = new string[2];
                dialogue[0] = ("You know Sparks, Although I can't see myself living on a diet of flies.. I am enjoying running around in your body. We'll get back to normal pal.");

                break;
            case 3:
                dialogue = new string[4];
                dialogue[0] = ("I'm sure I have a back-up MindSwapO'Matic down here somewhere... Pretty convenient we are headed in that direction, Sparks.");
                dialogue[1] = ("There it is! Time to get my moustache back! Damn I'm going to miss how electrifying this is...");
                dialogue[2] = ("You know what Sparks, I really enjoy this body. I think I might keep it.");
                break;
            case 4:
                dialogue = new string[3];
                dialogue[0] = ("Hmmm... perhaps I could lead my old body to the gecko cage... that will keep him safe!");
                dialogue[1] = ("This way Sparks, we'll get you back in your cage!");
                dialogue[2] = ("Perfect, now I just need to send him down the chute!");
                break;
            default:

                break;
        }
    }
}
