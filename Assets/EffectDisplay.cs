using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDisplay : MonoBehaviour
{
    BoardTile boardTile;
    public GameObject effectPrefab;
    public GameObject cardDetailsCanvas;
    public Card card;
    public GridManager gridManager;
    public GameManager gameManager;
    public void ShowEffect(BoardTile boardTile) { 
        cardDetailsCanvas.SetActive(true);
        gridManager.showingStat = true;
        this.boardTile = boardTile;
        List<Effect> effects = new List<Effect>();
        boardTile.GenerateDescription();
        card.build = false;
        card.cardSelect = false;
        card.InitFromBoardTile(boardTile);
        string title;
        string description;
        if (boardTile.effectList.Contains("Population")) {
            GameObject effect = GameObject.Instantiate(effectPrefab);
            title = "Population";
            description = "Every 4 <b><color=black>Population</color></b> earns $1 at the end of a turn";
            effect.GetComponent<Effect>().Init(title, description);
            effect.transform.SetParent(transform, false);
            if (boardTile.population > 0)
            {
                for (int i = 0; i < boardTile.groceries; i++)
                {
                    GameObject effect2 = GameObject.Instantiate(effectPrefab);
                    title = "Grocery";
                    description = "This building's <b><color=black>Population</color></b> is doubled by Grocery";
                    effect2.GetComponent<Effect>().Init(title, description);
                    effect2.transform.SetParent(transform, false);
                }
            }

        }
        if (boardTile.effectList.Contains("Commerce"))
        {
            GameObject effect = GameObject.Instantiate(effectPrefab);
            title = "Commerce";
            description = "When you land on a <b><color=black>Commerce</color></b> building, you earn $1 for each <b><color=black>Commerce</color></b> on this <b><color=black>Street</color></b>";
            effect.GetComponent<Effect>().Init(title, description);
            effect.transform.SetParent(transform, false);
            
        }
        if (boardTile.effectList.Contains("Tourist"))
        {
            GameObject effect = GameObject.Instantiate(effectPrefab);
            title = "Tourist";
            description = "Every <b><color=black>Tourist</color></b> will earn $1 for each <b><color=black>Beauty</color></b> in the map at the end of a turn";
            effect.GetComponent<Effect>().Init(title, description);
            effect.transform.SetParent(transform, false);
        }
        if (boardTile.effectList.Contains("Beauty"))
        {
            GameObject effect = GameObject.Instantiate(effectPrefab);
            title = "Beauty";
            description = "Each <b><color=black>Beauty</color></b> increases tourism income by $1";
            effect.GetComponent<Effect>().Init(title, description);
            effect.transform.SetParent(transform, false);
        }
        if (boardTile.effectList.Contains("Industry"))
        {
            if (gameManager.incomePerIndustry == 2)
            {
                GameObject effect = GameObject.Instantiate(effectPrefab);
                title = "Industry";
                description = "Each Industry consumes 1 adjacent <b><color=black>Resource</color></b> and earns $" + gameManager.incomePerIndustry + " per <b><color=black>Resource</color></b> consumed.";
                effect.GetComponent<Effect>().Init(title, description);
                effect.transform.SetParent(transform, false);
            }
            else {
                GameObject effect = GameObject.Instantiate(effectPrefab);
                title = "Industry";
                description = "Each Industry consumes 1 adjacent <b><color=black>Resource</color></b> and earns $*" + gameManager.incomePerIndustry + "* per <b><color=black>Resource</color></b> consumed.";
                effect.GetComponent<Effect>().Init(title, description);
                effect.transform.SetParent(transform, false);
            }
        }
        if (boardTile.effectList.Contains("Resource"))
        {
            GameObject effect = GameObject.Instantiate(effectPrefab);
            title = "Resource";
            description = "When <b><color=black>Resource</color></b> reaches 0, building is destroyed";
            effect.GetComponent<Effect>().Init(title, description);
            effect.transform.SetParent(transform, false);
        }
        if (boardTile.effectList.Contains("Street")) {
            GameObject effect = GameObject.Instantiate(effectPrefab);
            title = "Street";
            description = "Tiles are on the same <b><color=black>Street</color></b> if they are connected in a line";
            effect.GetComponent<Effect>().Init(title, description);
            effect.transform.SetParent(transform, false);
        }
        if (boardTile.effectList.Contains("Burger"))
        {
            GameObject effect = GameObject.Instantiate(effectPrefab);
            title = "Burger";
            description = "There are " + gameManager.burgNum + " burger shops on the map";
            effect.GetComponent<Effect>().Init(title, description);
            effect.transform.SetParent(transform, false);

        }

    }
    public void HideEffect() {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        gridManager.showingStat = false;
        cardDetailsCanvas.SetActive(false);
    }
}
