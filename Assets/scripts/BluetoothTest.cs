using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;



public class BluetoothTest : MonoBehaviour
{

    private bool IsConnected;
    private bool Isx=false;
    public TMP_Text context; 
    public static string dataRecived = "";

    // Start is called before the first frame update
    void Start()
    {
    #if UNITY_2020_2_OR_NEWER
        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)
          || !Permission.HasUserAuthorizedPermission(Permission.FineLocation)
          || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN")
          || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADVERTISE")
          || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
                    Permission.RequestUserPermissions(new string[] {
                        Permission.CoarseLocation,
                            Permission.FineLocation,
                            "android.permission.BLUETOOTH_SCAN",
                            "android.permission.BLUETOOTH_ADVERTISE",
                             "android.permission.BLUETOOTH_CONNECT"
                    });
        #endif
    #endif

        IsConnected = false;
        BluetoothService.CreateBluetoothObject();
       
    }

    // Update is called once per frame
    void Update()
    {   
        if (!IsConnected)
        {
            IsConnected =  BluetoothService.StartBluetoothConnection("pika");
            //BluetoothService.Toast("psy"+" status: " + IsConnected);
        }
        else if (IsConnected) {
            if(Isx==false)
            {
                BluetoothService.Toast("psy"+" status: " + IsConnected);
                
                Isx=true;
            }
            try
            {
               string datain =  BluetoothService.ReadFromBluetooth();
                if (datain.Length > 1)
                {
                    dataRecived = datain;
                    string[] data =dataRecived.Split(',');
                    context.text=dataRecived;
             
                }

            }
            catch (Exception e)
            {
                BluetoothService.Toast("Error in connection");
            }
        }
        
    }

    public void GetDevicesButton()
    {
       string[] devices = BluetoothService.GetBluetoothDevices();

        foreach(var d in devices)
        {
            Debug.Log(d);
        }
    }
}
