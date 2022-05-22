using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//This controls all player movements, animations
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GridManager gridManager;
    public GameManager gameManager;
    public Transform movePoint;
    public int curWaypoint = 0;
    public bool finishedMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init() { 
        transform.position = gridManager.waypoints[0].transform.position;
        movePoint = gridManager.waypoints[0].transform;
    }

    public void Move(int moves)
    {
        StartCoroutine("NextMove", moves);
        
        
    }
    public IEnumerator NextMove(int moves) {
        

        for (int i = 0; i < moves; i++)
        {
            finishedMoving = false;
            gridManager.playerPos++;
            if (gridManager.playerPos >= gridManager.waypoints.Count)
            {
                gridManager.playerPos = 0;
            }
            movePoint = gridManager.waypoints[gridManager.playerPos].transform;
            yield return new WaitUntil(() => finishedMoving == true);
        }
        if (gameManager.pickingCard) //when finished moving, gameManager.pickingCard == true
        {
            gridManager.Landed(gridManager.playerPos);
        }

    }




    // Update is called once per frame
    void Update()
    {
        if (!transform.position.Equals(movePoint.position))
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        }
        else {
            finishedMoving = true;
            
        }
        
    }
}
