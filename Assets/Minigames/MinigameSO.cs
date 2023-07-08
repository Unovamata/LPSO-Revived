using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MinigameSO : ScriptableObject{
    public float kibbleRate;
    public int kibbleSaved = 0;
    public int highscore = 0;

    //Getters and Setters;
    public float GetKibbleRate(){ return kibbleRate; }
    public void SetKibbleRate(float KibbleRate){ kibbleRate = KibbleRate; }
    public int GetKibbleSaved(){ return kibbleSaved; }
    public int SetKibbleSaved(int KibbleSaved){ return kibbleSaved = KibbleSaved; }
    public int GetHighScore(){ return highscore; }
    public void SetHighScore(int Highscore){ highscore = Highscore; }
}
