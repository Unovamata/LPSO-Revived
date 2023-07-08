using UnityEngine;
using TMPro;

public class MinigameScoreGUI : MonoBehaviour{
    //Instance Management;
    public static MinigameScoreGUI Instance;

    void Awake(){ Instance = this; }


    ////////////////////////////////////////////////////////////////////////////////////////

    [SerializeField] TextMeshProUGUI scoreTextMesh;
    [HideInInspector] int currentScore;

    public int GetCurrentScore(){ return currentScore; }
    public void SetCurrentScore(int CurrentScore){ currentScore = CurrentScore; }

    // Update is called once per frame
    void Update(){
        scoreTextMesh.text = currentScore.ToString();
    }
}
