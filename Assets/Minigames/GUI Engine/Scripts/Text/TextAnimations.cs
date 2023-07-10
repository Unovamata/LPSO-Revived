using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnimations{
    public static float OpacityDelay(float var) {
        return var - (var * 0.05f);
    }

    /*public static float ArraySum(int[] array) {
        int sum = 0;
        for(int i = 0; i < array.Length; i++) sum += array[i];
        return sum;
    }*/

    //VARIABLET = time, VARIABLED = delay;
    public const float SCALET = 0.6f, SCALED = 1.3f, MOVET = 1f;

    //JumpAndFade();
    public static void JumpAndFade(GameObject lean, Vector3 scale, float time) {
        float y = lean.transform.localPosition.y + (scale.x * 50);
        float delayMove = time * 2;

        LeanTween.scale(lean, scale, SCALET).setEase(LeanTweenType.easeOutElastic);
        LeanTween.moveLocal(lean, new Vector3(0, y, 0), MOVET).setDelay(delayMove);

        //Try to locate the TextMeshPro component;
        try{
            LPSOTextManager text = lean.GetComponent<LPSOTextManager>();
            LeanTween.value(lean, 1, 0, 1f).setOnUpdate((float val) => { text.SetOpacity(val); }).setDelay(OpacityDelay(delayMove)); 
        } catch { }
    }

    //JumpAndFade();
    public static void JumpAndFadeGeneral(GameObject lean, Vector3 scale, float time) {
        float y = lean.transform.localPosition.y + 50;
        float delayMove = time * 2;

        LeanTween.scale(lean, scale, SCALET).setEase(LeanTweenType.easeOutElastic);
        LeanTween.moveLocal(lean, new Vector3(0, y, 0), MOVET).setDelay(delayMove);

        //Try to locate the TextMeshPro component;
        try{
            LPSOTextManager text = lean.GetComponent<LPSOTextManager>();
            LeanTween.value(lean, 1, 0, 1f).setOnUpdate((float val) => { text.SetOpacity(val); }).setDelay(OpacityDelay(delayMove)); 
        } catch { }
    }

    //Jump();
    public static void Jump(GameObject lean, Vector3 scale) {
        LeanTween.scale(lean, scale, SCALET).setEase(LeanTweenType.easeOutElastic);
    }

    //Jump();
    public static void JumpDelay(GameObject lean, Vector3 scale, float delay) {
        LeanTween.scale(lean, scale, SCALET).setEase(LeanTweenType.easeOutElastic).setDelay(delay);
    }

    //ResetTextParameters();
    public static void ResetTextParameters(GameObject lean, Vector3 scale) {
        //Adjusting the opacity;
        TextMeshProUGUI text = lean.GetComponent<LPSOTextManager>().GetLeadingMesh();
        Color col = text.color, newCol = col;
        newCol.a = 1;
        text.color = newCol;

        //Changing the text's size;
        Jump(lean, scale);
    }

    public static IEnumerator GameIsReady(int waits) {
        yield return new WaitForSeconds(waits * (SCALED + SCALED * 2 + OpacityDelay(SCALED)));
        Debug.Log("Game ready!");
        yield return true;
    }
}