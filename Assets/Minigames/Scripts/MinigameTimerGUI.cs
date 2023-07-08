using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[AddComponentMenu("Minigames/GUI/Minigame Timer")]
public class MinigameTimerGUI : MonoBehaviour{

    //Instance Management;
    public static MinigameTimerGUI Instance;

    void Awake(){ Instance = this; }


    ////////////////////////////////////////////////////////////////////////////////////////

    //Inspector References;
    TextMeshProUGUI timeTextMesh;
    [SerializeField] GameObject timesUpGUIReference;

    //Time Checks;
    bool isTimeUp, isCountingDown;
    [SerializeField] float timeAvailable;
    public void AddTime(float input){ timeAvailable += input; }
    public bool IsThereAnyTime(){ return timeAvailable > 0; }

    [HideInInspector] float originalTimeAvailable;
    MinigameType MinigameReference;

    

    void Start(){
        TimerTextFormatTime();
        originalTimeAvailable = timeAvailable;
        MinigameReference = MinigameType.Instance;
    }

    //Time text gets formatted to a specific notation to be shown in the GUI;
    private void TimerTextFormatTime() {
        timeAvailable = Mathf.Clamp(timeAvailable, 0, 999);

        float minutes = Mathf.FloorToInt(timeAvailable / 60);
        float seconds = Mathf.FloorToInt(timeAvailable % 60);
        timeTextMesh.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    ////////////////////////////////////////////////////////////////////////////////////////

    bool isPaused;

    public bool GetIsPaused(){ return isPaused; }
    public void SetIsPaused(bool IsPaused){ isPaused = IsPaused; }

    void Update(){
        bool isMinigamePlayable = MinigameReference.GetIsMinigameReady() || !MinigameReference.GetHasMinigameEnded();

        if(!isMinigamePlayable) return;

        if(isTimeUp){
            return;
        } else {
            Instantiate(timesUpGUIReference);
            MinigameReference.SetHasMinigameEnded(true);
        }

        if(timeAvailable > 0) timeAvailable -= Time.deltaTime;

        TimerTextFormatTime();
    }


    ////////////////////////////////////////////////////////////////////////////////////////


    public void TimerReset(){
        timeAvailable = originalTimeAvailable;
        isTimeUp = false;
        MinigameReference.SetHasMinigameEnded(false);
    }

    public static void TimerReset(MinigameType minigame){
        MinigameTimerGUI timer = MinigameTimerGUI.Instance;

        timer.timeAvailable = timer.originalTimeAvailable;
        timer.isTimeUp = false;
        minigame.SetHasMinigameEnded(false);
    }
}
