using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LPSOText : MonoBehaviour{
    public GameObject whiteText; //Leading text;
    private TextMeshProUGUI whiteMesh;
    private Color color;
    [HideInInspector] public float opacity = 1;
    TextMeshProUGUI mesh;

    void Start(){
        mesh = GetComponent<TextMeshProUGUI>();
        whiteMesh = whiteText.GetComponent<TextMeshProUGUI>();
        color = whiteMesh.color;
    }

    private void Update() {
        whiteMesh.color = new Color(color.r, color.g, color.b, opacity);
        mesh.text = whiteMesh.text;
    }
}
