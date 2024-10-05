using System;
using Android.BLE;
using TMPro;
using UnityEngine;

public class MyScript : MonoBehaviour
{
    private bool IsConnected = false;

    private string deviceAddress = "E4:65:B8:83:A5:5A";

    private string serviceUUID = "4fafc201-1fb5-459e-8fcc-c5c9c331914b";

    private string characteristicUUID = "beb5483e-36e1-4688-b7f5-ea07361b26a8";

    [SerializeField]
    GameObject blecanvas;

    private BleDevice bleDevice;

    private BleGattCharacteristic characteristic;

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

    public DistanceCalc dsd;

    private void Start()
    {
        ConnectToDevice();
    }

    private void ConnectToDevice()
    {
        BleManager.Instance.SearchForDevices(5000, OnDeviceFound);
    }

    private void OnDeviceFound(BleDevice device)
    {
        if (device.MacAddress == deviceAddress)
        {
            bleDevice = device;
            bleDevice.Connect(OnDeviceConnected, OnDeviceDisconnected);
        }
    }

    private void OnDeviceConnected(BleDevice device)
    {
        characteristic = device.GetCharacteristic(serviceUUID, characteristicUUID);
        if (characteristic != null)
        {
            characteristic.Subscribe(OnCharacteristicValueChanged);
        }
    }

    private void OnDeviceDisconnected(BleDevice device)
    {
        Debug.LogWarning("Device disconnected");
    }

    private void OnCharacteristicValueChanged(byte[] data)
    {
        if (!IsConnected)
        {
            blecanvas.SetActive(false);
            IsConnected = true;
        }

        // Check if the received data has the expected length (16 bytes)
        if (data.Length == 16)
        {
            // Extract joystick X and Y values (floats)
            float joystickX = BitConverter.ToSingle(data, 0);
            xax = BitConverter.ToSingle(data, 4);

            // Extract button states (floats)
            but1 = BitConverter.ToSingle(data, 8) == 1.0f ? 1 : 0;
            but2 = BitConverter.ToSingle(data, 12) == 1.0f ? 1 : 0;

            if (xax <= 1000)
            {
                Debug.Log("Sdadsad");
                Lamp.transform.position = new Vector3((Lamp.transform.position.x + 0.01F) >= +2 ? +2 : Lamp.transform.position.x + 0.01F,
                    Lamp.transform.position.y,
                    Lamp.transform.position.z);
            }
            else if (xax >= 2000)
            {
                Lamp.transform.position = new Vector3((Lamp.transform.position.x - 0.01F) <= -2 ? -2 : Lamp.transform.position.x - 0.01F,
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
                ts.IncrementList((int)dsd.dist, dsd.resistance);
                ispressedmain2 = false;
            }
            // You can now use these values to control your game objects
            // For example:
            // UpdateJoystickPosition(joystickX, joystickY);
            // HandleButtonStates(button1State, button2State);
        }
        else
        {
            // Handle the case where the received data has an unexpected length
            Debug.LogWarning("Received data has an unexpected length: " + data.Length);
        }
    }
}
