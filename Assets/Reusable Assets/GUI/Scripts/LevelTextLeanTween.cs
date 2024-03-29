using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTextLeanTween : MonoBehaviour{
    [SerializeField] GameObject countdownObject, levelObject;


    public int levelNumber = 1;
    private float timer;
    [SerializeField] bool pauseTime = false;
    MinigameType game;

    void Start(){
        //Calling the data needed;
        levelNumber = FruitistaFetchGameManager.Instance.GetCurrentLevel();
        game = MinigameType.Instance;

        try { if(pauseTime) MinigameTimerGUI.Instance.SetIsPaused(true); } catch { } //Pausing time if found;
    }

    //Visual Scale;
    public Vector3 textScale = new Vector3(0.5f, 0.5f, 0.5f);

    //State manager;
    private bool start, countdown = false, manipulateGame = true;
    
    void Update(){
        //If the animation is not set up;
        if (!start) { //Animate;
            TextMeshProUGUI text = levelObject.GetComponent<LPSOTextManager>().GetLeadingMesh();
            text.text = string.Format("Level {0}", Mathf.Clamp(levelNumber, 1, 999));
            //TextAnimations.JumpAndFade(levelObject, textScale, TextAnimations.SCALED);
            //GameManagerType.ActivateScripts(GameManagerType.GetAllComponents(game.gameObject)); //And activate needed scripts;
            start = true;
        } else { //If that's already done, prepare for destruction;
            timer += Time.deltaTime;

            //Manipulate the game at a certain point;
            if(timer > 2 && manipulateGame) { 
                try { if(pauseTime) MinigameTimerGUI.Instance.SetIsPaused(false); } catch { } //Resume time if found;
                try {  //Reset the item counting bars if found;
                    MinigameFillBarGUI.Instance.ResetData();
                } catch { }
                manipulateGame = false;
            }

            //After that, create the countdown; If found;
            if(timer > 2.5f && !countdown){
                try { Instantiate(countdownObject, transform.parent); } catch { }
                countdown = true;
            } else if(timer > 3.5f) Destroy(gameObject); //After that, destroy;
        }
    }
}
