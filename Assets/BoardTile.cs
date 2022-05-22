using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//
public class BoardTile
{
    //references
    public Tilemap tilemap;

    //basic attributes
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
    int index = 15; //the index num in gameManager.tiles


    //grid attributes
    public bool buildable = true;
    bool walkable = false;
    public Vector3Int location;
    public int district;

    //building attributes
    public string name;
    public string displayName; //used to display
    public string description;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public int baseAmount = 0;

    //current stats
    public int population = 0;
    public int commerce = 0;
    public int tourist = 0;
    public int beauty = 0;
    public int industry = 0;
    public int resource = 0;

    //based stats used exclusively for card description
    public int basepopulation = 0;
    public int basecommerce = 0;
    public int basetourist = 0;
    public int basebeauty = 0;
    public int baseindustry = 0;
    public int baseresource = 0;

    public Tile tile;

    //building effects
    public int groceries = 0;
    public bool infiniteResource = false;

    //keep track of all the effects to show
    public List<string> effectList = new List<string>();

    public void Landed() { 
        
    }

    public void PassBy() { 
    
    }

    public void Init() {
        switch (this.name)
        {

            case "house":
                displayName = "House";
                break;
            case "pizza":
                displayName = "Pizza"; 
                break;
            case "burger":
                displayName = "Burger"; 
                break;
            case "grocery":
                displayName = "Grocery";
                break;
            case "movie":
                displayName = "Movie";
                break;
            case "restaurant":
                displayName = "Restaurant";
                break;
            case "office":
                displayName = "Office";
                break;
            case "apartment":
                displayName = "Apartment";
                break;
            case "hotel":
                displayName = "Hotel";
                break;
            case "park":
                displayName = "Park";
                break;
            case "wonderland":
                displayName = "Wonderland";
                break;
            case "forest":
                displayName = "Forest";
                break;
            case "wood":
                displayName = "Wood";
                break;
            case "lodge":
                displayName = "Lumber Mill";
                break;
            case "mountain":
                displayName = "Mountain";
                break;
            default:
                Debug.Log(this.name);
                break;
        }
        basepopulation = population;
        basecommerce = commerce;
        basetourist = tourist;
        basebeauty = beauty;
        baseindustry = industry;
        baseresource = resource;
        GenerateDescription();
    }

    public Tile CreateTile(bool left) { 
        Tile newTile = new Tile();
        if (left)
        {
            newTile.sprite = leftSprite;
        }
        else { 
            newTile.sprite = rightSprite;
        }
        
        return newTile;
    }
    
    //used to create a new instance (from library) without affecting the original
    public BoardTile Clone() { 
        BoardTile newTile = new BoardTile();
        newTile.name = name;
        newTile.description = description;
        newTile.leftSprite = this.leftSprite;
        newTile.rightSprite = this.rightSprite;
        newTile.baseAmount = baseAmount;
        newTile.population = population;
        newTile.commerce = commerce;
        newTile.tourist = tourist;
        newTile.beauty = beauty;
        newTile.industry = industry;
        newTile.resource = resource;
        newTile.basepopulation = basepopulation;
        newTile.basecommerce = basecommerce;
        newTile.basetourist = basetourist;
        newTile.basebeauty = basebeauty;
        newTile.baseindustry = baseindustry;
        newTile.baseresource = baseresource;
        newTile.infiniteResource = infiniteResource;
        return newTile;
    }

    public void GenerateDescription() {
        description = "";
        effectList.Clear();
        switch (this.name)
        {
            case "house": break;
            case "pizza": break;
            case "burger":
                description = description + "+1 <b><color=black>Commerce</color></b> for each burger on the map \n";
                effectList.Add("Burger");
                break;
            case "grocery": 
                description = description + "Double <b><color=black>Population</color></b> on this street\n";
                effectList.Add("Population");
                break;
            case "movie":
                description = description + "Earn $1 for each <b><color=black>Population</color></b> and <b><color=black>Commerce</color></b>\n";
                effectList.Add("Population");
                effectList.Add("Commerce");
                break;
            case "restaurant":
                description = description + "+1 <b><color=black>Commerce</color></b> each time you land on this tile\n";
                effectList.Add("Commerce");
                effectList.Add("Restaurant");
                break;
            case "office":
                description = description + "+$1 income from <b><color=black>Industry</color></b>\n";
                effectList.Add("Industry");
                break;
            case "apartment": break;
            case "hotel": break;
            case "park":
                description = description + "Earn $2 for each <b><color=black>Population</color></b> on this <b><color=black>Street</color></b> when you land on this tile \n";
                effectList.Add("Population");
                effectList.Add("Street");
                break;
            case "wonderland":
                description = description + "Earn $2 for each <b><color=black>Population</color></b> when you land on this tile\n";
                effectList.Add("Population");
                break;
            case "forest":
                description = description + "Becomes <b><color=black>Wood</color></b> when <b><color=black>Destroyed</color></b>\n";
                effectList.Add("Destroyed");
                break;
            case "wood": break;
            case "lodge":break;
            case "mountain":
                description = description + "Infinite <b><color=black>Resource</color></b>";
                effectList.Add("Resource");
                break;
            default:
                Debug.Log(this.name);
                break;
        }
        if (basepopulation > 0 || population > 0) {
            description = description + StatString(basepopulation, population, "Population");
            effectList.Add("Population");
        }
        if (basecommerce > 0 || commerce > 0) {
            description = description + StatString(basecommerce, commerce, "Commerce");
            effectList.Add("Commerce");
            effectList.Add("Street");
        }
        if (basetourist > 0 || tourist > 0)
        {
            description = description + StatString(basetourist, tourist, "Tourist");
            effectList.Add("Tourist");
            effectList.Add("Beauty");
        }
        if (basebeauty > 0 || beauty > 0)
        {
            description = description + StatString(basebeauty, beauty, "Beauty");
            effectList.Add("Tourist");
            effectList.Add("Beauty");
        }
        if (baseindustry > 0 || industry > 0)
        {
            description = description + StatString(baseindustry, industry, "Industry");
            effectList.Add("Industry");
            effectList.Add("Resource");
        }
        if (baseresource > 0 || resource > 0)
        {
            description = description + StatString(baseresource, resource, "Resource");
            effectList.Add("Resource");
        }
    }

    private string StatString(int baseAmt, int curAmt, string statDesc) {
        if (baseAmt == curAmt)
        {
            return "<b><color=black>" + statDesc + "</color></b> (" + curAmt + ")\n";
        }
        else {
            return "<b><color=black>" + statDesc + "</color></b> (*" + curAmt + "*)\n";
        }
    }

    //for debugging
    public string PrintStat() {
        string stringToBuild;
        stringToBuild = "----------------------------\n" +
            "name: " + name +
            "\ndescription: " + description +
            "\ndistrict: " + district +
            "\nlocation: " + location +
            "\nbaseAmount: " + baseAmount +
            "\npopulation: " + population +
            "\ncommerce: " + commerce +
            "\ntourist: " + tourist +
            "\nbeauty: " + beauty +
            "\nindustry: " + industry +
            "\nresource: " + resource +
        "\npbaseopulation: " + basepopulation +
        "\nbasecommerce: " + basecommerce +
        "\nbasetourist: " + basetourist +
        "\nbasebeauty: " + basebeauty +
        "\nbaseindustry: " + baseindustry +
        "\nbaseresource: " + baseresource;
        return stringToBuild;
    }




}
