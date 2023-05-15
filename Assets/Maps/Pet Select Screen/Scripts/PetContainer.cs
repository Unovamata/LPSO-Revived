using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetContainer : MonoBehaviour{
    public Pet petData;
}

public class Pet {
    private int fkId, petId, species, luck, happiness, hunger, comfort;
    private int[] limbs, clothes;
    private string name = "";
    private DateTime adoptionDate;
    private Texture2D texture;

    //Default Constructors;
    public Pet(int PetId, string Name){
        petId = PetId;
        name = Name;
    }

    //Getters and Setters;
    public int GetForeignKeyId() { return fkId; }
    public void SetForeignKeyId(int FkId) { fkId = FkId; }
    public int GetPetId() { return petId; }
    public void SetPetId(int PetId) { petId = PetId; }
    public int GetSpecies() { return species; }
    public void SetSpecies(int Species) { species = Species; }
    public int GetLuck() { return luck; }
    public void SetLuck(int Luck) { luck = Luck; }
    public int GetHappiness() { return happiness; }
    public void SetHappiness(int Happiness) { happiness = Happiness; }
    public int GetHunger() { return hunger; }
    public void SetHunger(int Hunger) { hunger = Hunger; }
    public int GetComfort() { return comfort; }
    public void SetComfort(int Comfort) { comfort = Comfort; }
    public int[] GetLimbs() { return limbs; }
    public void SetLimbs(int[] Limbs) { limbs = Limbs; }
    public int[] GetClothes() { return clothes; }
    public void SetClothes(int[] Clothes) { clothes = Clothes; }
    public string GetName() { return name; }
    public void SetName(string Name) { name = Name; }
    public DateTime GetAdoptionDate() { return adoptionDate; }
    public void SetAdoptionDate(DateTime AdoptionDate) { adoptionDate = AdoptionDate; }
    public Texture2D GetTexture() { return texture; }
    public void SetTexture(Texture2D Texture) { texture = Texture; }
}

public class Hourglass {
    GameObject hourglass;

    public Hourglass(GameObject Hourglass) {
        hourglass = Hourglass;
        HourglassRotate180();
    }

    private void HourglassRotate180() {
        hourglass.transform.rotation = new Quaternion(0, 0, 0, 0);
        LeanTween.rotateZ(hourglass, 180, 0.8f).setEaseOutCirc().setOnComplete(HourglassRotate360);
    }
    private void HourglassRotate360() {
        LeanTween.rotateZ(hourglass, 360, 0.8f).setEaseOutCirc().setOnComplete(HourglassRotate180);
    }
}