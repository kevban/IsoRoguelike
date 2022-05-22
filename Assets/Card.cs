using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public new string name;
    string description;
    public bool build = true; //true if meant to be built through left click
    public bool cardSelect = false; // true if meant to be added to deck through reward
    
    
    public Canvas canvas;
    public GameManager gameManager;
    public GridManager gridManager;
    public Tilemap buildingTiles;
    
    
    public BoardTile boardTile = new BoardTile();

    bool up = false; //used for card hover animation
    Vector2 originalPos; //used to return card
    CanvasGroup canvasGroup;
    Camera cam;



    void Awake()
    {
        buildingTiles = FindObjectOfType<Tilemap>();
        gameManager = FindObjectOfType<GameManager>();
        gridManager = FindObjectOfType<GridManager>();
        canvas = gameManager.inventoryCanvas.GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        cam = Camera.main;
    }


    /// <summary>
    /// creating a brand new Card from tile library given name
    /// </summary>
    /// <param name="name"></param>
    public void Init(string name) {
        /*
         * 0 - house
         * 1 - pizza
         * 2 - burger
         * 3 - grocery
         * 4 - movie
         * 5 - restaurant
         * 6 - office
         * 7 - appartment
         * 8 - hotel
         * 9 - park
         * 10 - wonderland
         * 11 - forest
         * 12 - wood
         * 13 - lodge
         * 14 - mountain
         */
        
        this.name = name;
        this.boardTile = gameManager.allTiles[gameManager.CardStringToIndex(name)];


        //child
        // 0 = background
        // 1 = sprite
        // 2 = border
        // 3 = name
        // 4 = descript
        // 5 = base amount
        // 6 = star
        transform.GetChild(1).GetComponent<Image>().sprite = boardTile.leftSprite;
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = boardTile.displayName;
        transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = boardTile.description;
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "+$" + boardTile.baseAmount;
    }

    /// <summary>
    /// Create an instance of Card using an existing BoardTile
    /// </summary>
    public void InitFromBoardTile(BoardTile boardTile) { 
        this.boardTile = boardTile;
        this.name = boardTile.name;
        //child
        // 0 = background
        // 1 = sprite
        // 2 = border
        // 3 = name
        // 4 = descript
        // 5 = base amount
        // 6 = star
        transform.GetChild(1).GetComponent<Image>().sprite = boardTile.leftSprite;
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = boardTile.displayName;
        transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = boardTile.description;
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "+$" + boardTile.baseAmount;
    }

    //This function is called when player press on the card. If from reward select screen, add the card to deck. If not (ie from deck), build the building in the tile.
    //If from inventory, the card is selected
    public void Build() {
        if (build)
        {
            gameManager.selectedTile = boardTile.CreateTile(true);
            gameManager.SelectCardFromInv(this);
            gridManager.isBuilding = true;
            /*
            int district;
            bool faceLeft = true;
            Vector3Int posToBuild;
            if (gridManager.playerPos >= 1 && gridManager.playerPos <= 6)
            {
                district = 0;
                posToBuild = buildingTiles.WorldToCell(new Vector3(gridManager.waypoints[gridManager.playerPos].transform.position.x + 0.5f,
                                                                               gridManager.waypoints[gridManager.playerPos].transform.position.y + 0.25f, 0));
                faceLeft = true;
            }
            else if (gridManager.playerPos >= 8 && gridManager.playerPos <= 13)
            {
                district = 1;
                posToBuild = buildingTiles.WorldToCell(new Vector3(gridManager.waypoints[gridManager.playerPos].transform.position.x + 0.5f,
                                                                                   gridManager.waypoints[gridManager.playerPos].transform.position.y - 0.25f, 0));
                faceLeft = false;
            }
            else if (gridManager.playerPos >= 15 && gridManager.playerPos <= 20)
            {
                district = 2;
                posToBuild = buildingTiles.WorldToCell(new Vector3(gridManager.waypoints[gridManager.playerPos].transform.position.x - 0.5f,
                                                                                   gridManager.waypoints[gridManager.playerPos].transform.position.y - 0.25f, 0));
                faceLeft = true;
            }
            else
            {
                district = 3;
                posToBuild = buildingTiles.WorldToCell(new Vector3(gridManager.waypoints[gridManager.playerPos].transform.position.x - 0.5f,
                                                                                   gridManager.waypoints[gridManager.playerPos].transform.position.y + 0.25f, 0));
                faceLeft = false;
            }
            switch (name)
            {
                case "house":

                    break;
                case "piiza":

                    break;
                default:
                    break;
            }*/
        }
        else if (cardSelect) {
            gameManager.AddCard(name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !gridManager.showingStat) {
            gridManager.effectDisplay.ShowEffect(boardTile);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        canvasGroup.alpha = 0.7f;
        
        canvasGroup.blocksRaycasts = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);
        transform.GetChild(6).gameObject.SetActive(false);
        originalPos = transform.position;

        gameManager.selectedTile = boardTile.CreateTile(true);
        gameManager.SelectCardFromInv(this);
        gridManager.isBuilding = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(5).gameObject.SetActive(true);
        transform.GetChild(6).gameObject.SetActive(true);
        if (gridManager.TempBuild())
        {
            gridManager.Build();
        }
        else {
            transform.position = originalPos;
        };
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        Vector2 dragOffset = new Vector2(40,40);
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, eventData.position, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position + dragOffset);
        if (gridManager.TempBuild()) { 
            transform.GetChild(1).gameObject.SetActive(true);
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (build && !up) {
            print("entered");
            gameObject.GetComponent<Animator>().Play("CardSelectFromInv");
            up = true;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (build && eventData.fullyExited && up)
        {
            print("exitted");
            gameObject.GetComponent<Animator>().Play("CardSelectFromInvReverse");
            up = false;
        }
        
    }
}
