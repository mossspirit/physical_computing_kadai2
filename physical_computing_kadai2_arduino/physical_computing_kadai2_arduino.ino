#define btn1 6
#define btn2 7
#define btn3 8
#define btn4 12
#define FADE_TIME 2

int incomingByte = 0;
int led_pins[] = {5, 9, 10, 11};
int btn_pins[] = {6, 7, 8, 12};
int status[4];
int btn2_old;
int btn3_old;
int btn4_old;
void setup(){
  Serial.begin(9600);
  for (int i=0; i<4; i++) {
    pinMode(btn_pins[i], INPUT);
    pinMode(led_pins[i], OUTPUT);
  }
}
void loop(){
  if (Serial.available() > 0) {
    // 受信したデータの1バイトを読み取る
    incomingByte = Serial.read();
  }
  LedManager();
  for(int k=0; k<4; k++){
    status[k] = digitalRead(btn_pins[k]);
  }
  if(status[0] == HIGH){
    Serial.println("Run");
  }
  if(status[1] == HIGH && btn2_old == LOW){
    Serial.println("TurnLeft");
    delay(300);
  }
  btn2_old = status[1];
  if(status[2] == HIGH && btn3_old == LOW){
    Serial.println("TurnRight");
    delay(300);
  }
  btn3_old = status[2];
  if(status[3] == HIGH && btn4_old == LOW){
    Serial.println("Light");
    delay(300);
  }
  btn4_old = status[3];
}

void LedManager(){
  for(int i=0; i<4; i++){
    digitalWrite(led_pins[i], LOW);
  }
  for(int i=0; i<incomingByte; i++){
    digitalWrite(led_pins[i], HIGH);
  }
}

