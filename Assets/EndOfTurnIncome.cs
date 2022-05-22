using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndOfTurnIncome : MonoBehaviour
{

    GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void UpdateIncome(int population, int industry, int tourism) {
        gameManager.income = population + + industry + tourism;
        GetComponent<TextMeshProUGUI>().text = "Income: $" + gameManager.income + "\n From population: $" + population + "\n From industry: $" + industry + "\n From tourism: $" + tourism;
    }
}
