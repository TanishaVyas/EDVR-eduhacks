#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

// Define UUIDs for the BLE service and characteristics
#define SERVICE_UUID        "4fafc201-1fb5-459e-8fcc-c5c9c331914b"
#define CHARACTERISTIC_UUID "beb5483e-36e1-4688-b7f5-ea07361b26a8"

// Pins for joystick and buttons
const int joystickXPin = 33; // Analog pin for joystick X-axis
const int joystickYPin = 32; // Analog pin for joystick Y-axis
const int button1Pin = 35;   // Digital pin for button 1
const int button2Pin = 34;   // Digital pin for button 2
int ledpin = 2;

// BLE variables
BLEServer* pServer = NULL;
BLECharacteristic* pCharacteristic = NULL;
bool deviceConnected = false;
bool oldDeviceConnected = false;
uint32_t value = 0;

// Server callback handling connection and disconnection
class MyServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* pServer) {
      deviceConnected = true;
    };

    void onDisconnect(BLEServer* pServer) {
      deviceConnected = false;
    }
};

void setup() {
  Serial.begin(115200);

  // Initialize joystick, buttons, and LED
  pinMode(button1Pin, INPUT_PULLUP);
  pinMode(button2Pin, INPUT_PULLUP);
  pinMode(ledpin, OUTPUT);

  // Create the BLE Device
  BLEDevice::init("ESP32_Controller");

  // Create the BLE Server
  pServer = BLEDevice::createServer();
  pServer->setCallbacks(new MyServerCallbacks());

  // Create the BLE Service
  BLEService *pService = pServer->createService(SERVICE_UUID);

  // Create a BLE Characteristic
  pCharacteristic = pService->createCharacteristic(
                      CHARACTERISTIC_UUID,
                      BLECharacteristic::PROPERTY_READ   |
                      BLECharacteristic::PROPERTY_WRITE  |
                      BLECharacteristic::PROPERTY_NOTIFY |
                      BLECharacteristic::PROPERTY_INDICATE
                    );

  // Add a BLE Descriptor
  pCharacteristic->addDescriptor(new BLE2902());

  // Start the service
  pService->start();

  // Start advertising
  BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
  pAdvertising->addServiceUUID(SERVICE_UUID);
  pAdvertising->setScanResponse(false);
  pAdvertising->setMinPreferred(0x0);  // Disable preferred connections
  BLEDevice::startAdvertising();
  Serial.println("Waiting for a client connection to notify...");

  // Turn on the LED to indicate the system is ready
  digitalWrite(ledpin, HIGH);
}

void loop() {
  // Read joystick values
  int joystickX = analogRead(joystickXPin);
  int joystickY = analogRead(joystickYPin);

  // Read button states
  bool button1State = digitalRead(button1Pin) == LOW; // Active low
  bool button2State = digitalRead(button2Pin) == LOW; // Active low

  // Print the values for debugging
  Serial.print("Joystick X: ");
  Serial.print(joystickX);
  Serial.print(" Joystick Y: ");
  Serial.print(joystickY);
  Serial.print(" Button 1: ");
  Serial.print(button1State);
  Serial.print(" Button 2: ");
  Serial.println(button2State);

  // Notify the connected device if the connection is active
  if (deviceConnected) {
    // Allocate a buffer to hold the binary data
    uint8_t buffer[8];

    // Assuming joystick values are 2 bytes each and button states are 1 byte each
    memcpy(buffer, &joystickX, sizeof(int16_t)); // 2 bytes for joystick X
    memcpy(buffer + 2, &joystickY, sizeof(int16_t)); // 2 bytes for joystick Y
    buffer[4] = button1State; // 1 byte for button 1
    buffer[5] = button2State; // 1 byte for button 2

    // Fill the rest of the buffer with zeros (optional)
    memset(buffer + 6, 0, 2);

    // Set the characteristic value to the buffer and notify the connected device
    pCharacteristic->setValue(buffer, sizeof(buffer));
    pCharacteristic->notify();

    // Increment some value (optional, for testing)
    value++;
  }

  // Handle connection changes
  if (!deviceConnected && oldDeviceConnected) {
      delay(500); // Give the stack a chance to get ready
      pServer->startAdvertising(); // Restart advertising
      Serial.println("Start advertising");
      oldDeviceConnected = deviceConnected;
  }
  if (deviceConnected && !oldDeviceConnected) {
      oldDeviceConnected = deviceConnected;
  }

  delay(10); // Adjust as necessary
}
