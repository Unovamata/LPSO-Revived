using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public interface IMinigameType{
    void Play();
    void HowToPlay();
    void Leave();
    void StopPlayingGame();
    void CallTransition();
}

public interface IMinigameAudio{
    void Mute();
    void PlaySound();
}

public class MinigameType : MonoBehaviour, IMinigameType, IMinigameAudio{


    //IMinigameType
    public void Play(){ }

    public void HowToPlay(){ }

    public void Leave(){ }
    public void StopPlayingGame(){ }

    public void CallTransition(){}

    //IMinigameAudio

    public void Mute(){}

    public void PlaySound(){}


    ////////////////////////////////////////////////////////////////////////////////////////
    

    //Instance Management;
    public static MinigameType Instance;

    void Awake(){ Instance = this; }


    ////////////////////////////////////////////////////////////////////////////////////////


    //Variable Management;
    bool isMinigameReady, isPlayingMinigame, hasMinigameEnded, hasTransitionEnded, canLoadResultsScreen;

    public bool GetIsMinigameReady(){ return isMinigameReady; }
    public void SetIsMinigameReady(bool IsMinigameReady){ isMinigameReady = IsMinigameReady; }
    public bool GetHasMinigameEnded(){ return hasMinigameEnded; }
    public void SetHasMinigameEnded(bool HasMinigameEnded){ hasMinigameEnded = HasMinigameEnded; }
    public bool GetHasTransitionEnded(){ return hasTransitionEnded; }
    public void SetHasTransitionEnded(bool HasTransitionEnded){ hasTransitionEnded = HasTransitionEnded; }
    public bool GetCanLoadResultsScreen(){ return canLoadResultsScreen; }
    public void SetCanLoadResultsScreen(bool CanLoadResultsScreen){ canLoadResultsScreen = CanLoadResultsScreen; }


    [SerializeField] AudioSource soundtrack, sfx;

    public void PlaySFX(AudioClip sound){ sfx.PlayOneShot(sound); }

    [SerializeField] MinigameSO controller;

    public MinigameSO GetMinigameSOController(){ return controller; }

    /*[HideInInspector] public static GameManagerType Instance;

    private void Awake() {
        Instance = this;
    }

    //-------------------------------------------------------------------------------------


    //References;
    public GameObject generalGreenText, generalPinkText, GUI, startScreen, highscore, transition;
    public LeaderboardMinigame resultsScreen;
    public MinigameTimeGUI timer;
    public MinigameBarFiller bar;
    public Transform canvas;
    


    //-------------------------------------------------------------------------------------

    //Audio;
    public AudioListener audioListener;
    [HideInInspector] public AudioSource soundtrack, sfx, sfx2;
    public AudioClip songToPlay;
    Vector2 originalResultsPosition;
    public bool playPressed = false;

    private void Start() {
        soundtrack = gameObject.AddComponent<AudioSource>();
        soundtrack.playOnAwake = true;
        soundtrack.loop = true;
        soundtrack.clip = songToPlay;
        soundtrack.Play();
        sfx = gameObject.AddComponent<AudioSource>();
        sfx.playOnAwake = true;
        sfx2 = gameObject.AddComponent<AudioSource>();
        sfx2.playOnAwake = true;
        playPressed = false;

        //Repositioning the results table;
        originalResultsPosition = resultsScreen.transform.position;
        resultsScreen.transform.position = Vector2.one * 6;
    }


    //-------------------------------------------------------------------------------------


    //Game states;
    [HideInInspector] public bool gameEnd = false, transitionEnd = false,
        canLoadResultsScreen = false, abruptEnd = false;
    [HideInInspector] public bool gameStart = false; //When transition finishes;
    [HideInInspector] public bool isReady = false; //When countdown finishes;

    private void Update() {
        //if(Input.GetKey(KeyCode.Space)) SceneManager.LoadScene(SceneManager.GetActiveScene().name); //DEBUG;

        if (gameStart) {
            //resultsScreen.transform.position = Vector2.one * 6;
        }

        if (transitionEnd) {
            GUI.SetActive(true);
            gameStart = true;
            highscore.SetActive(false);
            startScreen.SetActive(false);
            if(gameStart && gameEnd){
                //EndScreenMinigame.ProcessScores();
                GameManagerType.Instance.canLoadResultsScreen = true;
            }
            playPressed = false;
            transitionEnd = false;
        }

        if(gameEnd){
            if(canLoadResultsScreen) {
                //resultsScreen.transform.position = originalResultsPosition;
                highscore.gameObject.SetActive(true);
            }
            
            if(canLoadResultsScreen && !resultsScreen.gameObject.activeSelf){
                resultsScreen.transform.position = originalResultsPosition;
                //resultsScreen.gameObject.SetActive(true);
                highscore.gameObject.SetActive(true);
            }
            if(GUI.activeSelf) GUI.SetActive(false);
        }
    }

    //-------------------------------------------------------------------------------------

    public static void CreateGreenText(string text, Vector2 pos, float drawTime, float scale) {
        GameObject result = Instantiate(GameManagerType.Instance.generalGreenText, pos, Quaternion.identity, GameManagerType.Instance.canvas);
        GeneralText config = result.GetComponent<GeneralText>();
        
        config.newText = text;
        config.drawTime = drawTime;
        config.textScale = new Vector3(scale, scale, scale);
    }

    public static void CreatePinkText(string text, Vector2 pos, float drawTime, float scale) {
        GameObject result = Instantiate(GameManagerType.Instance.generalPinkText, pos, Quaternion.identity, GameManagerType.Instance.canvas);
        GeneralText config = result.GetComponent<GeneralText>();
        
        config.newText = text;
        config.drawTime = drawTime;
        config.textScale = new Vector3(scale, scale, scale);
    }

    public static string FormatDecimals(int score) {
        return score.ToString("N0");
    }

    public Camera camera;

    public static void CameraPulse(float startSize) {
        Camera cam = GameManagerType.Instance.camera;
        float destinationSize = 2.75f;
        cam.orthographicSize = startSize;
        LeanTween.value(cam.gameObject, startSize, destinationSize, 0.5f).setOnUpdate((float val) => { cam.orthographicSize = destinationSize; }); 
    }

    public static void ActivateScripts(MonoBehaviour[] components) {
        foreach(MonoBehaviour component in components) component.enabled = true;
    }

    public static MonoBehaviour[] GetAllComponents(GameObject container) {
        return container.GetComponents<MonoBehaviour>();
    }*/
}