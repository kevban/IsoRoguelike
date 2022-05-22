using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Effect : MonoBehaviour
{
    public GameObject title;
    public GameObject description;
    public void Init(string title, string description) { 
        this.title.GetComponent<TextMeshProUGUI>().text = title;
        this.description.GetComponent<TextMeshProUGUI>().text = description;
    }
}
