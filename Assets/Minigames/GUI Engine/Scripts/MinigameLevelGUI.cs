using UnityEngine;
using TMPro;

public class MinigameLevelGUI : MonoBehaviour{
    //Instance Management;
    public static MinigameLevelGUI Instance;

    void Awake(){ Instance = this; }


    ////////////////////////////////////////////////////////////////////////////////////////

    [SerializeField] TextMeshProUGUI scoreTextMesh;
    [SerializeField] int currentLevel = 1;

    public int GetCurrentLevel(){ return currentLevel; }
    public void SetCurrentLevel(int CurrentLevel){ currentLevel = CurrentLevel; }

    // Update is called once per frame
    void Update(){
        scoreTextMesh.text = currentLevel.ToString();
    }
}
