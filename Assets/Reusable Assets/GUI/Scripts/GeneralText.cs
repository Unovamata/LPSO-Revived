using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneralText : MonoBehaviour{
    [SerializeField] GameObject textObject;
    public Vector3 textScale = new Vector3(0.5f, 0.5f, 0.5f);
    public string newText;
    bool updated = false;
    float timer = 0;
    public float drawTime = 0.5f;

    void Update(){
        if (!updated) {
            TextMeshProUGUI text = textObject.GetComponent<LPSOText>().whiteText.GetComponent<TextMeshProUGUI>();
            text.text = newText; //Replace the old text;
            TextAnimations.JumpAndFadeGeneral(textObject, textScale, drawTime);
            updated = true;
        } else {
            timer += Time.deltaTime;
            if(timer > 3.5f) Destroy(gameObject);
        }
    }
}
