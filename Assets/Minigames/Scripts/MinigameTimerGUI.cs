using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[AddComponentMenu("Minigames/GUI/Minigame Timer")]
public class MinigameTimeGUI : MonoBehaviour{

    //Instance Management;
    MinigameTimeGUI Instance;

    public MinigameTimeGUI GetInstance(){ return Instance; }

    void Awake(){ Instance = this; }


    ////////////////////////////////////////////////////////////////////////////////////////

    //Inspector References;
    TextMeshProUGUI timeTextMesh;
    [SerializeField] GameObject timesUpGUIReference;

    //Time Checks;
    bool isTimeUp, isCountingDown;
    [SerializeField] float timeAvailable;
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


    void Update(){
        bool isMinigamePlayable = MinigameReference.GetIsMinigameReady() || !MinigameReference.GetHasMinigameEnded();

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
}
