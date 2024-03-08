using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class FirstScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Average(1,3,6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Average(int a, int b, int c)
    {
        int average = (a + b + c) / 3;
        Debug.Log("Average of the three numbers is : " + average);
    }
}
