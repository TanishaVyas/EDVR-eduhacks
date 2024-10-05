using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class callwifi : MonoBehaviour
{
    private WebSocket ws;

    public TMP_Text context;

    public int but1;

    public int but2;

    public GameObject Lamp;

    bool ispressedmain;

    bool canpress;

    public string[] data;

    bool ispressedmain2;

    bool canpress2;

    [SerializeField]
    CardboardReticlePointer cr;

    [SerializeField]
    Tableshowdata ts;
    public float xax;

    public  DistanceCalc  dsd;
    void Start()
    {
        // Replace with the IP address of your NodeMCU
        string serverAddress = "ws://192.168.43.1:8080";

        ws = new WebSocket(serverAddress);
        ws.OnMessage += OnMessage;
        ws.Connect();
    }

    void Update()
    {
        if (data.Length > 7)
        {    
            if (xax <= -0.50 )
            {   
                Debug.Log("Sdadsad");
                Lamp.transform.position =
                    new Vector3((Lamp.transform.position.x + 0.01F)>=+2?+2:Lamp.transform.position.x + 0.01F,
                        Lamp.transform.position.y,
                        Lamp.transform.position.z);
            }
            else if (xax>=0.50)
            {   
                Lamp.transform.position =
                    new Vector3((Lamp.transform.position.x - 0.01F)<=-2?-2:Lamp.transform.position.x - 0.01F,
                        Lamp.transform.position.y,
                        Lamp.transform.position.z);
            }
            if (canpress && but1 == 0)
            {
                ispressedmain = true;
                canpress = false;
            }
            else if (but1 == 1)
            {
                canpress = true;
                ispressedmain = false;
            }
            if (canpress2 && but2 == 0)
            {
                ispressedmain2 = true;
                canpress2 = false;
            }
            else if (but2 == 1)
            {
                canpress2 = true;
                ispressedmain2 = false;
            }
        }
        if (ispressedmain)
        {
            //doo change here
            cr.isjustpress = 1;
            Debug.Log("pressed");
            ispressedmain = false;
        }
        if (ispressedmain2)
        {
            //func
            ts.IncrementList((int)dsd.dist,dsd.resistance);
            ispressedmain2 = false;
        }
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        // Handle received messages here
        //Debug.Log("Received message: " + e.Data);
        data = e.Data.Split(',');
        context.text = e.Data;
        try
        {
            but1 = (int) float.Parse(data[7]);
            but2 = (int) float.Parse(data[8]);
            xax = float.Parse(data[4]);
        }
        catch
        {
            Debug.Log("err");
        }
    }

    private void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }
}
