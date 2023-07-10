using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITextLeanTween : MonoBehaviour{
    [SerializeField] List<MonoBehaviour> components;
    public Vector3 textScale = new Vector3(0.5f, 0.5f, 0.5f);
    bool animated = false;
    float timer;
    public AudioClip soundToPlay;
    MinigameType gameType;

    private void Start() {
        gameType = MinigameType.Instance;
        try { gameType.PlaySFX(soundToPlay); } catch { }
    }


    // Update is called once per frame
    void Update(){
        if (!animated) {
            //TextAnimations.JumpAndFade(gameObject, textScale, TextAnimations.SCALED);
            //GameManagerType.ActivateScripts(GameManagerType.GetAllComponents(GameManagerType.Instance.gameObject));
            animated = true;
        } else {
            timer += Time.deltaTime;
            if (timer > 5f) Destroy(transform.parent.gameObject);
        }

    }
}
