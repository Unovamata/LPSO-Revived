using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameBarFiller : MonoBehaviour{
    [HideInInspector] public static MinigameBarFiller Instance;
    public RectTransform BlueBar, GreenBar;
    public int maxItems = 0, currentItems = 0;
    public TextMeshProUGUI textMesh;
    public bool reset = false, canReset = true;

    //Animation;
    float animationSpeed = 7, animationScale = 0;
    
    void Update(){
        textMesh.text = currentItems.ToString();

        if (reset) {
            animationScale = BlueBar.rect.width / maxItems;
            reset = false;
        }
        
        GreenBar.sizeDelta = Vector2.Lerp(GreenBar.sizeDelta, new Vector2(animationScale * currentItems, GreenBar.rect.height), animationSpeed * Time.deltaTime);
    }
}
