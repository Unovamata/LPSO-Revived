using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManageText : MonoBehaviour{
    //Calling the parent text;
    public GameObject parent;
    TextMeshProUGUI myText, parentText;
    RectTransform myRect, parentRect;

    private void Start() {
        myText = GetComponent<TextMeshProUGUI>();
        myRect = GetComponent<RectTransform>();
        parentText = parent.GetComponent<TextMeshProUGUI>();
        parentRect = parent.GetComponent<RectTransform>();
    }


    // Update is called once per frame
    void Update() {
        if(parent != null){
            //Changing the properties out of the text object;
            myRect.sizeDelta = parentRect.sizeDelta;

            //Setting the properties of the object based on "t";
            myText.text = parentText.text;
            myText.fontSize = parentText.fontSize;
            Color col = myText.color;
            myText.color = new Color(col.r, col.g, col.b, parentText.color.a); //Fading in and out;
            myText.characterSpacing = parentText.characterSpacing;
            myRect.localPosition = parentRect.localPosition;
        } else Destroy(gameObject);
    }
}
