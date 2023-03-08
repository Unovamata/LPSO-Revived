using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkeletonSO", menuName = "ScriptableObjects/SkeletonData", order = 1)]
public class SkeletonSO : ScriptableObject {
    public List<Limb> limbs = new List<Limb>();
}

public class Limb {
    public Texture2D limbTexture;
    public string partName;
    public Vector4 coordinates;
    public Vector2 pivot;
}