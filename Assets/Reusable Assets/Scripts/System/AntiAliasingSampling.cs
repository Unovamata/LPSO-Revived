using UnityEngine;
using System;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class AntiAliasingSampling : MonoBehaviour {
    const int BoxDownPass = 0;
    const int BoxUpPass = 1;

    [SerializeField] Shader bloomShader;
    [NonSerialized] Material bloom;
    [Range(1, 2)] public int iterations = 1;
    [Range(0.1f, 2)] public float downSampling = 1;

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if(bloom == null) {
            bloom = new Material(bloomShader);
            bloom.hideFlags = HideFlags.HideAndDontSave;
        }

        int width = (int) (source.width / downSampling);
        int height = (int) (source.height / downSampling);
        RenderTextureFormat format = source.format;

        //Calling the texture;
        RenderTexture[] textures = new RenderTexture[16];
        RenderTexture currentDestination = textures[0] = RenderTexture.GetTemporary(width, height, 0, format);

        Graphics.Blit(source, currentDestination);
        RenderTexture currentSource = currentDestination;
        Graphics.Blit(currentSource, destination);
        RenderTexture.ReleaseTemporary(currentSource);

        int i = 1;
        for(; i < iterations; i++) {
            width /= 2;
            height /= 2;
            if(height < 2) break;
            currentDestination = textures[i] = RenderTexture.GetTemporary(width, height, 0, format);
            Graphics.Blit(currentSource, currentDestination);
            currentSource = currentDestination;
        }

        for(i -= 2; i >= 0; i--) {
            currentDestination = textures[i];
            textures[i] = null;
            Graphics.Blit(currentSource, currentDestination);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDestination;
        }

        Graphics.Blit(currentSource, destination);
    }
}