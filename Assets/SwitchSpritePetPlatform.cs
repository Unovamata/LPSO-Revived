using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchSpritePetPlatform : MonoBehaviour{
    [SerializeField] SpriteRenderer topRender, bottomRender;
    [SerializeField] Sprite topSelected, bottomSelected;
    [HideInInspector] public Sprite oldTop, oldBottom;
    public GameObject star, hourglass, pet, ringPrefabReference, ring;
    [HideInInspector] public int indexer;

    private void Start() {
        HourglassRotate180();
        if(IsSelectedSlot()) RingLeanTween(1, 2, 1, 0.5f, false);
    }

    private void HourglassRotate180() {
        hourglass.transform.rotation = new Quaternion(0, 0, 0, 0);
        LeanTween.rotateZ(hourglass, 180, 0.8f).setEaseOutCirc().setOnComplete(HourglassRotate360);
    }

    private void HourglassRotate360() {
        LeanTween.rotateZ(hourglass, 360, 0.8f).setEaseOutCirc().setOnComplete(HourglassRotate180);
    }

    public bool update = true;
    bool canRender;

    private void Update() {
        if (update) {
            oldTop = topRender.sprite;
            oldBottom = bottomRender.sprite;
            update = false;
        }

        canRender = true;

        //Selected platform ring;
        if(IsSelectedSlot()) {
            if(PetLoader.Instance.refresh) RingLeanTween(2, 1, 0, 0.3f, true);

            if(ring == null) RingLeanTween(1, 2, 1, 0.5f, false);
        } else RingLeanTween(2, 1, 0, 0.3f, true);
    }

    private void RingLeanTween(float scale, float divisor, float alpha, float fadeTime, bool isDestroyable) {
        if(!isDestroyable) ring = Instantiate(ringPrefabReference, transform);
        else try { LeanTween.cancel(ring); } catch { };
        try {
            SpriteRenderer[] renders = ring.GetComponentsInChildren<SpriteRenderer>();
            LeanTween.scale(ring, Vector3.one * scale, fadeTime).setEaseOutCirc();

            if(isDestroyable) LeanTween.alpha(renders[0].gameObject, alpha, fadeTime).setOnComplete(DestroyRing);
            else LeanTween.alpha(renders[0].gameObject, alpha, fadeTime);
            LeanTween.alpha(renders[1].gameObject, alpha, fadeTime / divisor);
        } catch { }
    }

    private void DestroyRing() {
        Destroy(ring);
    }

    private void OnMouseEnter() {
        if(canRender) {
            topRender.sprite = topSelected;
            bottomRender.sprite = bottomSelected;
        }
    }

    private void OnMouseExit() {
        topRender.sprite = oldTop;
        bottomRender.sprite = oldBottom;
    }

    private bool IsSelectedSlot() {
        PetLoader instance = PetLoader.Instance;
        bool condition;

        try {
            condition = instance.selectedSlot.ToString() == transform.parent.name && instance.selectedPetTab == instance.tabs.currentTab;
        } catch {
            return false;
        }

        return condition;
    }

    private void OnMouseDown() {
        PetLoader instance = PetLoader.Instance;
        if(pet != null) {
            PetLoader.LoadPetOnDisplay(pet);
            PetPlatformLoader.ManageMainPlatformIfPremium(pet);
            instance.selectedSlot = int.Parse(transform.parent.name);
            instance.selectedPetTab = instance.tabs.currentTab;
        }
    }
}
