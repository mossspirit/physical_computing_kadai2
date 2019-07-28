using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    float time = 0;
    [SerializeField] float GameOverTime = 60.0f;
    [SerializeField] Text gameTimerText;
    [SerializeField] GameObject gameClearText;
    [SerializeField] GameObject gameOverText;
    public bool isGame = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isGame) {
            GameOverTime -= Time.deltaTime; //ゲームカウントダウン処理
            gameTimerText.text = "Time:" + Mathf.CeilToInt(GameOverTime).ToString();

            if (GameOverTime < 0) {
                GameOver();
            }
        }
    }

    public void GameClear() {
        gameClearText.SetActive(true);
        isGame = true;
    }
    public void GameOver() {
        gameOverText.SetActive(true);
        isGame = true;
    }
}
