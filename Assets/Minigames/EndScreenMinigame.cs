using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardMinigame : MonoBehaviour{
    // All the text meshes that are a darker blue go in this list;
    public List<TextMeshProUGUI> scoresTextMeshes = new List<TextMeshProUGUI>();
    public TextMeshProUGUI totalScoreTextMesh, youEarnedTextMesh, totalTextMesh;
    int kibbleInHand, kibbleObtained, minigameTotalScore, kibbleDivisor;

    /*public List<int> scores = new List<int>();
    public List<TextMeshProUGUI> scoresTextMeshes = new List<TextMeshProUGUI>();
    public TextMeshProUGUI totalScoreTextMesh, kibbleObtainedTextMesh, totalKibbleTextMesh, highscoreTextMesh;
    public int kibbleInHand, totalScore, kibbleObtained;
    public int kibbleDivisor;

    // Start is called before the first frame update
    void Start(){
        ProcessScores();
    }

    public static void ProcessScores() {
        MinigameSO controller = GameManagerType.Instance.controller;
        EndScreenMinigame leaderboard = GameManagerType.Instance.resultsScreen;
        leaderboard.totalScore = 0;
        leaderboard.kibbleInHand += controller.kibble;
        int i = 0;

        //Drawing the scores in the table;
        foreach (TextMeshProUGUI texts in leaderboard.scoresTextMeshes){
            leaderboard.totalScore += leaderboard.scores[i];
            texts.text = GameManagerType.FormatDecimals(leaderboard.scores[i]) + " Pts";
            i++;
        }

        //Calculating kibble;
        leaderboard.kibbleObtained = Mathf.CeilToInt(leaderboard.totalScore / leaderboard.kibbleDivisor);
        leaderboard.kibbleInHand += leaderboard.kibbleObtained;

        //Drawing the info in the table;
        leaderboard.totalScoreTextMesh.text = GameManagerType.FormatDecimals(leaderboard.totalScore) + " Pts";
        leaderboard.kibbleObtainedTextMesh.text = GameManagerType.FormatDecimals(leaderboard.kibbleObtained);
        leaderboard.totalKibbleTextMesh.text = GameManagerType.FormatDecimals(leaderboard.kibbleInHand);
        leaderboard.highscoreTextMesh.text = GameManagerType.FormatDecimals(controller.highscore);
    }*/
}
