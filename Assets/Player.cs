using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.Animation;


//This controls all player movements, animations
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GridManager gridManager;
    public GameManager gameManager;
    public SpriteRenderer playerSprite;
    public Transform movePoint;
    public int curWaypoint = 0;
    public bool finishedMoving = true;
    public Animator animator;
    public SpriteLibraryAsset spriteLibrary;

    // Start is called before the first frame update
    void Start()
    {
        AnimationClip anim;
    }

    public void SetCEO(int ceoNum) {
        switch (ceoNum) {
            case 0:
                break;
            case 1:
                break;
            default:
                break;
        }
        spriteLibrary.AddCategoryLabel(Resources.LoadAll<Sprite>("Char" + (ceoNum + 1))[0], "Idle", "Char1_0");
        spriteLibrary.AddCategoryLabel(Resources.LoadAll<Sprite>("Char" + (ceoNum + 1))[1], "Idle", "Char1_1");
        spriteLibrary.AddCategoryLabel(Resources.LoadAll<Sprite>("Char" + (ceoNum + 1))[2], "Walk", "Char1_2");
        spriteLibrary.AddCategoryLabel(Resources.LoadAll<Sprite>("Char" + (ceoNum + 1))[3], "Walk", "Char1_3");
        spriteLibrary.AddCategoryLabel(Resources.LoadAll<Sprite>("Char" + (ceoNum + 1))[4], "Walk", "Char1_4");
        spriteLibrary.AddCategoryLabel(Resources.LoadAll<Sprite>("Char" + (ceoNum + 1))[5], "Walk", "Char1_5");
        print("dwedw");
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
            Vector3 movement = transform.position - movePoint.position;
            animator.SetFloat("Horizontal", -movement.x);
            animator.SetFloat("Vertical", -movement.y);
            animator.SetBool("Moving", true);
            yield return new WaitUntil(() => finishedMoving == true);
        }
        if (gameManager.pickingCard) //when finished moving, gameManager.pickingCard == true
        {
            animator.SetBool("Moving", false) ;
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
