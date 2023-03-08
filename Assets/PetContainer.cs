using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetContainer : MonoBehaviour{
    public Pet petData;
}

public class Pet {
    public int fkId, petId, species, luck, happiness, hunger, comfort;
    public int[] limbs, clothes;
    public string name = "";
    public DateTime adoptionDate;
    public Texture2D texture;
}
