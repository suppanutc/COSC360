using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    private void Update()
    {
        if (GameMaster.deactivatePlayerPanel) //GameMaster.deactivateHUD ||)
        {
            this.gameObject.SetActive(GameMaster.deactivateHUD);
        }
    }
}
