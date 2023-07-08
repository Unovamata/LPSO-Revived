using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownMinigames : MonoBehaviour{
    //-------------------------------------------------------------------------------------

    
    [SerializeField] GameObject three, two, one, go;
    [SerializeField] AudioClip[] sounds;
    public bool countdownStarted = false;

    float timer = 0, countdownSoundsTimer = 0, cooldown = 5f;
    float[] wait = new float[4];
    int indexWait = 0;

    private void Start() {
        for(int i = 0; i < 4; i++) {
            wait[i] = TextAnimations.SCALED * i;
        }
    }

    private void Update() {
        countdownSoundsTimer += Time.deltaTime;
        try {
            if(countdownSoundsTimer > wait[indexWait]) {
                MinigameType.Instance.PlaySFX(sounds[indexWait]);
                indexWait++;
            }
        } catch { }
        

        if (!countdownStarted) {
            Countdown(three, 0);
            Countdown(two, 1);
            Countdown(one, 2);
            Countdown(go, 3);
            countdownStarted = true;
        } else {
            timer += Time.deltaTime;
            if (timer > 4f) MinigameType.Instance.SetIsMinigameReady(true);
            if(timer > TextAnimations.SCALED * cooldown) {
                Destroy(gameObject);
            }
        }
    }

    //Countdown(); VARIABLET = time, VARIABLED = delay;
    private void Countdown(GameObject lean, int wait) {
        float delay = TextAnimations.SCALED * wait, delayMove = delay + 1.2f;

        //Resetting the size of the text while saving its scale destination;
        LeanTween.scale(lean, Vector3.one, TextAnimations.SCALED).setEase(LeanTweenType.easeOutElastic).setDelay(delay);
        LeanTween.moveLocal(lean, new Vector3(0, 50, 0), TextAnimations.MOVET).setDelay(delayMove);
        //Try to locate the TextMeshPro component;
        try {
            LPSOText text = lean.GetComponent<LPSOText>();
            LeanTween.value(lean, 1, 0, 1f).setOnUpdate((float val) => { text.opacity = val; }).setDelay(TextAnimations.OpacityDelay(delayMove));
        }
        catch { }
    }
}
