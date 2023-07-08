using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameFillBarGUI : MonoBehaviour{
    //Instance Management;
    public static MinigameFillBarGUI Instance;

    void Awake(){ Instance = this; }


    ////////////////////////////////////////////////////////////////////////////////////////

    [SerializeField] Image backgroundBar, progressBar;
    public RectTransform backgroundBarRect, progressBarRect;
    public TextMeshProUGUI currentItemsTextMesh;

    void SetTextInTextMesh(string input){ currentItemsTextMesh.text = input; }

    float barWidth = 0, barHeight = 0;

    void Start(){
        //Getting the rect transforms;
        backgroundBarRect = backgroundBar.rectTransform;
        progressBarRect = progressBar.rectTransform;

        barWidth = backgroundBarRect.rect.width;
        barHeight = backgroundBarRect.rect.height;
        SetTextInTextMesh("0");
    }
    

    ////////////////////////////////////////////////////////////////////////////////////////


    int maxItemsInBar = 0, currentItemsInBar = 0;
    public bool isRefreshingData = false;

    public int GetMaxItemsInBar(){ return maxItemsInBar; }
    public void SetMaxItemsInBar(int MaxItemsInBar){ maxItemsInBar = MaxItemsInBar; }
    public int GetCurrentItemsInBar(){ return currentItemsInBar; }
    public void SetCurrentItemsInBar(int CurrentItemsInBar){ currentItemsInBar = CurrentItemsInBar; }
    public bool GetIsRefreshingData(){ return isRefreshingData; }
    public void SetIsRefreshingData(bool IsRefreshingData){ isRefreshingData = IsRefreshingData; }
    public void RefreshData(){ isRefreshingData = true; }
    public void ResetData(){ 
        isRefreshingData = true;
        currentItemsInBar = 0;
    }


    //Animation;
    float animationSpeed = 7, animationScale = 0;
    
    void Update(){
        SetTextInTextMesh(currentItemsInBar.ToString());

        if (isRefreshingData) {
            animationScale = barWidth / maxItemsInBar;
            isRefreshingData = false;
        }
        
        progressBarRect.sizeDelta = Vector2.Lerp(progressBarRect.sizeDelta, 
            new Vector2(animationScale * currentItemsInBar, barHeight), 
            animationSpeed * Time.deltaTime);
    }
}
