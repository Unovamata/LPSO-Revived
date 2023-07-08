using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FruitistaFetchGameManager : MonoBehaviour{
    [HideInInspector] public static FruitistaFetchGameManager Instance;

    private void Awake() {
        Instance = this;
    }

    //References;
    public TextMeshProUGUI highscore, level; //GUI Elements;
    //public MinigameBarFiller bar;
    public Transform canvas;
    public GameObject levelText;
    public GameObject comboText;
    GameManagerType game;
    public AudioClip collectAppleSound, destroyAppleSound;

    //-------------------------------------------------------------------------------------


    //Game States;
    [HideInInspector] public List<GameObject> pooledObjects;

    //Creating the apples;
    void Start(){
        pooledObjects = new List<GameObject>();
        game = GameManagerType.Instance;
        PoolApples(1, 1);
        //Instantiate(levelText);
    }


    //-------------------------------------------------------------------------------------


    //Data Followers;
    [HideInInspector] public int applesCollected = 0, currentScore = 0;
    [HideInInspector] public int totalApples = 0, maxCombo = 0;
    [SerializeField] SpriteRenderer glowRender;
    bool scoreSent = false, createdMoreApples = false;
    [SerializeField] float pityBreakerTime = 17, originalTime = 12; //In seconds;
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioClip windSound;
    bool playWind = false;

    // Update is called once per frame
    void Update(){
        if(applesCollected >= totalApples) NewLevel();

        //Create more apples mid-game;
        if(!game.gameEnd) {
            if(currentLevel > 2) {
                particles.gameObject.SetActive(true);
                if(pityBreakerTime <= originalTime / 2 && pityBreakerTime > 0) {
                    particles.enableEmission = true;
                    if(!playWind) {
                        game.sfx2.PlayOneShot(windSound);
                        playWind = true;
                    }
                } else {
                    particles.enableEmission = false;
                    playWind = false;
                }

                if(!createdMoreApples && pityBreakerTime <= 0) CreateMoreApples();
                else pityBreakerTime -= Time.deltaTime;
            }
        } else particles.gameObject.SetActive(false);


        //Game end;
        if (game.canLoadResultsScreen) {
            //Destroying relevant objects;
            GameObject[] apples = GameObject.FindGameObjectsWithTag("Apple");
            foreach(GameObject apple in apples) Destroy(apple);
        }

        if (!game.gameEnd) {
            Color glowOpacity = glowRender.color;
            glowOpacity.a = Mathf.Sin(Time.time) * 2;
            glowRender.color = glowOpacity;
        }
    }

    public void SendScores() {
        if(game.isReady) {
            /*EndScreenMinigame results = game.resultsScreen;
            int comboScore = maxCombo * 100;
            int totalScore = comboScore + currentScore;

            if(results.scores.Count < 2) {
                results.scores.Add(currentScore);
                results.scores.Add(comboScore);
            }

            if(game.controller.highscore < totalScore) game.controller.highscore = totalScore;
            scoreSent = true;*/
        }
    }

    public void ResetMinigame() {
        applesCollected = 0;
        totalApples = 0;
        currentScore = 0;
        currentLevel = 0;
        game.timer.timeAvailable = game.timer.originalTime;
        highscore.text = "0";
        level.text = "Level 1";
        this.enabled = false;
        poolSizeRange = Vector2.one;
        game.timer.timesUp = false;
        //GameManagerType.Instance.resultsScreen.scores = new List<int>();
    }

    //-------------------------------------------------------------------------------------


    //Apple Generation;
    [HideInInspector] public Vector2 poolSizeRange = Vector2.one;
    private int amountToPool;

    //Data management;
    [SerializeField] public GameObject[] objectsToPool = new GameObject[4]; //Green, Orange, Red, Yellow;
    [HideInInspector] public int[] applesPooled = new int[4]; //Apples pooled for each type; For combos;
    public int maxAppleIndex = 0; //1 through 4;

    private void PoolApples(int min, int max) {
        amountToPool = Random.Range(min, max);
        
        //Generating the apples;
        int appleToGenerate = 2;

        for (int i = 0; i < amountToPool; i++){
            if(currentLevel >= 2) appleToGenerate = Random.Range(0, maxAppleIndex);

            //Creating the apples on a circumference;
            float radius = Random.Range(0.7f, 2.3f), circle = Random.Range(0,360);
            float y = Mathf.Sin(circle), x = Mathf.Cos(circle);
            Vector2 point = new Vector2(x, y), applePosition = point * radius;

            //Counting the apples;
            totalApples++;
            applesPooled[appleToGenerate]++; //Adding the apple generated to a tracker;

            //Creating the apples inside the game;
            GameObject obj = Instantiate(objectsToPool[appleToGenerate], point * 5, transform.rotation);
            obj.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(Vector2.Distance(point, applePosition));
            obj.GetComponent<ApplePickup>().endPosition = applePosition;
            pooledObjects.Add(obj);
        }

        //Loading the data inside the bar filler GUI element;
        game.bar.maxItems = totalApples;
    }

    private void CreateMoreApples() {
        int pool = Random.Range(1, Mathf.RoundToInt(currentLevel * 1.5f));

        //Generating the apples;
        int appleToGenerate = 2;

        for(int i = 0; i < pool; i++) {
            if(currentLevel >= 2) appleToGenerate = Random.Range(0, maxAppleIndex);

            //Creating the apples on a circumference;
            float radius = Random.Range(0.7f, 2.3f), circle = Random.Range(0, 360);
            float y = Mathf.Sin(circle), x = Mathf.Cos(circle);
            Vector2 point = new Vector2(x, y), applePosition = point * radius;

            //Counting the apples;
            totalApples++;

            //Creating the apples inside the game;
            GameObject obj = Instantiate(objectsToPool[appleToGenerate], point * 5, transform.rotation);
            obj.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(Vector2.Distance(point, applePosition));
            obj.GetComponent<ApplePickup>().endPosition = applePosition;
            obj.GetComponent<ApplePickup>().type = false;
            pooledObjects.Add(obj);
        }

        //Loading the data inside the bar filler GUI element;
        game.bar.maxItems = totalApples;
        createdMoreApples = true;
        game.bar.reset = true;
    }


    //-------------------------------------------------------------------------------------


    [HideInInspector] public int currentLevel = 1;

    private void NewLevel() {
        //Resetting pooled apples;
        for(int i = 0; i < 4; i++) applesPooled[i] = 0;

        //Resetting data;
        applesCollected = 0;
        totalApples = 0;
        currentLevel++;
        pooledObjects = new List<GameObject>();
        //game.isReady = false;
        PoolApples((int)poolSizeRange.x, (int)poolSizeRange.y);

        if(currentLevel != 1 && !game.playPressed){
            Instantiate(levelText, canvas.transform); //Level X;
            poolSizeRange.x = poolSizeRange.x + Random.Range(1, 3);
            poolSizeRange.y = Mathf.RoundToInt(poolSizeRange.x * 1.5f) + Random.Range(2, 4);
        }

        //Pity Breaker;
        createdMoreApples = false;
        pityBreakerTime = originalTime + ((currentLevel - 2) * 0.325f);

        //GUI;
        level.text = string.Format("Level: {0}", currentLevel); //Sending data to the GUI;
        game.timer.timeAvailable = game.timer.timeAvailable + 0.2f; //In seconds;
    }
}
