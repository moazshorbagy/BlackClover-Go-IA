using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    

public class BoardManager : MonoBehaviour
{
    
    
    private const int NumberOfTiles=19;
    private int[] Selction={-1,-1};
    public GameObject particle;
    public List <GameObject> Stones; // a list containg a white stone and a black stone
    Dictionary<Vector2, GameObject> SpawnedStones = new Dictionary<Vector2, GameObject>();

/*this Function takes an index which indicates which stone to spawn, index is 0 for black stones and 1 for white stones
  it's second parameter is the position to spawn the stone at position should range from 0.0f to 18.0f in x and y directions.
*/
    void SpawnStones(int index,Vector2 position)
    {
        float XPos=1.2f+(position.x*1.2f);
        float YPos=1.2f+(position.y*1.2f);
        Vector3 MappedPosition =new Vector3(XPos,0.3f,YPos);
        GameObject Stone = Instantiate(Stones[index],MappedPosition,Quaternion.identity)as GameObject;
        Stone.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
        Stone.transform.SetParent(transform);
        SpawnedStones.Add(position,Stone);

    }
/*
This Function takes and array of positions to delete the stones present at these locations 
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
            Destroy(SpawnedStones[position]);     
        }
        catch (KeyNotFoundException)
        {
            allDeleted=false;  
        }
        }
         return allDeleted;

    }

    // Start is called before the first frame update
    void Start()
    {
        
         
            Vector2 position;
            
            position= new Vector2(2.0f,0.0f);
            SpawnStones(0,position);

             
            
            position=new Vector2(1.0f,0.0f);
           
            GameObject Stone =SpawnedStones[position];
            Destroy(Stone);
        


    }
    //Test function will be implemented later

    void UpdateSelection()
    {
        if(!Camera.main)
        {
            return;
        }
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,25.0f,LayerMask.GetMask("GoPlane")))
        {
            
            Selction[0]=(int) hit.point.x;
            Selction[1]=(int) hit.point.z;
            Debug.Log("X= " + hit.point.x);
            Debug.Log("Z= " + hit.point.z);

        }
        else{
            Selction[0]=-1;
            Selction[1]=-1;

        }
         

    }
    
    void Update()
    {
        UpdateSelection();

       
    }
}
}