#include <WiFi.h>
#include <HTTPClient.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include <ArduinoJson.h>

LiquidCrystal_I2C LCD = LiquidCrystal_I2C(0x27, 16, 2);

unsigned long startTime;
bool timerRunning = false;
int crewId = 0;

void spinner() {
  static int8_t counter = 0;
  const char* glyphs = "\xa1\xa5\xdb";
  LCD.setCursor(15, 1);
  LCD.print(glyphs[counter++]);
  if (counter == strlen(glyphs)) {
    counter = 0;
  }
}

void startTimer() {
  startTime = millis();
  timerRunning = true;
  LCD.clear();
  LCD.setCursor(0, 0);
  LCD.print("Timer started");
}

void stopTimer() {
  if (timerRunning) {
    unsigned long elapsedTime = millis() - startTime;
    timerRunning = false;

    LCD.clear();
    LCD.setCursor(0, 0);
    LCD.print("Time taken:");
    LCD.setCursor(0, 1);
    LCD.print(elapsedTime / 1000.0, 2); // Display time in seconds

    delay(2000);
    
    sendResultToServer(crewId, elapsedTime);
  }
}

void sendResultToServer(int crewId, unsigned long timeTaken) {
  HTTPClient http;
  http.begin("https://localhost:7178.azurewebsites.net/api/Result"); // Change URL to your local server
  //https://azureveb20240521093611.azurewebsites.net/api/Result
  http.addHeader("Content-Type", "application/json");

  // Forming JSON
  DynamicJsonDocument doc(1024);
  doc["CrewId"] = crewId;
  doc["TimeTaken"] = timeTaken / 1000.0; // Convert to seconds

  String requestBody;
  serializeJson(doc, requestBody);

  // Sending POST request
  int httpResponseCode = http.POST(requestBody);

  if (httpResponseCode > 0) {
    if (httpResponseCode == HTTP_CODE_OK) {
      String response = http.getString();
      Serial.println("Response: " + response);
      LCD.clear();
      LCD.setCursor(0, 0);
      LCD.print("Result sent");
    }
  } else {
    Serial.println("Error sending POST request");
    LCD.clear();
    LCD.setCursor(0, 0);
    LCD.print("Error sending");
  }

  http.end();
}

void setup() {
  Serial.begin(115200);

  LCD.init();
  LCD.backlight();
  LCD.setCursor(0, 0);
  LCD.print("Connecting to ");
  LCD.setCursor(0, 1);
  LCD.print("WiFi ");

  WiFi.begin("Wokwi-GUEST", "", 6);
  while (WiFi.status() != WL_CONNECTED) {
    delay(250);
    spinner();
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());

  LCD.clear();
  LCD.setCursor(0, 0);
  LCD.println("Online");
  LCD.setCursor(0, 1);
  LCD.println("Ready");

  // Simulate getting CrewId from the user
  Serial.println("Enter CrewId:");
  while (Serial.available() == 0) {} // Wait for input
  crewId = Serial.parseInt();
  Serial.print("CrewId: ");
  Serial.println(crewId);

  // Start the timer
  startTimer();
}

void loop() {
  // Logic to stop the timer through button press or other trigger
  if (Serial.available()) { // Serial monitor used to simulate stop trigger
    char command = Serial.read();
    if (command == 's') {
      stopTimer();
    }
  }

  delay(250);
}
