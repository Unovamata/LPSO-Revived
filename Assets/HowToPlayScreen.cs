using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayScreen : MonoBehaviour{
    [HideInInspector] public static HowToPlayScreen Instance;

    private void Awake() {
        Instance = this;
    }
    
    [SerializeField] RectTransform transitionObject;
    RectTransform myRect;
    GameObject rectGo;
    [HideInInspector] public GameObject parentObject;
    Vector2 startObject, pos;
    bool startAnimation = false, animationType = false, mousePressed = false, startScreenPassed = false; //False = In, True = out;

    private void Start() {
        myRect = GetComponent<RectTransform>();
        rectGo = myRect.gameObject;
        startObject = transitionObject.localPosition;
        pos = myRect.localPosition;
        gameObject.SetActive(false);
        parentObject = gameObject.transform.parent.gameObject;
        parentObject.SetActive(false);
        //LeanTween.moveY(myRect.gameObject, 1f, 2).setEaseOutCirc();
    }

    void Update(){
        //Continue only if the transition is equals to in;
        if(Input.GetMouseButtonDown(0) && !animationType) {
            animationType = true; //From In to Out;
            startAnimation = false;
            mousePressed = true;
        }

        if(!startAnimation) {
            switch(animationType) {
                case true: //In;
                    if(Input.GetMouseButtonDown(0)) {
                        LeanTween.cancel(rectGo);
                        LeanTween.moveY(rectGo, -10f, 2).setEaseOutCirc();
                    }
                break;

                case false: //Out;
                    LeanTween.moveY(rectGo, 1f, 2).setEaseOutCirc();
                break;
            }

            startAnimation = true;
        }

        //Letting the animation loop;
        if(animationType && !LeanTween.isTweening(rectGo) && mousePressed) {
            ResetHowToPlay();
        }

        //print(animationType);

        transitionObject.localPosition = new Vector2(pos.x, startObject.y - (pos.y - myRect.localPosition.y));
    }

    private void ResetHowToPlay() {
        startAnimation = false;
        animationType = false;
        mousePressed = false;
        LeanTween.cancel(rectGo);
        gameObject.SetActive(false);
        parentObject.SetActive(false);
    }
}
