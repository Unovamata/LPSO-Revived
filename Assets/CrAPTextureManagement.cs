using pumpkin.display;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.U2D.IK;
using UnityEditor;

public class CrAPTextureManagement : MonoBehaviour{
    public static CrAPTextureManagement Instance;

    private void Awake() {
        Instance = this;
    }

    //Data;
    public SkeletonSO skeleton;
    public Texture2D skeletonData;
    [HideInInspector] public Texture2D temporalTexture;
    [SerializeField] Texture2D limbTexture;

    //Unity Editor;
    Material material;
    MeshRenderer mesh;
    [HideInInspector] public int x, y, w, h;
    public Limb limb;

    //Callers;
    string limbName = "Head";

    // Start is called before the first frame update
    void Start() {
        mesh = GetComponent<MeshRenderer>();
        material = mesh.material;
        LoadSkeletonData("SkeletonData");

        //Texture to edit;
        temporalTexture = skeletonData;
        limb = CallLimb(limbName);
        Vector4 coordinates = limb.coordinates;
        x = (int)coordinates.x;
        y = (int)coordinates.y;
        w = (int)coordinates.z;
        h = (int)coordinates.w;

        AssetDatabase.CreateAsset(mesh, "Mesh");
        AssetDatabase.SaveAssets();
    }

    // Update is called once per frame
    void Update(){
        material.mainTexture = temporalTexture;
        mesh.material = material;
    }

    private void LoadSkeletonData(string route) {
        //Sprite has to be located in the "Resources" folder;
        UnityEngine.Sprite[] sprites = Resources.LoadAll<UnityEngine.Sprite>(route);
        skeleton.limbs = new List<Limb>();

        //Extracting limb data;
        foreach(UnityEngine.Sprite sprite in sprites) {
            //Formatting the limb;
            Limb limb = new Limb();
            limb.partName = sprite.name;
            int w = (int) sprite.rect.width, h = (int) sprite.rect.height;

            limb.coordinates = new Vector4(sprite.rect.x, sprite.rect.y, w, h);
            /* 0: X; Where the sprite box starts;
             * 1: Y; Where the sprite box ends;
             * 2: Width; Size of the texture;
             * 3: Height; Size of the texture; */

            //Sprite center;
            limb.pivot = new Vector2(sprite.pivot.x / w, sprite.pivot.y / h);

            //Generating the textures;
            Texture2D texture = new Texture2D(w, h);
            Rect rect = sprite.textureRect;
            Color[] pixels = sprite.texture.GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);

            texture.SetPixels(pixels);
            texture.Apply();
            limb.limbTexture = texture;

            //Saving the data;
            skeleton.limbs.Add(limb);
        }
    }

    private Texture2D CloneTexture(Texture2D original) {
        Texture2D clone = new Texture2D(original.width, original.height);
        clone.SetPixels(original.GetPixels());
        return clone;
    }

    //Returning a limb by calling its name;
    private Limb CallLimb(string name) {
        return skeleton.limbs.Find(limbSearcher => limbSearcher.partName == name);
    }

    public void ClearTextureAt(int x, int y, int w, int h) {
        Color[] c = FillArrayColor(Color.clear, w, h);
        temporalTexture.SetPixels(x, y, w, h, c);
        temporalTexture.Apply();
    }

    public void PasteTexture(Limb limb, Texture2D skeletonTexture) {
        Vector4 coordinates = limb.coordinates;
        int x = (int)coordinates.x, y = (int)coordinates.y, w = (int)coordinates.z, h = (int)coordinates.w;
        Color[] colors = limb.limbTexture.GetPixels();
        skeletonTexture.SetPixels(x, y, w, h, colors);
        skeletonTexture.Apply();
    }

    public static Color[] FillArrayColor(Color color, int w, int h) {
        return Enumerable.Repeat(color, w * h).ToArray();
    }
}

//Custom Editor;
[CustomEditor(typeof(CrAPTextureManagement))]
public class CustomInspector : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        CrAPTextureManagement manager = (CrAPTextureManagement) target; //Referencing the script;

        EditorGUILayout.LabelField("Texture Management");

        foreach(Limb limb in manager.skeleton.limbs) {
            string name = limb.partName;
            Vector4 coordinates = limb.coordinates;
            int x = (int)coordinates.x;
            int y = (int)coordinates.y;
            int w = (int)coordinates.z;
            int h = (int)coordinates.w;

            if(GUILayout.Button("Delete " + name)) {
                manager.ClearTextureAt(x, y, w, h);
            }

            if(GUILayout.Button("Paste " + name)) {
                manager.PasteTexture(limb, manager.skeletonData);
            }
        }


    }
}

/*if(GUILayout.Button("Delete " + name)) {
                manager.ClearTextureAt(manager.x, manager.y, manager.w, manager.h);
            }

            if(GUILayout.Button("Paste " + name)) {
                manager.PasteTexture(manager.limb, manager.skeletonData);
            }*/