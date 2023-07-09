using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FruitistaFetchGameManager : MinigameType{

    //Instance Management;
    public static FruitistaFetchGameManager Self;

    void Awake(){ Self = this; }


    ////////////////////////////////////////////////////////////////////////////////////////


    //Text prefabs reference;
    [SerializeField] GameObject levelPrefabReference, comboPrefabReferece;

    public GameObject GetComboPrefabReference(){ return comboPrefabReferece; }

    //Sounds;
    [SerializeField] AudioClip collectAppleSound, destroyAppleSound;

    public AudioClip GetCollectAppleSound(){ return collectAppleSound; }
    public AudioClip GetDestroyAppleSound(){ return destroyAppleSound; }

    //Data Reference;
    [HideInInspector] List<GameObject> pooledObjects; //Total of apples created in a level;

    public List<GameObject> GetPooledObjects(){ return pooledObjects; }

    //GUI References;
    protected override void Start(){
        base.Start();
        pooledObjects = new List<GameObject>();
        PoolApples(1, 1);
    }


    ////////////////////////////////////////////////////////////////////////////////////////


    [SerializeField] List<GameObject> instancePool;
    int amountToPool, maxAppleIndex, totalApples = 0;
    [HideInInspector] public int[] applesPooled = new int[4]; //Apples pooled for each type; For combos;

    void PoolApples(int min, int max) {
        amountToPool = Random.Range(min, max);
        
        //Generating the apples;
        int appleToGenerate = 2; //For the first level, only red apples can be generated;

        for (int i = 0; i < amountToPool; i++){
            if(currentLevel >= 2) appleToGenerate = Random.Range(0, maxAppleIndex);

            //Creating the apples on a circumference;
            float radius = Random.Range(0.7f, 2.3f);
            float circle = Random.Range(0f, 360f);
            float y = Mathf.Sin(circle);
            float x = Mathf.Cos(circle);

            Vector2 point = new Vector2(x, y); 
            Vector2 applePosition = point * radius;

            //Counting the apples;
            totalApples++;
            applesPooled[appleToGenerate]++; //Apples pooled for each type; For combos;

            //Creating the apples inside the game;
            GameObject obj = Instantiate(instancePool[appleToGenerate], point * 5, transform.rotation);
            obj.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(Vector2.Distance(point, applePosition));
            obj.GetComponent<ApplePickup>().endPosition = applePosition;
            pooledObjects.Add(obj);
        }

        //Loading the data inside the bar filler GUI element;
        fillBarGUI.SetMaxItemsInBar(totalApples);
    }


    ////////////////////////////////////////////////////////////////////////////////////////


    /*[HideInInspector] public static FruitistaFetchGameManager Instance;

    private void Awake() {
        Instance = this;
    }

    //References;
    public TextMeshProUGUI highscore, level; //GUI Elements;
    //public MinigameBarFiller bar;
    public Transform canvas;
    public GameObject levelText;
    public GameObject comboText;
    public AudioClip collectAppleSound, destroyAppleSound;

    ////////////////////////////////////////////////////////////////////////////////////////

    //Game States;
    [HideInInspector] public List<GameObject> pooledObjects;
    MinigameTimerGUI timer;
    MinigameFillBarGUI bar;
    MinigameSO minigameSOController;

    //Creating the apples;
    void Start(){
        pooledObjects = new List<GameObject>();
        PoolApples(1, 1);
        timer = MinigameTimerGUI.Instance;
        bar = MinigameFillBarGUI.Instance;
        minigameSOController = GetMinigameSOController();
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
        if(GetHasMinigameEnded()){
            particles.gameObject.SetActive(false);

            Color glowOpacity = glowRender.color;
            glowOpacity.a = Mathf.Sin(Time.time) * 2;
            glowRender.color = glowOpacity;

            return;
        } 

        if(applesCollected >= totalApples) NewLevel();

        

        //Create more apples mid-game;
        if(currentLevel > 2) {
            particles.gameObject.SetActive(true);
            if(pityBreakerTime <= originalTime / 2 && pityBreakerTime > 0) {
                particles.enableEmission = true;
                if(!playWind) {
                    PlaySFX(windSound);
                    playWind = true;
                }
            } else {
                particles.enableEmission = false;
                playWind = false;
            }

            if(!createdMoreApples && pityBreakerTime <= 0) CreateMoreApples();
            else pityBreakerTime -= Time.deltaTime;
        }


        //Game end;
        if (!GetCanLoadResultsScreen()) return;

        //Destroying relevant objects;
        GameObject[] apples = GameObject.FindGameObjectsWithTag("Apple");
        foreach(GameObject apple in apples) Destroy(apple);
    }

    public void SendScores() {
        if(GetIsMinigameReady()) {
            int comboScore = maxCombo * 100;
            int totalScore = comboScore + currentScore;
            EndScreenMinigame results = game.resultsScreen;
            

            if(results.scores.Count < 2) {
                results.scores.Add(currentScore);
                results.scores.Add(comboScore);
            }

            if(minigameSOController.highscore < totalScore) minigameSOController.highscore = totalScore;
            scoreSent = true;
        }
    }

    public void ResetMinigame() {
        applesCollected = 0;
        totalApples = 0;
        currentScore = 0;
        currentLevel = 0;
        MinigameTimerGUI.TimerReset(this);
        highscore.text = "0";
        level.text = "Level 1";
        this.enabled = false;
        poolSizeRange = Vector2.one;
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
        bar.SetMaxItemsInBar(totalApples);
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

        bar.SetMaxItemsInBar(totalApples);
        createdMoreApples = true;
        bar.RefreshData();
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

        if(currentLevel != 1){
            Instantiate(levelText, canvas.transform); //Level X;
            poolSizeRange.x = poolSizeRange.x + Random.Range(1, 3);
            poolSizeRange.y = Mathf.RoundToInt(poolSizeRange.x * 1.5f) + Random.Range(2, 4);
        }

        //Pity Breaker;
        createdMoreApples = false;
        pityBreakerTime = originalTime + ((currentLevel - 2) * 0.325f);

        //GUI;
        level.text = string.Format("Level: {0}", currentLevel); //Sending data to the GUI;
        timer.AddTime(0.2f);
    }*/
}
