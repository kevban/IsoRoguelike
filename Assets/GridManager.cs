using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

//this controls all aspect of the game board and UI
public class GridManager : MonoBehaviour
{
    public GridSystem grid;
    public GameObject waypointPrefab;
    public GameObject buildtilePrefab;
    public bool coroutineAllowed = true;
    public Player player;
    public GameObject diceImage;
    public GameObject cardHolder;
    public GameManager gameManager;
    public Tilemap tilemap;
    public Tile greenTile;
    public AnimatedTile highlightedTile;
    public List<Vector3Int> highlightedTilePos;
    public GameObject inventoryCanvas;
    public bool isBuilding;
    public bool showingStat = false;
    public List<GameObject> buildingPoints = new List<GameObject>();
    public Cash cash;
    public EndOfTurnIncome endOfTurnIncome;
    public GameObject debugCanvas;
    public GameObject statText;
    public EffectDisplay effectDisplay;

    List<BuildingPoint> tilesToDecreaseResource = new List<BuildingPoint>();


    public int playerPos; //The player's current position (after dice is rolled)
    public List<Vector3Int> walkableTiles = new List<Vector3Int>(); // the Vector3Ints of the walkable tiles
    public List<BoardTile> buildTiles = new List<BoardTile>();
    [HideInInspector] public float xPos = 1.5f; // the bottom middle walkable tile on the square map
    [HideInInspector] public float yPos = -2.5f;
    public int xPosInt = -4;// the int pos in tilemap
    public int yPosInt = -7;
    [HideInInspector] public List<GameObject> waypoints; //the coordinates of each walkable tile the the player will follow
    Vector3Int tempTileLocation;
    bool tempTileActivated = false;


    public Vector3Int location; //mouse pos on tilemap
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        waypoints = new List<GameObject>();
        BuildWaypoints();
        player.Init();

    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0) && isBuilding && (gameManager.selectedCard != null))
        {

            Build();
        }*/
        if (Input.GetMouseButtonDown(1) && !showingStat)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            location = tilemap.WorldToCell(mousePos);
            location.z = 0;
            location.x -= 5;
            location.y -= 5;
            for (int i = 0; i < buildingPoints.Count; i++)
            {
                if (location == buildingPoints[i].GetComponent<BuildingPoint>().tile.location) {
                    effectDisplay.ShowEffect(buildingPoints[i].GetComponent<BuildingPoint>().tile);
                    //ShowStat(buildingPoints[i].GetComponent<BuildingPoint>().tile);
                }
            }
        }
    }

    public void ShowStat(BoardTile tile) {
        debugCanvas.SetActive(true);
        showingStat = true;
        statText.GetComponent<TextMeshProUGUI>().text = tile.PrintStat();
    }

    public void HideStat() {
        showingStat = false;
        debugCanvas.SetActive(false);
    }

    /// <summary>
    /// Build the selected building at mouse position
    /// </summary>
    public void Build() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        location = tilemap.WorldToCell(mousePos);
        location.z = 0;
        location.x -= 5;
        location.y -= 5;
        tilemap.SetTile(tempTileLocation, greenTile);
        tempTileActivated = false;
        tilemap.SetTile(location, gameManager.selectedTile);
        resetHighlight();
        GameObject buildingPoint = Instantiate(buildtilePrefab, tilemap.CellToWorld(location), Quaternion.identity);
        buildingPoint.GetComponent<BuildingPoint>().tile = gameManager.selectedCard.boardTile.Clone();
        buildingPoint.GetComponent<BuildingPoint>().tile.location = location;
        buildingPoint.GetComponent<BuildingPoint>().tile.district = FindDistrict(location);
        buildingPoints.Add(buildingPoint);
        ProcBuild(buildingPoint.GetComponent<BuildingPoint>().tile);
        Destroy(gameManager.selectedCard.gameObject);
        inventoryCanvas.SetActive(false);
        
        isBuilding = false;

    }



    /// <summary>
    /// Shows a flashing building for user to see. Return true if can build, false otherwise
    /// </summary>
    public bool TempBuild() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        location = tilemap.WorldToCell(mousePos);
        location.z = 0;
        location.x -= 5;
        location.y -= 5;
        if (tilemap.GetTile(location) == highlightedTile)
        {
            tilemap.SetTile(location, gameManager.selectedTile);
            tempTileLocation = location;
            tempTileActivated = true;
            return true;
        }
        else if (tempTileActivated)
        {
            if (tempTileLocation != location) {
                tilemap.SetTile(tempTileLocation, highlightedTile);
                tempTileActivated = false;
                return false;
            }
            return true;
        }
        return false;
    }

    //this function returns the district of a given playerPos
    public int FindDistrict(Vector3Int pos) {
        int district;
        if (pos.x == -5)
        {
            district = 0;
        }
        else if (pos.y == 1)
        {
            district = 1;
        }
        else if (pos.x == 4)
        {
            district = 2;
        }
        else if (pos.y == -7)
        {
            district = 3;
        }
        else {
            district = 4;
        }
        return district;
    }


    //generates waypoints for character to follow
    private void BuildWaypoints()
    {
        //for this level, index 0, 7, 14, 21 are corners. 4 Districts
        //1-6 district 0, 8-13 district 1, 15-20 distrct 2, 22-27 district 3
        for (int i = 0; i < 28; i++)
        {
            waypoints.Add(Instantiate(waypointPrefab, new Vector3(xPos, yPos, 0f), Quaternion.identity));
            walkableTiles.Add(new Vector3Int(xPosInt, yPosInt, 0));
            if (i < 7)
            {
                xPos -= 0.5f;
                yPos += 0.25f;
                yPosInt += 1;


            }
            else if (i < 14)
            {
                xPos += 0.5f;
                yPos += 0.25f;
                xPosInt += 1;
            }
            else if (i < 21)
            {
                xPos += 0.5f;
                yPos -= 0.25f;
                yPosInt -= 1;

            }
            else
            {
                xPos -= 0.5f;
                yPos -= 0.25f;
                xPosInt -= 1;
            }

        }
    }

    public void highlightTiles() {
        if (playerPos == 0)
        {
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1)) == greenTile) {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1));
            }
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y)) == greenTile) {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y));
            }
        }
        else if (playerPos < 7)
        {
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y));

            }
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y)) == greenTile)
            {
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y));
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y), highlightedTile);
            }


        }
        else if (playerPos == 7) {
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1));

            }
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y));
            }


        }
        else if (playerPos < 14)
        {
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1));

            }
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1));
            }


        }
        else if (playerPos == 14)
        {
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y));

            }
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1));
            }
        }
        else if (playerPos < 21)
        {
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y));

            }
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y));
            }
        }
        else if (playerPos == 21)
        {
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y));

            }
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1));
            }
        }
        else
        {
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1));

            }
            if (tilemap.GetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1)) == greenTile)
            {
                tilemap.SetTile(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1), highlightedTile);
                highlightedTilePos.Add(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1));
            }
        }
    }

    public void resetHighlight() {
        for (int i = 0; i < highlightedTilePos.Count; i++) {
            if (tilemap.GetTile(highlightedTilePos[i]) == highlightedTile) {
                tilemap.SetTile(highlightedTilePos[i], greenTile);
            }
        }
        highlightedTilePos.Clear();
    }
    //procedurally generate square level. Unused
    private void GenerateBuildLevel() {
        //for this level, index 0, 7, 14, 21 are corners. 4 Districts
        //1-6 district 0, 8-13 district 1, 15-20 distrct 2, 22-27 district 3
        gameManager.district = new List<List<int>>();
        for (int i = 0; i < 4; i++) {
            gameManager.district.Add(new List<int>());
            for (int s = 0; s < 2; s++)
            {
                gameManager.district[i].Add(0);
            }
        }
        for (int i = 0; i < 28; i++)
        {
            waypoints.Add(Instantiate(waypointPrefab, new Vector3(xPos, yPos, 0f), Quaternion.identity));

            tilemap.SetTile(tilemap.WorldToCell(new Vector3(xPos, yPos, 0f)), greenTile);
            if (i == 0)
            {
                xPos -= 0.5f;
                yPos -= 0.25f;
                //tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[0].tile);
                xPos += 1f;
                //tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos -= 1f; //next road pos
                yPos += 0.5f;
            }
            else if (i < 7)
            {
                xPos -= 0.5f;
                yPos -= 0.25f;
                //tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos += 1f;
                yPos += 0.5f;
                //tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos -= 1f; //next road pos
            }
            else if (i == 7)
            {
                xPos -= 0.5f;
                yPos -= 0.25f;
                // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                yPos += 0.5f;
                // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos += 1f; //next road pos
            }
            else if (i < 14)
            {
                xPos -= 0.5f;
                yPos += 0.25f;
                // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos += 1f; //next road pos
                yPos -= 0.5f;
                // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                yPos += 0.5f; //next road pos
            }
            else if (i == 14) {
                xPos -= 0.5f;
                yPos += 0.25f;
                // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos += 1f; //next road pos
                            // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                yPos -= 0.5f; //next road pos
            }
            else if (i < 21)
            {
                xPos -= 0.5f;
                yPos -= 0.25f;
                // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos += 1f; //next road pos
                yPos += 0.5f;
                // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                yPos -= 0.5f; //next road pos
            }
            else if (i == 21) {
                xPos += 0.5f;
                yPos += 0.25f;
                //  tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                yPos -= 0.5f; //next road pos
                              // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos -= 1f; //next road pos
            }
            else
            {
                xPos -= 0.5f;
                yPos += 0.25f;
                // tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos += 1f; //next road pos
                yPos -= 0.5f;
                //  tilemap.SetTile(Vector3Int.FloorToInt(new Vector3(xPos, yPos, 0f)), gameManager.tiles[15].tile);
                xPos -= 1f; //next road pos
            }

        }
    }

    public void RollDice() {
        if (coroutineAllowed) {
            StartCoroutine("RollTheDice");
        }
    }

    private IEnumerator RollTheDice() {
        coroutineAllowed = false;
        gameManager.selectedCard = null;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++) {
            randomDiceSide = Random.Range(0, 6);
            diceImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/dice" + (randomDiceSide + 1).ToString());
            yield return new WaitForSeconds(0.05f);
        }
        resetHighlight();
        player.Move(randomDiceSide + 1);
        gameManager.pickingCard = true;
        coroutineAllowed = true;
    }

    

    public void Landed(int index) {
        Vector3 pos1;
        Vector3 pos2;
        if (index == 0)
        {
            pos1 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1));
            pos2 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y));
        }
        else if (playerPos < 7)
        {
            pos1 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y));
            pos2 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y));
        }
        else if (playerPos == 7)
        {
            pos1 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1));
            pos2 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y));
        }
        else if (playerPos < 14)
        {
            pos1 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1));
            pos2 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1));
        }
        else if (playerPos == 14)
        {
            pos1 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y));
            pos2 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1));
        }
        else if (playerPos < 21)
        {
            pos1 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y));
            pos2 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x - 1, walkableTiles[playerPos].y));
        }
        else if (playerPos == 21)
        {
            pos1 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x + 1, walkableTiles[playerPos].y));
            pos2 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1));
        }
        else
        {
            pos1 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y + 1));
            pos2 = tilemap.CellToWorld(new Vector3Int(walkableTiles[playerPos].x, walkableTiles[playerPos].y - 1));
        }
        for (int i = 0; i < buildingPoints.Count; i++) {
            if (IsPointWithinCollider(buildingPoints[i].GetComponent<Collider>(), pos1) || IsPointWithinCollider(buildingPoints[i].GetComponent<Collider>(), pos2)) {
                ProcLanding(buildingPoints[i].GetComponent<BuildingPoint>());
            }
        }
        ProcEndOfTurn();
    }

    //go through each BuildingPoint to display EOT earnings
    private IEnumerator ShowEOTEarnings()
    {
        for (int i = 0; i < buildingPoints.Count; i++)
        {
            if (buildingPoints[i].GetComponent<BuildingPoint>().endOfTurnIncome > 0) {
                buildingPoints[i].GetComponent<BuildingPoint>().CashPopEOT();
                cash.UpdateCash(buildingPoints[i].GetComponent<BuildingPoint>().endOfTurnIncome);
                buildingPoints[i].GetComponent<BuildingPoint>().tile.GenerateDescription();
                yield return new WaitForSeconds(0.2f);
            }

        }
        yield return new WaitForSeconds(0.8f);
            gameManager.PickCards();
            gameManager.pickingCard = false;
        
    }

    public void ProcLanding(BuildingPoint tile) {
        int landingEarnings = 0; //keep track of specific building landing earnings
        
        switch (tile.tile.name)
        {
            case "house": break;
            case "pizza": break;
            case "burger": break;
            case "grocery": break;
            case "movie":
                landingEarnings += gameManager.worldPop + gameManager.worldCom;
                break;
            case "restaurant":
                tile.tile.commerce++;
                break;
            case "office": break;
            case "apartment": break;
            case "hotel": break;
            case "park":
                landingEarnings += 2 * (gameManager.districtPop[tile.tile.district]);
                break;
            case "wonderland":
                landingEarnings += 2 * (gameManager.worldPop);
                break;
            case "forest": break;
            case "wood": break;
            case "lodge": break;
            case "mountain": break;
            default:
                print(tile.tile.name);
                break;
        }
        ShowEarnings(tile.tile.baseAmount + gameManager.districtCom[tile.tile.district] * Mathf.Min(1, tile.tile.commerce) + landingEarnings, tile);
    }
    //displays the earnings popup with given earnings
    public void ShowEarnings(int earnings, BuildingPoint buildingPoint) {
        cash.UpdateCash(earnings);
        if (earnings > 0) {
            buildingPoint.CashPop(earnings);
        }
    }

   

    public void ProcBuild(BoardTile tile) {
        switch (tile.name)
        {

            case "house": break;
            case "pizza": break;
            case "burger":
                gameManager.burgNum++;
                break;
            case "grocery": break;
            case "movie": break;
            case "restaurant": break;
            case "office": 
                gameManager.incomePerIndustry++;
                break;
            case "apartment": break;
            case "hotel": break;
            case "park": break;
            case "wonderland": break;
            case "forest": break;
            case "wood": break;
            case "lodge": break;
            case "mountain": break;
            default:
                print(tile.name);
                break;
        }
        CalcEndOfTurn();
    }

    public void ProcDestroy(BuildingPoint buildingToDestroy) {
        
        tilemap.SetTile(buildingToDestroy.tile.location, greenTile);
        print("procced destroy");
        switch (buildingToDestroy.tile.name)
        {

            case "house": break;
            case "pizza": break;
            case "burger":
                gameManager.burgNum--;
                break;
            case "grocery": break;
            case "movie": break;
            case "restaurant": break;
            case "office": 
                gameManager.incomePerIndustry--;
                break;
            case "apartment": break;
            case "hotel": break;
            case "park": break;
            case "wonderland": break;
            case "forest":
                GameObject buildingPoint = Instantiate(buildtilePrefab, tilemap.CellToWorld(buildingToDestroy.tile.location), Quaternion.identity);
                buildingPoint.GetComponent<BuildingPoint>().tile = gameManager.allTiles[12].Clone(); //wood
                buildingPoint.GetComponent<BuildingPoint>().tile.location = buildingToDestroy.tile.location;
                buildingPoint.GetComponent<BuildingPoint>().tile.district = buildingToDestroy.tile.district;
                tilemap.SetTile(buildingPoint.GetComponent<BuildingPoint>().tile.location, buildingPoint.GetComponent<BuildingPoint>().tile.CreateTile(true));
                buildingPoints.Add(buildingPoint);
                break;
            case "wood": break;
            case "lodge": break;
            case "mountain": break;
            default:
                print(buildingToDestroy.tile.name);
                break;
        }
        buildingPoints.Remove(buildingToDestroy.gameObject);
        Destroy(buildingToDestroy.gameObject);
    }
    //this function go through each BuildingPoint and update its EOT income for display animation;
    //in addition, this function also "tie up loose ends" on things like grocery
    void UpdateEOTIncome() {
        for (int i = 0; i < buildingPoints.Count; i++) {
            BuildingPoint curPoint = buildingPoints[i].GetComponent<BuildingPoint>();
            int EOTIncome = 0;

            //Special effects:
            //grocery
            if (curPoint.tile.groceries < gameManager.districtGroceries[curPoint.tile.district] && curPoint.tile.population > 0) {
                curPoint.tile.population *= (gameManager.districtGroceries[curPoint.tile.district] + 1);
                curPoint.tile.groceries = gameManager.districtGroceries[curPoint.tile.district];
            }
            

            //calculating
            EOTIncome += curPoint.tile.population / 4;
            EOTIncome += curPoint.tile.tourist * gameManager.worldBeauty;
            curPoint.endOfTurnIncome += EOTIncome;
            curPoint.tile.GenerateDescription();


            //industry income is added in CalcEndOfTurn()
        }
    }

    //this function is used to update the grid when it changes
    public void CalcEndOfTurn() {
        List<int> populationCounter = new List<int>(); //calculate population in each district
        List<int> commerceCounter = new List<int>();
        BoardTile curTile;


        //below keeps track of total EOT income. Unused for now
        int populationIncome = 0;
        int industryIncome = 0;
        int tourismIncome = 0;
        tilesToDecreaseResource.Clear();
        gameManager.Recalc(); //resetting all GameManager variables to 0;

        
        

     for (int i = 0; i < buildingPoints.Count; i++)
        {
            buildingPoints[i].GetComponent<BuildingPoint>().endOfTurnIncome = 0;
            curTile = buildingPoints[i].GetComponent<BuildingPoint>().tile;
            if (curTile.name == "grocery")
            {
                    gameManager.districtGroceries[curTile.district]++;
            }
            else if (curTile.name == "burger") {
                curTile.commerce = gameManager.burgNum;
            }

            gameManager.districtPop[curTile.district] += curTile.population;
            gameManager.worldPop += curTile.population;

            gameManager.districtCom[curTile.district] += curTile.commerce;
            gameManager.worldCom += curTile.commerce;
            
            if (curTile.industry > 0) {
                List<BuildingPoint> adjacentTile = CheckAdjacent(curTile);
                for (int s = 0; s < adjacentTile.Count; s++) {
                    if (adjacentTile[s].tile.resource > 0)
                    {
                        tilesToDecreaseResource.Add(adjacentTile[s]);
                        industryIncome += gameManager.incomePerIndustry * curTile.industry;
                        buildingPoints[i].GetComponent<BuildingPoint>().endOfTurnIncome += gameManager.incomePerIndustry * curTile.industry;
                    }
                    else if (adjacentTile[s].tile.infiniteResource) {
                        print("yes");
                        industryIncome += gameManager.incomePerIndustry * curTile.industry;
                        buildingPoints[i].GetComponent<BuildingPoint>().endOfTurnIncome += gameManager.incomePerIndustry * curTile.industry;
                    }
                }
            }
            gameManager.worldBeauty += curTile.beauty;
            gameManager.worldTourist += curTile.tourist;
        }

        UpdateEOTIncome();

        //below are used for calculating end of turn income. Unused for now
        // calculating population income
        populationIncome = gameManager.worldPop/4;
        //calculating tourism income
        tourismIncome = gameManager.worldBeauty * gameManager.worldTourist;
        //endOfTurnIncome.UpdateIncome(populationIncome, industryIncome, tourismIncome);
    }

    // this function returns the adjacent BuildingPoints of given BoardTile
    public List<BuildingPoint> CheckAdjacent(BoardTile tile) {
        Vector3 pos1 = tilemap.CellToWorld(new Vector3Int(tile.location.x, tile.location.y + 1));
        Vector3 pos2 = tilemap.CellToWorld(new Vector3Int(tile.location.x, tile.location.y - 1));
        Vector3 pos3 = tilemap.CellToWorld(new Vector3Int(tile.location.x + 1, tile.location.y));
        Vector3 pos4 = tilemap.CellToWorld(new Vector3Int(tile.location.x - 1, tile.location.y));
        List<BuildingPoint> adjacentTiles = new List<BuildingPoint>();
        for (int i = 0; i < buildingPoints.Count; i++) {
            Collider collider = buildingPoints[i].GetComponent<Collider>();
            if (IsPointWithinCollider(collider, pos1) || IsPointWithinCollider(collider, pos2) || IsPointWithinCollider(collider, pos3) || IsPointWithinCollider(collider, pos4))
            {
                adjacentTiles.Add(buildingPoints[i].GetComponent<BuildingPoint>());
            }
        }
        return adjacentTiles;
    }

    //this is called after player finish moving
    public void ProcEndOfTurn() {
        CalcEndOfTurn();
        for (int i = 0; i < tilesToDecreaseResource.Count; i++) {
            if (tilesToDecreaseResource[i] != null) {
                tilesToDecreaseResource[i].tile.resource--;
                tilesToDecreaseResource[i].ResourceDepletionEOT();
                if (tilesToDecreaseResource[i].tile.resource <= 0)
                {
                    ProcDestroy(tilesToDecreaseResource[i]);
                }
            }
            
            
        }
        gameManager.levelText.UpdateLevel();
        StartCoroutine("ShowEOTEarnings");

    }

    public void CheckPassive(BoardTile tile) {
        switch (tile.name)
        {

            case "house": break;
            case "pizza": break;
            case "burger":break;
            case "grocery": 

                break;
            case "movie": break;
            case "restaurant": break;
            case "office": break;
            case "apartment": break;
            case "hotel": break;
            case "park": break;
            case "wonderland": break;
            case "forest": break;
            case "wood": break;
            case "lodge": break;
            case "mountain": break;
            default:
                print(name);
                break;
        }
    }

    public int FindOccur(List<int> list, int item) {
        int count = 0;
        for (int i = 0; i < list.Count; i++) {
            if (list[i] == item) {
                count++;
            }
        }
        return count;
    }

    public void PassBy(int index)
    {
        
    }

    public static bool IsPointWithinCollider(Collider collider, Vector3 point)
    {
        return (collider.ClosestPoint(point) - point).sqrMagnitude < Mathf.Epsilon * Mathf.Epsilon;
    }
}
