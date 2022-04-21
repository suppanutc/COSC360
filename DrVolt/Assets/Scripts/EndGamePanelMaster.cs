using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanelMaster : MonoBehaviour
{
    [SerializeField]
    Text levelClear;
    [SerializeField]
    Text restartText;
    [SerializeField]
    Text niceWorkText;
    [SerializeField]
    Text collectableText;

    public void UpdateText()
    {
        levelClear.text = "LEVEL " + GameMaster.currentLevel + " CLEARED!";
        restartText.text = "YOU HAD " + GameMaster.restarts + " RESTARTS";
        if (GameMaster.restarts == 0)
        {
            niceWorkText.text = "PERFECT PLAYTHROUGH";
        }
        else if (GameMaster.restarts < 6)
        {
            niceWorkText.text = "EXCELLENT RUN";
        }
        else if (GameMaster.restarts < 21)
        {
            niceWorkText.text = "NOT TOO BAD AT ALL";
        }
        else if (GameMaster.restarts < 61)
        {
            niceWorkText.text = "ALWAYS ROOM FOR IMPROVEMENT!";
        }
        else if (GameMaster.restarts < 120)
        {
            niceWorkText.text = "THAT MANY? WOW...";
        }
        else if (GameMaster.restarts < 200 && GameMaster.currentLevel != 4)
        {
            niceWorkText.text = "GOOD LUCK ON THE NEXT LEVEL...";
        }
        else if (GameMaster.restarts < 200)
        {
            niceWorkText.text = "GIT GUD";
        }
        collectableText.text = "You have " + GameMaster.collectablesCollected + " out of 4! collectables";

    }
}
