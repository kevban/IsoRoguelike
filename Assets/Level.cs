using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject gameOverCanvas;
    public GameObject hbdCanvas;
    // Start is called before the first frame update
    public void UpdateLevel() { 
        gameManager.turnCounter++;
        if (gameManager.turnCounter >= 10)
        {
            gameManager.turnCounter = 0;
            if (gameManager.cash < gameManager.levelRequirements[gameManager.level])
            {
                gameOverCanvas.SetActive(true);
            }
            else
            {
                gameManager.level++;
                if (gameManager.level >= gameManager.levelRequirements.Length)
                {
                    hbdCanvas.SetActive(true);
                    gameManager.audioManager.StopAll();
                }
                else
                {
                    GetComponent<TextMeshProUGUI>().text = "$" + gameManager.levelRequirements[gameManager.level] + " Cash needed in <color=red>" + (10 - gameManager.turnCounter) + "</color> turns";
                }
            }
            
        }
        else {
            GetComponent<TextMeshProUGUI>().text = "$" + gameManager.levelRequirements[gameManager.level] + " Cash needed in <color=red>" + (10 - gameManager.turnCounter) + "</color> turns";

        }
    }
}
