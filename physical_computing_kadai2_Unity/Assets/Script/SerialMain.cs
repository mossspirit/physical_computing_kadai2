using System;
using System.IO.Ports;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public class SerialMain : MonoBehaviour {

    public string portName;
    public int baurate;
    SerialPort serial;
    [NonSerialized]
    public bool isLoop = true;
    [SerializeField] PlayerManager playerManager;
    string str;
    GameManager game;

    void Start() {
        this.serial = new SerialPort(portName, baurate, Parity.None, 8, StopBits.One);
        game = GetComponent<GameManager>();
        try {
            this.serial.Open();
            Scheduler.ThreadPool.Schedule(() => ReadData()).AddTo(this);
        } catch (Exception e) {
            Debug.Log("can not open serial port");
        }
    }
    void Update() {
        if(SceneManager.GetActiveScene().name == "Main") {
            if (str == "Run") {
                playerManager.Run();
                str = "";
            } else if (str == "TurnLeft") {
                playerManager.TurnLeft();
                str = "";
            } else if (str == "TurnRight") {
                playerManager.TurnRight();
                str = "";
            } else {

            }
            if (str == "Light") {
                playerManager.HandLight();
                str = "";
                if (game.isGame) {
                    SceneManager.LoadScene("Title");
                    Scene();
                    str = "";
                }
            }
        } else {
            if (str == "Light") {
                SceneManager.LoadScene("Main");
                Scene();
                str = "";
            }
        }
    }

    private void ReadData() {
        while (this.isLoop && this.serial != null && this.serial.IsOpen) {
            try {
                str = this.serial.ReadLine();
            } catch (System.Exception e) {
            }
        }
    }
    public void Write(byte[] buffer) {
        try {
            this.serial.Write(buffer, 0, 1);
        } catch (System.Exception e) {
            Debug.LogWarning(e.Message);
        }
    }

    public void Scene() {
        this.isLoop = false;
        try {
            this.serial.Close();
        } catch (System.Exception e) {
            Debug.Log(e.Message);
        }

        Debug.Log("SerialClosed");
    }

    float Map(float value, float start1, float stop1, float start2, float stop2) {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    private void OnApplicationQuit() {
        this.isLoop = false;
        try {
            this.serial.Close();
        } catch (System.Exception e) {
            Debug.Log(e.Message);
        }

        Debug.Log("SerialClosed");
    }
}