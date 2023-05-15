using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabButtonFunctions : MonoBehaviour {
    public int index;

    public void LoadPetPlatforms() {
        PetLoader instance = PetLoader.Instance;

        if(index != instance.tabs.currentTab) {
            instance.tabs.currentTab = index;
            instance.tabs.refresh = true;
            instance.refresh = true;
            if(instance.tabs.maxTabs < 5) {
                instance.tabs.tabMainIndex = index - 1;
            }
        }
    }

    float easingTime = 0.15f;
    float startSize = 0;

    public static void ScaleUp(GameObject button) {
        LeanTween.scale(button, 0.45f * Vector3.one, 0.25f).setEaseOutBack();
    }

    public static void ScaleDown(GameObject button) {
        LeanTween.scale(button, 0.37f * Vector3.one, 0.25f).setEaseOutBack();
    }

    public void PulseOnMouseEnter() {
        GameObject parent = transform.parent.gameObject;
        startSize = parent.transform.localScale.x;
        float size = 0.45f;
        Vector3 scale = new Vector3(size * Mathf.Sign(startSize), Mathf.Abs(size), size);
        LeanTween.scale(transform.parent.gameObject, scale, 0.25f).setEaseOutBack();
    }

    public void PulseOnMouseLeave() {
        GameObject parent = transform.parent.gameObject;
        float size = startSize;
        Vector3 scale = new Vector3(size, Mathf.Abs(size), size);
        LeanTween.scale(transform.parent.gameObject, scale, 0.25f).setEaseOutBack();
    }
}