using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    
    
    private const int NumberOfTiles=19;
    private int[] Selction={-1,-1};
    public List <GameObject> Stones;

    void SpawnStones(int index,Vector3 position)
    {
        GameObject go = Instantiate(Stones[index],position,Quaternion.identity)as GameObject;
        go.transform.SetParent(transform);

    }

    // Start is called before the first frame update
    void Start()
    {
        
         
            Vector3 position= new Vector3(1.2f,0.3f,0.6f);
            SpawnStones(0,position);
        


    }
    void DrawGoBoard()

    {
        Vector3 width = Vector3.right *NumberOfTiles;
        Vector3 height = Vector3.forward *NumberOfTiles;
        for (int i=0;i<=NumberOfTiles;i++)
        {
            Vector3 start=Vector3.forward *i;
            Debug.DrawLine(start,start+width);
            for (int j=0;j<=NumberOfTiles;j++)
        {
            start=Vector3.right *j;
            Debug.DrawLine(start,start+height);
        }

        }
     if(Selction[0]>=0 && Selction[1]>=0)
        {
            Debug.DrawLine(
                Vector3.forward*Selction[1] +Vector3.right*Selction[0],
                Vector3.forward*(Selction[1]+1)+Vector3.right*(Selction[0]+1));
                Debug.DrawLine(
                Vector3.forward*(Selction[1]+1) +Vector3.right*(Selction[0]),
                Vector3.forward*(Selction[1])+Vector3.right*(Selction[0]+1));
        }


    }
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
        }
        else{
            Selction[0]=-1;
            Selction[1]=-1;

        }

    }
    void Update()
    {
        UpdateSelection();
       
        DrawGoBoard();
        
    }
}
