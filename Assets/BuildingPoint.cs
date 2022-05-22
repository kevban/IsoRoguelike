using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BuildingPoint : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    public BoardTile tile;
    public GameObject cashPopup;
    public GameObject resourceText;
    public int endOfTurnIncome = 0;

    private void Start()
    {
        UpdateResourceText();
    }
    public void CashPop(int earnings) {
        cashPopup.GetComponent<TextMeshPro>().text = "+$" + earnings;
        cashPopup.GetComponent<Animator>().Play("CashPopupANIMATION");
    }

    public void CashPopEOT() {
        cashPopup.GetComponent<TextMeshPro>().color = Color.yellow;
        cashPopup.GetComponent<TextMeshPro>().fontSize = 3;
        cashPopup.GetComponent<TextMeshPro>().text = "+$" + endOfTurnIncome;
        cashPopup.GetComponent<Animator>().Play("CashPopupANIMATION");
        
    }

    public void ResourceDepletionEOT() {
        cashPopup.GetComponent<TextMeshPro>().color = Color.blue;
        cashPopup.GetComponent<TextMeshPro>().fontSize = 2;
        UpdateResourceText();
        cashPopup.GetComponent<TextMeshPro>().text = "-1\nResource";
        cashPopup.GetComponent<Animator>().Play("CashPopupANIMATION");
    }
    void UpdateResourceText() {
        
        if (tile.resource <= 0)
        {
            resourceText.SetActive(false);
        }
        else {
            resourceText.GetComponent<TextMeshPro>().text = tile.resource.ToString();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
        if (eventData.pointerClick != null) {
            //eventData.pointerClick.GetComponent<>
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
