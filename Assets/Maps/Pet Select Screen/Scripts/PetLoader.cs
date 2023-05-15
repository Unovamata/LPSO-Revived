using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PetLoader : MonoBehaviour{
    public static PetLoader Instance;

    private void Awake() {
        Instance = this;
    }

    //Pet Management;
    [HideInInspector] public List<Pet> ownedPets = new List<Pet>();
    [HideInInspector] public IDictionary<int, bool> petPremium = new Dictionary<int, bool>();
    public Transform parent, display;
    public Transform[] slots;
    [HideInInspector] public int selectedSlot = 1, selectedPetTab = 1;
    [HideInInspector] public SpriteRenderer[] renders;
    public TextMeshProUGUI petName, petType;
    [HideInInspector] public Transform selectedPet;
    [HideInInspector] public TabCreator tabs;
    [HideInInspector] public Vector3 position;
    public Sprite[] sprites;
    private GameObject hourglass;

    // Start is called before the first frame update
    public void Start(){
        //Pet Setup;
        PetDictionarySetup();
        slots = GetTopLevelChildren(parent);
        renders = display.GetComponentsInChildren<SpriteRenderer>();
        tabs = GetComponent<TabCreator>();

        //Hourglass;
        try {
            hourglass = display.Find("Hourglass").gameObject;
            new Hourglass(hourglass);
        } catch { }
    }

    //Are pets premium?
    public void PetDictionarySetup() {
        petPremium.Add(0, false);
        petPremium.Add(1, true);
    }

    public static Transform[] GetTopLevelChildren(Transform parent) {
        int count = parent.childCount;
        Transform[] children = new Transform[count];

        //Taking in the first child of a parent;
        for(int id = 0; id < count; id++){
            children[id] = parent.GetChild(id);
        } return children;
    }

    public void FillPetListDebug(int max) {
        for(int i = 0; i < max; i++) ownedPets.Add(new Pet(UnityEngine.Random.Range(0, 2), i.ToString()));
    }

    public Transform petSpawnerMainDisplay;

    //Puts the pet on the main display;
    public static GameObject LoadPetOnDisplay(GameObject pet) {
        PetLoader instance = PetLoader.Instance;

        //Hourglass;
        instance.hourglass.SetActive(true);

        //Calling the pet;
        Transform selected = instance.selectedPet.transform;
        selected.gameObject.SetActive(false);
        //selected.localScale = Vector3.one * 1.1f;

        //Creating the pet;
        Vector3 pos = instance.petSpawnerMainDisplay.position;
        GameObject newPet = Instantiate(pet, pos, Quaternion.identity, instance.petSpawnerMainDisplay);
        instance.selectedPet = newPet.transform;
        newPet.name = "PetSelected";

        if(newPet.GetComponentInChildren<SpriteRenderer>()) { //If it's a ticket;
            newPet.transform.position = instance.position + new Vector3(0.4f, -0.1f, 0);
        }

        //Changing GUI data;
        try {
            instance.petName.text = pet.GetComponent<PetContainer>().petData.GetName();
            instance.petType.text = "Silver Ticket";
        } catch {
            instance.petName.text = pet.name; //Ticket names;
            instance.petType.text = "";

            //Ticket amount;
            GetTicketWhiteText(newPet).text = "1";
        }

        //Destroying the old pet;
        Destroy(selected.gameObject);
        instance.hourglass.SetActive(false);
        return newPet;
    }

    public static TextMeshProUGUI GetTicketWhiteText(GameObject instance) {
        return instance.transform.GetChild(0).GetChild(0).Find("White Text").GetComponent<TextMeshProUGUI>();
    }

    public static void DeactivateHourglass(Transform platform) {
        platform.Find("Hourglass").gameObject.SetActive(false);
    }

    public static void ActivateHourglass(Transform platform) {
        platform.Find("Hourglass").gameObject.SetActive(true);
    }

    public static bool IsPremium(GameObject pet) {
        PetLoader instance = PetLoader.Instance;
        Pet data;
        bool isPremium;

        try {
            data = pet.GetComponent<PetContainer>().petData;
            isPremium = instance.petPremium[data.GetPetId()];
        } catch {
            try { isPremium = pet.name == "Bronze Ticket" ? false : true; } 
            catch { return IsPremium(instance.ownedPets[instance.selectedPetTab * instance.selectedSlot].GetPetId());  }
        }

        return isPremium;
    }

    public static bool IsPremium(int petId) { return PetLoader.Instance.petPremium[petId]; }
    public static bool IsPremium(Pet pet) { return PetLoader.Instance.petPremium[pet.GetPetId()]; }

    [HideInInspector] public bool refresh = false;
}
