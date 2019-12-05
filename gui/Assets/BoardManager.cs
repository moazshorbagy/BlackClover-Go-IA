using System.Collections;
using System; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Board
{
    

public class BoardManager : MonoBehaviour
{
    private const int NumberOfTiles=19;
    private int BlackStoneScore=0;
    private int WhiteStoneScore=0;
    public Text BlackStoneText;
    public Text WhiteStoneText;
    public List <GameObject> Stones; // a list containg a white stone and a black stone
    Dictionary<Vector2, GameObject> SpawnedStones = new Dictionary<Vector2, GameObject>();// hash map that contatins all stones currently available on the board

/*this Function takes an index which indicates which stone to spawn, index is 0 for black stones and 1 for white stones
  it's second parameter is the position to spawn the stone at position should range from 0 to 18 in x and y directions.
*/
    void SpawnStones(int index,Vector2 position)
    {

        if(CheckInBounds(position)==false)
        {
            Debug.Log("This position is out of bounds! " );
            return;
        }
        float XPos=1.2f+(position.x*1.2f);
        float YPos=1.2f+(position.y*1.2f);
        Vector3 MappedPosition =new Vector3(XPos,0.3f,YPos);
        GameObject Stone = Instantiate(Stones[index],MappedPosition,Quaternion.identity)as GameObject;
        Stone.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
        Stone.transform.SetParent(transform);
        try
        {
            SpawnedStones.Add(position,Stone);

        }
        catch(ArgumentException e)
        {
            Debug.Log("This position is already taken!" );

        }
        

    }
    /* This function checks if the given position is inside the board bounds or not
      and returns true if the move is inside the bounds and returns false otherwise
    */
    bool CheckInBounds(Vector2 position)
    {
        if (position.x<0 || position.x>18 || position.y<0 ||position.y>18 )
            return false;
        
        return true;
        
        
    }
/*
This Function takes an array of positions to delete the stones present at these locations 
It returns true if all stones where deleted successfully
It returns false if there is no stone at any position in the passed array , Throwing a KeyNotFoundException
*/
    bool RemoveStones(Vector2[] arrayOfPositions)
    {
        bool allDeleted =true;
        foreach(Vector2 position in arrayOfPositions) 
        {
        try
        {
            SpawnedStones.Remove(position);
            Destroy(SpawnedStones[position]);     
        }
        catch (KeyNotFoundException e)
        {
            allDeleted=false;  
        }
        }
         return allDeleted;

    }

    // Start is called before the first frame update
    void Start()
    {
    
        
            
        
    }

    /*
    This function gets the user actions and spawn a stone on the board if the user clicks on a valid position on the board.
    It also passes his turn if The keyboard button "P" was pressed
    */

    void GetUserActions()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        

        int xPos= (int)Math.Round((mouseWorldPosition.x/1.2)-1);
        int yPos= (int)Math.Round((mouseWorldPosition.z/1.2)-1);

        
        Vector2 spawningPosition=new Vector2 (xPos,yPos);
        if (Input.GetMouseButtonDown(0))
        {
            SpawnStones(0,spawningPosition);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
             Debug.Log("Your Turn is passed " );
        }   


    }
    
    void Update()
    {
        GetUserActions();
        

        
        

       
    }
}
}