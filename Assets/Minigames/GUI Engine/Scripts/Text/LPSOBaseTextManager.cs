using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LPSOBaseTextManager : MonoBehaviour{
    /* Imagine the leading mesh as the leader of a group, if the leading text
     * does anything, then the other text elements will follow suit, copying
     * everything from color, opacity, and so on; */
    [SerializeField] TextMeshProUGUI leadingMesh;
    public TextMeshProUGUI GetLeadingMesh(){ return leadingMesh; }

    //Child meshes are the list of meshes what will copy the leading mesh;
    [SerializeField] List<TextMeshProUGUI> childMeshes;

    //Text properties;
    Color color;
    [Range(0, 1)] float opacity;

    public Color GetColor(){ return color; }
    public void SetColor(Color @Color){ color = @Color; }
    public float GetOpacity(){ return opacity; }
    public void SetOpacity(float Opacity){ opacity = Opacity; }

    protected virtual void Start(){
        color = leadingMesh.color;
    }

    protected void Update(){
        leadingMesh.color = new Color(color.r, color.g, color.b, opacity);

        foreach(TextMeshProUGUI mesh in childMeshes){
            Color meshColor = mesh.color;
            Color color = new Color(meshColor.r, meshColor.g, meshColor.b, opacity);

            mesh.color = color;
            mesh.text = leadingMesh.text;
        }
    }
}

public class LPSOTextManager : LPSOBaseTextManager{
    public Vector3 textScale = new Vector3(0.5f, 0.5f, 0.5f);
    public string newText;
    bool updated = false;
    float timer = 0;
    public float drawTime = 0.5f;


    ////////////////////////////////////////////////////////////////////////////////////////


    void Update(){
        base.Update();

        /*if (!updated) {
            TextMeshProUGUI text = textObject.GetComponent<LPSOText>().whiteText.GetComponent<TextMeshProUGUI>();
            text.text = newText; //Replace the old text;
            TextAnimations.JumpAndFadeGeneral(textObject, textScale, drawTime);
            updated = true;
        } else {
            timer += Time.deltaTime;
            if(timer > 3.5f) Destroy(gameObject);
        }*/
    }
}
