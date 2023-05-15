using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameTimeGUI : MonoBehaviour{
    public TextMeshProUGUI timer;
    public GameObject timesUpGUI;
    [HideInInspector] public bool timesUp = false, canCount = true;
    public float timeAvailable = 0;
    [HideInInspector] public float originalTime = 0;
    GameManagerType gameType;

    private void Start() {
        LoadTime();
        originalTime = timeAvailable;
        gameType = GameManagerType.Instance;
    }

    // Update is called once per frame
    void Update(){
        if(gameType.isReady){
            if(canCount) {
                if(timeAvailable > 0) timeAvailable -= Time.deltaTime;
                else {
                    if(!timesUp) {
                        Instantiate(gameType.transition, gameType.canvas);
                        Instantiate(timesUpGUI);
                        gameType.transitionEnd = false;
                        gameType.gameEnd = true;
                        timesUp = true;
                    }
                }
                LoadTime();
            }
        } else timeAvailable = originalTime;
    }

    private void LoadTime() {
        timeAvailable = Mathf.Clamp(timeAvailable, 0, 999);

        float minutes = Mathf.FloorToInt(timeAvailable / 60);
        float seconds = Mathf.FloorToInt(timeAvailable % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
