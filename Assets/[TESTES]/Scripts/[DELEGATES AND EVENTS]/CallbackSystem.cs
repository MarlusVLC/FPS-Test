using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackSystem : MonoBehaviour
{

    public delegate void OnMessageReceived();

    public event OnMessageReceived ONComplete;
    
    // Start is called before the first frame update
    void Start()
    {
        // OnMessageReceived test = WriteMessage;
        // test += WriteSecMessage;
        // test();

        ONComplete += WriteMessage;
        ONComplete += WriteSecMessage;
        ONComplete -= WriteMessage;
        
        ONComplete();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WriteMessage()
    {
        Debug.Log("WriteMessage() Called");
    }

    void WriteSecMessage()
    {
        Debug.Log("This is the second message");
    }
}
