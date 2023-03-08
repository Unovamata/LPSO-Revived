using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MinigameSO : ScriptableObject{
    public float volume = 0.5f, volumeSave = 0.5f;
    public int kibble = 0;
    public int highscore = 0;

    //Getters and Setters;
    public float Volume { get { return volume; } set { volume = value; } }
    public float VolumeSave { get { return volumeSave; } set { volumeSave = value; } }
    public int Kibble { get { return kibble; } set { kibble = value; } }
    public int HighScore { get { return highscore; } set { highscore = value; } }
}
