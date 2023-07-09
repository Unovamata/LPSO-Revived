using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameHighscoreMedalGUI : MonoBehaviour{
    //Instance Management;
    public static MinigameHighscoreMedalGUI Instance;

    void Awake(){ Instance = this; }


    ////////////////////////////////////////////////////////////////////////////////////////


    [SerializeField] TextMeshProUGUI highscoreTextMesh;
    int highscore = 0;

    public int GetHighscore(){ return highscore; }
    public void SetHighscore(int Highscore){ highscore = Highscore; }

    void Start(){
        MinigameSO minigameSO = MinigameType.Instance.GetMinigameSOController();
        highscore = minigameSO.GetHighScore();
        highscoreTextMesh.text = highscore.ToString();
    }

    void Update(){
        highscoreTextMesh.text = highscore.ToString();
    }
}
