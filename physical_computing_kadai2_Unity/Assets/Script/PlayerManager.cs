using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerManager : MonoBehaviour {

    [SerializeField] GameObject handLight;
    [SerializeField] int speed = 3;
    [SerializeField] float MaxLightTime = 10;
    [SerializeField] GameObject yazirusi;
    GameManager gameManager;
    SerialMain serialMain;
    Animator anim;
    float lighttime;
    int SendValue;
    int old_SendValue;
    byte[] buf = new byte[1];
	void Start () {
        anim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        serialMain = GameObject.Find("GameManager").GetComponent<SerialMain>();

        buf[0] = 4;
        serialMain.Write(buf);
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            buf[0] = 0;
            serialMain.Write(buf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            buf[0] = 1;
            serialMain.Write(buf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            buf[0] = 2;
            serialMain.Write(buf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            buf[0] = 3;
            serialMain.Write(buf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            buf[0] = 4;
            serialMain.Write(buf);
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            HandLight();
        }
        if (Input.GetKey(KeyCode.W)) {
            Run();
        } else {
            anim.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            TurnRight();
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            TurnLeft();
        }
        if (handLight.activeInHierarchy) {
            
            lighttime += Time.deltaTime;
            if (lighttime > MaxLightTime)
                HandLight();
        }
        SendValue = Mathf.CeilToInt(Map(Mathf.Clamp(MaxLightTime - lighttime, 0, MaxLightTime), MaxLightTime, 0, 4, 0));
        if(SendValue != old_SendValue) {

            Debug.Log("Old:" + old_SendValue + " New" + SendValue);
            buf[0] = (byte)SendValue;
            serialMain.Write(buf);
            old_SendValue = SendValue;
        }
	}
    public void Run() {
        anim.SetBool("Run", true);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);  
    }
    public void TurnRight() {
        float angle = Mathf.LerpAngle(0, 90, Time.time);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + angle, 0);
    }
    public void TurnLeft() {
        float angle = Mathf.LerpAngle(0, -90, Time.time);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + angle, 0);
    }
    public void HandLight() {
        if (handLight.activeInHierarchy || lighttime > MaxLightTime) {
            handLight.SetActive(false);
            yazirusi.SetActive(false);
        } else {
            handLight.SetActive(true);
            yazirusi.SetActive(true);
        }
    }

    float Map(float value, float start1, float stop1, float start2, float stop2) {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Goal") {
            Debug.Log("Goal!!");
            gameManager.GameClear();
        }
    }
}
