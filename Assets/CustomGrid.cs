using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    //properties of the grid
    public int rows;
    public int cols;
    public float height;
    public float width;
    public float spacing;
    //height and width of each child in grid, derived from scale
    float cellHeight;
    float cellWidth;
    //scale used to scale each child
    public float scale;
    //current status of the tbale
    int numOfChild = 0;
    List<GameObject> children = new List<GameObject>();
    

    public void AddChild(GameObject gameObject) {
        numOfChild++;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        cellWidth = rectTransform.rect.width * scale;
        
        gameObject.transform.SetParent(transform, false);
        gameObject.transform.localScale = new Vector3(scale, scale, 1);

        //this will pave the children from left to right
        gameObject.transform.localPosition = new Vector3(cellWidth * numOfChild + spacing - width/2, -height / 6 + height/2, 1); //not sure why this is the case
        print(spacing);
    }

    public void RemoveChild() { 
        
    }
}
