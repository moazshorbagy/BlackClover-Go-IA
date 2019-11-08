using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBoard : MonoBehaviour
{
    public float x_speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, x_speed, 0);
        
    }
}
