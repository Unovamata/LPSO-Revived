using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LPSOTextManager : MonoBehaviour{
    public enum AnimationType{
        DefaultScaling,
        BouncingScaling,
        ResetAnimation,
        NoAnimation,
    }

    [SerializeField] AnimationType animationState, destinationAnimationState;
    public AnimationType GetAnimationState(){ return animationState; }
    public void SetAnimationState(AnimationType AnimationState){ animationState = AnimationState; }
    public AnimationType GetDestinationAnimationState(){ return destinationAnimationState; }
    public void SetDestinationAnimationState(AnimationType DestinationAnimationState){ destinationAnimationState = DestinationAnimationState; }

    ////////////////////////////////////////////////////////////////////////////////////////


    /* Imagine the leading mesh as the leader of a group, if the leading text
     * does anything, then the other text elements will follow suit, copying
     * everything from color, opacity, and so on; */
    [SerializeField] TextMeshProUGUI leadingMesh;
    public TextMeshProUGUI GetLeadingMesh(){ return leadingMesh; }

    //Child meshes are the list of meshes what will copy the leading mesh;
    [SerializeField] List<TextMeshProUGUI> childMeshes;

    //Text properties;
    Color color;
    [SerializeField] [Range(0, 1)] float opacity = 1;
    [SerializeField] float destinationScale = 1, startScale;
    Vector3 startPosition;
    [SerializeField] Vector3 endPosition;

    public Color GetColor(){ return color; }
    public void SetColor(Color @Color){ color = @Color; }
    public float GetOpacity(){ return opacity; }
    public void SetOpacity(float Opacity){ opacity = Opacity; }
    public float GetDestinationScale(){ return destinationScale; }
    public void SetDestinationScale(float DestinationScale){ destinationScale = DestinationScale; }
    public Vector3 GetStartPosition(){ return startPosition; }
    public void SetStartPosition(Vector3 StartPosition){ startPosition = StartPosition; }
    public Vector3 GetEndPosition(){ return endPosition; }
    public void SetEndPosition(Vector3 EndPosition){ endPosition = EndPosition; }

    RectTransform rectTransform;

    bool isAnimating;

    protected virtual void Start(){
        color = leadingMesh.color;

        rectTransform = GetComponent<RectTransform>();

        //Positioning;
        isAnimating = startPosition == Vector3.zero && endPosition == Vector3.zero;

        startScale = rectTransform.localScale.x;
        startPosition = rectTransform.localPosition;

        foreach(TextMeshProUGUI mesh in childMeshes){
            Color meshColor = mesh.color;
            Color color = new Color(meshColor.r, meshColor.g, meshColor.b, opacity);

            mesh.color = color;
            mesh.text = leadingMesh.text;
        }

        //Text Formatting;
        leadingMesh.color = new Color(color.r, color.g, color.b, opacity);
    }


    ////////////////////////////////////////////////////////////////////////////////////////


    float time = 1f, scaleTime = 0.6f, doubleTime;
    bool triggeredAnimation, triggeredMovement, canAnimate = true;

    protected void Update(){
        switch(animationState){
            case AnimationType.DefaultScaling:
            default:
                rectTransform.localScale = new Vector2(startScale, startScale);

                TriggerAnimation();
            break;

            case AnimationType.BouncingScaling:
                try{
                    if(!triggeredAnimation){
                        LeanTween.scale(gameObject, Vector2.one * destinationScale, scaleTime).setEase(LeanTweenType.easeOutElastic);
                        LeanTween.value(gameObject, 1, 0, 1f).setOnUpdate((float val) => { opacity = val; }).setDelay(OpacityDelayFormula(time * 2)); 
                        triggeredAnimation = true;
                    } 
                } catch { }

                TriggerAnimation();
            break;

            case AnimationType.ResetAnimation:
                //Coloring;
                opacity = 1;

                Color colorReference = leadingMesh.color, newColor = colorReference;
                newColor.a = 1;
                leadingMesh.color = newColor;

                //States;
                triggeredAnimation = false;
                triggeredMovement = false;
                
                rectTransform.localPosition = startPosition;
                animationState = destinationAnimationState;

                TriggerAnimation();
            break;

            case AnimationType.NoAnimation:
                canAnimate = false;
            break;
        }
    }

    void TriggerAnimation(){
        if(canAnimate){
            if(!triggeredMovement){
                LeanTween.moveLocal(gameObject, endPosition, time).setDelay(time * 2f);
                triggeredMovement = true;
            }
        }
    }

    //A formula to create a delay between action based in the input time;
    public static float OpacityDelayFormula(float var) {
        return var - (var * 0.05f);
    }
}