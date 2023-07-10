using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectApples : MonoBehaviour{
    private const float SIZE = 1.5f; //Scale * SIZE to increase per apple collected.
    [SerializeField] public ApplePickup.AppleType firstApple; //Apple dictating what can be collected;
    public SpriteRenderer render; //Collect area sprite;
    private Color originalColor, destinationColor; //Color management;
    private Vector2 originalSize, destinationScale; //Scale management;
    [HideInInspector] public Color[] appleColors = new Color[5]; //Colors to color the collect area;
    private int appleIndex = 4, cooldown = 0, combo = 0; //Controllers for the collect area;
    FruitistaFetchGameManager minigameReference;
    PlayerMove player;

    //-------------------------------------------------------------------------------------

    void Start() {
        render = this.GetComponent<SpriteRenderer>();
        originalColor = render.color;
        destinationColor = originalColor;
        originalSize = transform.localScale;
        destinationScale = originalSize;
        appleColors[0] = new Color(0.52f, 0.75f, 0, 0.50f);
        appleColors[1] = new Color(0.98f, 0.6f, 0.06f, 0.50f);
        appleColors[2] = new Color(0.75f, 0.11f, 0.06f, 0.50f);
        appleColors[3] = new Color(0.75f, 0.70f, 0.1f, 0.50f);
        minigameReference = FruitistaFetchGameManager.Self;
        player = PlayerMove.Instance;
    }

    //-------------------------------------------------------------------------------------

    private bool emptied = false, almostThere = false, gottemAll = false;

    private void Update() {
        Vector2 shadow = transform.localScale;
        Vector2 vectorLerp = Vector2.Lerp(shadow, destinationScale, Time.deltaTime * 150f); ;
        transform.localScale = vectorLerp;
        player.rings[0].gameObject.transform.localScale = vectorLerp * 0.7f;
        //player.rings[1].gameObject.transform.localScale = vectorLerp * 0.7f;

        //Coloring;
        Color lerping = Color.Lerp(render.color, destinationColor, Time.deltaTime * 7f);
        render.color = lerping;
        
        Color ringColor = destinationColor;
        ringColor.a = 0.6f;
        var mainRingZero = PlayerMove.Instance.rings[0].main;
        mainRingZero.startColor = ringColor;
        /*var mainRingOne = PlayerMove.Instance.rings[1].main;
        mainRingOne.startColor = ringColor;*/


        //If apples have been destroyed wait for a little bit;
        if(cooldown > 0) cooldown--;

        //Destroying apples;
        float distance = Vector2.Dot(transform.position, player.basketPosition);
        if(distance <= 0.35f){
            if(!minigameReference.GetHasMinigameEnded()) {
                DestroyApplePile(player.applesList.Count);
            }
        } else {
            if(Input.GetMouseButtonDown(0)){
                Vector3 basket = new Vector2(player.basketPosition.x, 
                    0.35f + player.basketPosition.y);

                //Bouncing apples;
                foreach(GameObject apple in player.applesList) {
                    ApplePickup appleVariables = apple.GetComponent<ApplePickup>();
                    basket.y += (0.15f * appleVariables.pilePosition);
                    appleVariables.startPosition = basket;
                    appleVariables.nextPos = basket;
                    appleVariables.transform.position = basket;
                    appleVariables.time = 0;
                    appleVariables.bounces = 0;
                    appleVariables.canPickup = false;
                    appleVariables.isPicked = false;
                    emptied = true;
                }

                combo = 0;
                player.applesList = new List<GameObject>();
                firstApple = ApplePickup.AppleType.Null;
            }
             
        }

        //Resetting the basket's contents;
        if(emptied){
            firstApple = ApplePickup.AppleType.Null;
            destinationColor = originalColor;
            destinationScale = originalSize;
            almostThere = false;
            gottemAll = false;
            emptied = false;
        }

        if(minigameReference.GetCanLoadResultsScreen()){
            emptied = true;
            combo = 0;
            TextAnimations.ResetTextParameters(minigameReference.GetComboPrefabReference(), Vector3.zero);
        }
    }

    //-------------------------------------------------------------------------------------

    private void OnCollisionStay2D(Collision2D collision) {
        int applePile = PlayerMove.Instance.applesList.Count; 

        //Collecting apples;
        if(cooldown <= 0 && minigameReference.GetIsMinigameReady() && !minigameReference.GetHasMinigameEnded()) CollectApple(collision, applePile);
    }

    //-------------------------------------------------------------------------------------

    private void CollectApple(Collision2D collision, int applePile) {
        ApplePickup appleController;
        GameObject apple;

        try{ 
            appleController = collision.gameObject.GetComponent<ApplePickup>(); 
        } catch{ appleController = null; }
        if(appleController == null) return;

        //If the apple type is equal to the first apple taken;
        if(firstApple == appleController.color) {
            if(appleController.isPicked) return;
            //Animating the basket;
            float time = 1f;
            Vector3 vectorBasket = new Vector3(0.15f, 0.15f, 0.15f);
            player.gameObject.transform.localScale = vectorBasket * 0.85f;
            LeanTween.cancel(player.gameObject);
            LeanTween.scale(player.gameObject, vectorBasket, time).setEase(LeanTweenType.easeOutElastic).setDelay(0.1f);
            Vector3 three = new Vector3(3, 3, 3);
                

            //Animating the apples;
            for(int i = 0; i < PlayerMove.Instance.applesList.Count; i++){
                float delay = i * 0.01f;
                GameObject currentApple = player.applesList[i];
                currentApple.transform.localScale = three * 0.6f;
                LeanTween.cancel(currentApple);
                LeanTween.scale(currentApple, three, time).setEase(LeanTweenType.easeOutElastic).setDelay(delay);
                //currentApple.GetComponent<ApplePickup>().isPicked = true;
            }

            //Coloring the shadow;
            destinationColor = appleColors[(int)firstApple];


            //Adding onto a list;
            apple = collision.gameObject;

            //Returning if it contains the apple in the list;
            if(player.applesList.Contains(apple)) return;

            //Position on pile;
            appleController.pilePosition = applePile;
            appleController.isPicked = true;

            apple.GetComponent<SpriteRenderer>().sortingOrder = 6 + applePile;
            PlayerMove.Instance.applesList.Add(apple);
            cooldown++;
            destinationScale = new Vector2(Mathf.Clamp(originalSize.x + (SIZE * (applePile + 1)), 0, 50f), Mathf.Clamp(originalSize.y + (SIZE * (applePile + 1)), 0, 50f));

            //Triggering the text once per cycle;
            if(appleController.type) {
                if(!almostThere) {
                    if(minigameReference.applesPooled[(int)firstApple] > 2 && Mathf.FloorToInt(minigameReference.applesPooled[(int)firstApple] / 2) == applePile) {
                        //GameManagerType.CreateGreenText("Getting Close!", transform.position, 0.4f, 0.8f);
                        almostThere = true;
                    }
                }

                if(!gottemAll) {
                    if(minigameReference.applesPooled[(int)firstApple] - 1 == applePile) {
                        //GameManagerType.CreatePinkText("Got 'Em All!", transform.position, 0.4f, 1f);
                    }
                }
            }

            minigameReference.PlaySFX(minigameReference.GetCollectAppleSound());
        } else {
            //If its a Null apple;
            if((int)firstApple == 4 && appleController.canPickup) firstApple = appleController.color;
        }
    }

    //-------------------------------------------------------------------------------------

    private void DestroyApplePile(int applePile) {
        MinigameTimerGUI timer = MinigameTimerGUI.Instance;

        //Destroying the apples;
        //if (collision.gameObject == PlayerMove.Instance.ShadowBasket) {
        if (Input.GetMouseButtonDown(0) && timer.IsThereAnyTime() && applePile > 0) {
            emptied = true;
            minigameReference.PlaySFX(minigameReference.GetDestroyAppleSound());

            //Combos;
            try {
                if(applePile >= minigameReference.applesPooled[(int)firstApple]){
                    float addedTime = 0.5f;
                    combo++;
                    if(combo > 5) {

                        timer.AddTime(addedTime);
                        Vector2 textPosition = transform.position;
                        textPosition.y -= 0.25f;
                        //GameManagerType.CreatePinkText("+0.5 Secs!", textPosition, 0.4f, 0.8f);
                    }
                    if(minigameReference.GetMaxCombo() < combo) minigameReference.SetMaxCombo(combo);
                } else {
                    combo = 0;
                    TextAnimations.ResetTextParameters(minigameReference.GetComboPrefabReference(), Vector3.zero);
                }
            } catch { }

            //Score calculation;
            MinigameScoreGUI score = MinigameScoreGUI.Instance;
            int currentScore = score.GetCurrentScore();
            currentScore += Mathf.RoundToInt((applePile * 100) * (1 + (applePile * 0.15f)) * (1 + (combo * 0.05f)));
            score.SetCurrentScore(currentScore);
            //GameManagerType.CameraPulse(2.6f);

            for(int i = 0; i < applePile; i++) {
                GameObject appleToRemove = PlayerMove.Instance.applesList[i];
                minigameReference.GetPooledObjects().Remove(appleToRemove);
                Destroy(appleToRemove);
            }

            //Translating the data to the gui bars & scores;
            //MinigameHighscoreMedalGUI.Instance.SetHighscore();
            //game.highscore.text = currentScore.ToString();
            
            MinigameFillBarGUI bar = MinigameFillBarGUI.Instance;

            int applesGotten = bar.GetCurrentItemsInBar(); //Bar;
            //minigameReference.applesCollected = applesGotten + applePile;
            bar.SetCurrentItemsInBar(applesGotten + applePile);
            //gameType.bar.textMesh.text = (applesGotten + applePile).ToString();

            if(combo > 5){
                Vector3 position = new Vector3(4f, -2.3f, 0f),
                extraTimePos = new Vector3(position.x, position.y - 0.25f, position.z);

                float comboScale = 0.5f + ((combo * 0.01f) * 2);
                TextAnimations.Jump(minigameReference.GetComboPrefabReference(), new Vector3(comboScale, comboScale, 0));

                minigameReference.GetComboPrefabReference().GetComponent<LPSOTextManager>().GetLeadingMesh().text = string.Format("x{0} Combo", combo);
            }

            //Removing garbage data;
            PlayerMove.Instance.applesList = new List<GameObject>();
            //firstApple = 0;
            destinationColor = originalColor;
            destinationScale = originalSize;
            cooldown = 5;

            //Text Prompts;
            string appleText = "";

            if(applePile > 1) appleText = string.Format("{0} Apples!", applePile);
            else appleText = "1 Apple!";
            //GameManagerType.CreateGreenText(appleText, transform.position, 0.4f, 0.8f);
        }
        //}
    }
}
