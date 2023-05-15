using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchSpritePetPlatform : MonoBehaviour{
    [SerializeField] SpriteRenderer topRender, bottomRender;
    [SerializeField] Sprite topSelected, bottomSelected;
    [HideInInspector] public Sprite oldTop, oldBottom;
    public GameObject star, hourglass, pet, ringPrefabReference, ring, petSpawner;
    [HideInInspector] public int indexer;
    private void Start() {
        new Hourglass(hourglass);
        if(IsSelectedSlot()) RingLeanTween(1, 2, 1, 0.5f, false);
    }

    private bool IsSelectedSlot() {
        PetLoader instance = PetLoader.Instance;
        bool condition;

        try {
            condition = (instance.selectedSlot.ToString() == transform.name && instance.selectedPetTab == instance.tabs.currentTab);
        } catch {
            return false;
        }

        return condition;
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

            for(int i = 1; i < renders.Length; i++) {
                LeanTween.alpha(renders[i].gameObject, alpha, fadeTime / divisor);
            }
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

    private void OnMouseDown() {
        PetLoader instance = PetLoader.Instance;
        if(pet != null) {
            PetLoader.LoadPetOnDisplay(pet);
            PetPlatformLoader.ManageMainPlatformIfPremium(pet);
            instance.selectedSlot = int.Parse(transform.name);
            instance.selectedPetTab = instance.tabs.currentTab;
        }
    }
}
