using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


//this controls all game states, scores, mechanics, player deck
public class GameManager : MonoBehaviour
{
    public GameObject cardHolder;

    [HideInInspector] 
    public int nextCard = 0;
    public Sprite[] buildingSprites;

    public List<BoardTile>  tiles = new List<BoardTile>(); //contains the base version of buildable tiles
    public List<BoardTile> allTiles = new List<BoardTile>(); //contains the base version of all tiles
    public GameObject cardPrefab;
    public GameObject cardSelectCanvas;
    public GameObject cardSelectHolder;
    public GridManager gridManager;
    public GameObject inventoryCanvas;
    public Level levelText;
    public AudioManager audioManager;


    [HideInInspector]
    public List<Card> playerDeck = new List<Card>();

    public bool pickingCard = false;
    public Tile selectedTile;
    public Card selectedCard;

    //game state variables
    public int cash = 0; // the amount of cash
    public int income = 0; // amount of cash @ end of round
    public int level;
    public int turnCounter = 0;
    public int[] levelRequirements = { 5, 25, 50, 100, 300, 520, 1314 };

    public int burgNum = 0;
    public int[] districtPop = {0, 0, 0, 0, 0 };
    public int[] districtCom = { 0, 0, 0, 0, 0 };
    public int[] districtGroceries = { 0, 0, 0, 0, 0 }; //keep track of amount of groceries in each of the 5 district
    public int worldPop = 0;// this is the world population i.e. sum of all population on board
    public int worldCom = 0;
    public int worldBeauty = 0;
    public int worldTourist = 0;
    public int incomePerIndustry = 2;

    public static int ceo = 0;

    //4 districts, starting @ 0 = bottom left
    //within each district: 0 = population, 1 = commerce
    public List<List<int>> district; 

    void Awake()
    {
        loadAllTiles();
        LoadPlayer();
        //audioManager.Shuffle();
    }

    void LoadPlayer() {
        switch (ceo) {
            default:
                break;
                
        }
    }



    public void Recalc() {
        for (int i = 0; i < districtPop.Length; i++) {
            districtPop[i] = 0;
        }
        for (int i = 0; i < districtCom.Length; i++)
        {
            districtCom[i] = 0;
        }
        for (int i = 0; i < districtCom.Length; i++)
        {
            districtGroceries[i] = 0;
        }
        worldPop = 0;
        worldCom = 0;
        worldBeauty = 0;
        worldTourist = 0;
    }

    public void PickCards() { 
        cardSelectCanvas.SetActive(true);
        int cardsGenerated = 0;
        List<int> cardsToShow = new List<int> ();
        while (cardsGenerated < 3) {
            int randomNum = Random.Range(0, tiles.Count);
            if (!cardsToShow.Contains(randomNum)) {
                cardsGenerated++;
                cardsToShow.Add(randomNum);
            }
        }
        for (int i = 0; i < 3; i++) {
            GameObject cardToAdd = Instantiate(cardPrefab);
            cardToAdd.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            cardToAdd.transform.SetParent(cardSelectHolder.transform, false);
            cardToAdd.GetComponent<Card>().Init(tiles[cardsToShow[i]].name);
            cardToAdd.GetComponent<Card>().build = false;
            cardToAdd.GetComponent<Card>().cardSelect = true;
        }
        
    }

    //transforms the index of the card to card name
    public string CardIndexToString(int index) {
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

        switch (index) {
            case 0: return "house";
            case 1: return "pizza";
            case 2: return "burger";
            case 3: return "grocery";
            case 4: return "movie";
            case 5: return "restaurant";
            case 6: return "office";
            case 7: return "apartment";
            case 8: return "hotel";
            case 9: return "park";
            case 10: return "wonderland";
            case 11: return "forest";
            case 12: return "wood";
            case 13: return "lodge";
            case 14: return "mountain";
            default: return "";
        }
    }

    public int CardStringToIndex(string name) {
        switch (name)
        {
            case "house": return 0;
            case "pizza": return 1;
            case "burger": return 2;
            case "grocery": return 3;
            case "movie": return 4;
            case "restaurant": return 5;
            case "office": return 6;
            case "apartment": return 7;
            case "hotel": return 8;
            case "park": return 9;
            case "wonderland": return 10;
            case "forest": return 11;
            case "wood": return 12;
            case "lodge": return 13;
            case "mountain": return 14;
            default:
                print (name);
                return -1;
        }
    }

    //this function create and add a card with cardName to player hand
    public void AddCard(string cardName) {
        if (cardName == "skip")
        {
            pickingCard = false;
            foreach (Transform child in cardSelectHolder.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            gridManager.highlightTiles();
            cardSelectCanvas.SetActive(false);
            inventoryCanvas.SetActive(true);
        }
        else {
            GameObject cardToAdd = Instantiate(cardPrefab);
            cardToAdd.GetComponent<Card>().Init(cardName);
            playerDeck.Add(cardToAdd.GetComponent<Card>());
            cardToAdd.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            cardToAdd.transform.SetParent(cardHolder.transform, false);
            pickingCard = false;
            foreach (Transform child in cardSelectHolder.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            gridManager.highlightTiles();
            cardSelectCanvas.SetActive(false);
            inventoryCanvas.SetActive(true);
        }

    }

    void loadAllTiles() {
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
        buildingSprites = Resources.LoadAll<Sprite>("BuildingTiles1");
            for (int index = 0; index < 15; index++)
            {
                BoardTile tile = new BoardTile();
                switch (index)
                {
                    case 0:
                        tile.name = "house";
                        tile.baseAmount = 10;
                        tile.population = 4;
                        break;
                    case 1:
                        tile.name = "pizza";
                        tile.baseAmount = 10;
                        tile.commerce = 2;
                        break;
                    case 2:
                        tile.name = "burger";
                        tile.baseAmount = 5;
                        break;
                    case 3:
                        tile.name = "grocery";
                        break;
                    case 4:
                        tile.name = "movie";
                        break;
                    case 5:
                        tile.name = "restaurant";
                        break;
                    case 6:
                        tile.name = "office";
                        break;
                    case 7:
                        tile.name = "apartment";
                        tile.population = 12;
                        break;
                    case 8:
                        tile.name = "hotel";
                        tile.baseAmount = 4;
                        tile.population = 4;
                        tile.commerce = 2;
                        tile.tourist = 2;
                        break;
                    case 9:
                        tile.name = "park";
                        tile.beauty = 2;
                        break;
                    case 10:
                        tile.name = "wonderland";
                        break;
                    case 11:
                        tile.name = "forest";
                        tile.beauty = 2;
                        tile.resource = 2;
                        tile.baseAmount = 2;
                        break;
                    case 12:
                        tile.name = "wood";
                        tile.baseAmount = 2;
                        tile.resource = 10;
                    tile.buildable = false;
                        break;
                    case 13:
                        tile.name = "lodge";
                        tile.baseAmount = 10;
                        tile.industry = 2;
                        break;
                    case 14:
                        tile.name = "mountain";
                    tile.infiniteResource = true;
                        break;
                    default:
                        break;
                }
            tile.Init();
            tile.leftSprite = buildingSprites[index * 2];
            tile.rightSprite = buildingSprites[index * 2 + 1];
            if (tile.buildable) { 
                tiles.Add(tile);
            }
            allTiles.Add(tile);
            }
        }
        


}
