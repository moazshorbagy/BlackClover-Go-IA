using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    void Update()
    {
        //Event e = Event.current;
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Detected key code: " );
            SceneManager.LoadScene("Menu");

        }
    }
}
