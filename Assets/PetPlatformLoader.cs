using System;
using System.Reflection;
using TMPro;
using UnityEngine;

public class PetPlatformLoader : PetLoader{
    public int silverTicketsAmount, bronzeTicketsAmount;
    [SerializeField] GameObject pet, silverTicket, bronzeTicket;
    [SerializeField] TextMeshProUGUI xPetsText;
    [SerializeField] Transform shadow;
    [SerializeField] int createPets;

    //Data control;
    int petPointer;

    private void Start() {
        base.Start();
        selectedPet = display.transform.Find("PetSelected");
        position = selectedPet.position;

        //Loading the tickets;
        if(bronzeTicketsAmount > 0) {
            CreateTicket(slots[petPointer], bronzeTicket, bronzeTicketsAmount.ToString(), false);
            petPointer++;
        }

        if(silverTicketsAmount > 0) {
            CreateTicket(slots[petPointer], silverTicket, silverTicketsAmount.ToString(), true);
            petPointer++;
        }

        selectedSlot = petPointer + 1;

        FillPetListDebug(createPets);

        //Calling the platform's positions;
        for(int i = petPointer; i < slots.Length; i++) {
            Transform platform = slots[i];
            print(slots.Length);
            Vector3 pos = platform.position;

            //ownedPets.Add(new Pet { petId = UnityEngine.Random.Range(0, 2), name = (i - petPointer).ToString() });
            LoadPetInPlatform(platform, i - petPointer);
        }

        //**Display Only Renders**;
        //Pet platform management;
        GameObject petReference = slots[petPointer].GetComponentInChildren<SwitchSpritePetPlatform>().pet;
        LoadPetOnDisplay(petReference);
        ManageMainPlatformIfPremium(petReference);
        xPetsText.text = ownedPets.Count.ToString() + " Pets in my";

        //Tab Creation;
        //tabs = GetComponent<TabCreator>();
        int tabCount = Mathf.CeilToInt((ownedPets.Count + petPointer) / 6f);
        if(tabCount < 5) {
            tabs.hasNormalNumbering = true;
            tabs.tabMainIndex = -Mathf.RoundToInt(tabCount / 2);
            shadow.localScale = new Vector3(0.07f * (tabCount - 1), 0.37f, 0.37f);
        } else {
            tabs.hasNormalNumbering = false;
            tabs.hasScrollers = true;
            tabs.hasSmallTabs = true;
        }
        tabs.maxTabs = tabCount < 5? tabCount : 5;
        tabs.tabsToCreate = tabCount;
        //tabs.tabMainIndex = 4;    
    }

    public static void ManageMainPlatformIfPremium(GameObject pet) {
        SpriteRenderer[] renders = PetLoader.Instance.renders;
        Sprite[] platformSprites = PetLoader.Instance.sprites;

        //0: Pearls, 1: Bottom, 2: Top, 3: Star;
        if(!IsPremium(pet)) {
            renders[3].gameObject.SetActive(false); //Remove star if it's not premium; Main platform;
            renders[0].sprite = platformSprites[3];
            renders[1].sprite = platformSprites[4];
            renders[2].sprite = platformSprites[5];
        }
        else {
            renders[3].gameObject.SetActive(true); //Remove star if it's not premium; Main platform;
            renders[0].sprite = platformSprites[0];
            renders[1].sprite = platformSprites[1];
            renders[2].sprite = platformSprites[2];
        }
    }

    /////////////////////////////////////////////////////////////////////////////////

    
    private void Update() {
        if(refresh) {
            int currentPos = 0;

            if(tabs.currentTab == 1) {
                //Loading the tickets;
                if(bronzeTicketsAmount > 0) {
                    CreateTicket(slots[currentPos], bronzeTicket, bronzeTicketsAmount.ToString(), false);
                    //SwitchPlatformSpritePremium(slots[currentPos], false);
                    currentPos++;
                }

                if(silverTicketsAmount > 0) {
                    CreateTicket(slots[currentPos], silverTicket, silverTicketsAmount.ToString(), true);
                    //SwitchPlatformSpritePremium(slots[currentPos], true);
                    currentPos++;
                }
            }

            int petPos = ((tabs.currentTab - 1) * 6) - petPointer;

            for(int i = currentPos; i < slots.Length; i++) {
                Transform platform = slots[i];
                LoadPetInPlatform(platform, petPos + i);
            }

            refresh = false;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////

    private bool CreateTicket(Transform platform, GameObject ticket, string ticketAmount, bool isPremium) {
        GameObject instance = Instantiate(ticket, platform);

        instance.name = ticket.name; //Used for data handling later on;

        //Deleting old data;
        SwitchSpritePetPlatform platformData = platform.GetComponentInChildren<SwitchSpritePetPlatform>();

        //Destroying any old references;
        try { Destroy(platformData.pet); } catch { }
        platformData.star.SetActive(false);
        platformData.pet = instance;

        //Changing text;
        GetTicketWhiteText(instance).text = ticketAmount;
        DeactivateHourglass(platform);

        SwitchPlatformSpritePremium(platform, isPremium);
        return true;
    }

    private void LoadPetInPlatform(Transform platform, int i) {
        //Checking if the pet can be written
        Pet petExtracted;
        SwitchSpritePetPlatform platformData;
        try {
            petExtracted = ownedPets[i];
        } catch {
            //If the pet does not exists, then destroy current pets in the platforms;
            platformData = platform.GetComponent<SwitchSpritePetPlatform>();
            try { Destroy(platformData.pet); } catch { }
            DeactivateHourglass(platform);

            SwitchPlatformSpritePremium(platform, false);
            return; 
        }

        //Creating the pet;
        Vector3 pos = platform.position;
        GameObject petInstance = Instantiate(pet, new Vector3(pos.x - 0.19f, pos.y + 0.55f, -1), Quaternion.identity, platform);
        DeactivateHourglass(platform);

        //Editor Parameters;
        petInstance.GetComponent<PetContainer>().petData = petExtracted; //Setting the pet for processing;
        petInstance.name = "Pet";

        platformData = platform.GetComponentInChildren<SwitchSpritePetPlatform>();
        try { Destroy(platformData.pet); } catch { }
        platformData.pet = petInstance;

        //Stop if not premium, else, color the platform;
        SwitchPlatformSpritePremium(platform, IsPremium(ownedPets[i]));
    }

    private void SwitchPlatformSpritePremium(Transform platform, bool id) {
        SwitchSpritePetPlatform switcher = platform.GetComponentInChildren<SwitchSpritePetPlatform>();
        PetLoader instance = PetLoader.Instance;
        switcher.update = true;

        SpriteRenderer[] renders = platform.gameObject.GetComponentsInChildren<SpriteRenderer>();
        if(!id) { //Not premium;
            renders[0].sprite = instance.sprites[4];
            renders[1].sprite = instance.sprites[5];
            switcher.star.SetActive(false);
        } else { //Premium;
            renders[0].sprite = instance.sprites[1];
            renders[1].sprite = instance.sprites[2];
            switcher.star.SetActive(true);
        }
    }
}