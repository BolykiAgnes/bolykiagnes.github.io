#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <WiFiClient.h>

#define trigPin D4 
#define echoPin D5

long duration;
int distance;

const char* ssid = "xxxxx";
const char* password = "xxxxx";

const char* postServerName = "http://192.168.0.103:5001/api/ParkingLot/change-status";

unsigned long lastTime = 0;
unsigned long timerDelay = 5000;

int occupiedCounter = 0;
int spotStatus = 0;
int freeCounter = 0;

String sensorReadings;

String HTTP_METHOD = "GET";

void setup() {
  Serial.begin(115200); // Starts the serial communication
  pinMode(trigPin, OUTPUT); // Sets the trigPin as an Output
  pinMode(echoPin, INPUT); // Sets the echoPin as an Input
 
  WiFi.begin(ssid, password);
  Serial.println("Connecting");
  while(WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.print("Connected to WiFi network with IP Address: ");
  Serial.println(WiFi.localIP());
}

void loop() {

  while(occupiedCounter != 3) {
    // Clears the trigPin
    digitalWrite(trigPin, LOW);
    delayMicroseconds(2);
    
    // Sets the trigPin on HIGH state for 10 micro seconds
    digitalWrite(trigPin, HIGH);
    delayMicroseconds(10);
    digitalWrite(trigPin, LOW);
    
    // Reads the echoPin, returns the sound wave travel time in microseconds
    duration = pulseIn(echoPin, HIGH);
    
    // Calculating the distance
    distance = duration*0.034/2;
    // Prints the distance on the Serial Monitor
    Serial.print("Distance: ");
    Serial.println(distance);
    
    if(distance < 50) {
      occupiedCounter++;
      freeCounter = 0;
      Serial.print("Occupied counter set to: ");
      Serial.println(occupiedCounter);
      }
      if(distance > 49) {
        freeCounter++;
        occupiedCounter = 0;
        Serial.print("Free counter set to ");
        Serial.println(freeCounter);
        if(freeCounter == 3 && spotStatus != 0) {
          Serial.println("Sending request with free status");
          PostSensorData("{\"spotId\":\"6\",\"isAvailable\":\"true\"}");
          freeCounter = 0;
          spotStatus = 0;
          }
      }
      
      delay(1000);
    }

 
    if(occupiedCounter == 3 && spotStatus != 1) {
      spotStatus = 1;
      Serial.println("Sending request with occupied status");
      PostSensorData("{\"spotId\":\"6\",\"isAvailable\":\"false\"}");
      }
      occupiedCounter = 0;
}
    



  void PostSensorData(String httpRequestData) {
    if(WiFi.status() == WL_CONNECTED) {
      WiFiClient client;
      HTTPClient http;
      
      http.begin(client, postServerName);
      http.addHeader("Content-Type", "application/json");
      int httpResponseCode = http.POST(httpRequestData);

      Serial.print("HTTP Response code: ");
      Serial.println(httpResponseCode);

      http.end();
      }
      else {
        Serial.println("WiFi Disconnected");
        }
    }
