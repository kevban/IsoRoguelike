using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cash : MonoBehaviour
{
    public GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void UpdateCash(int amount) {
        gameManager.cash += amount;
        gameObject.GetComponent<TextMeshProUGUI>().text = "Cash: $" + gameManager.cash.ToString();
    }
}
