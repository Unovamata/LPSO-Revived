using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ApplePickup : MonoBehaviour{
    public static ApplePickup Instance;
    //Apple types for picking;
    public enum AppleType{Green = 0, Orange = 1, Red = 2, Yellow = 3, Null = 4}
    public AppleType color;

    //Movement;
    public int pilePosition; //Position in the pile of apples;
    [HideInInspector] public bool isPicked = false, canPickup = false, type = true;
    //Type = true; Counts to combo. Type = false; Doesn't counts to combo;

    //Animation;
    [HideInInspector] public Vector2 startPosition, endPosition, tempPosition, nextPos;
    float timeSkip, speed;

    private void Start() {
        speed = Random.Range(1.1f, 1.5f);
        startPosition = transform.position;
        timeSkip = 1;
        int[] choose = { 1, -1 };
        orientation = choose[Random.Range(0, 2)];
        startAngle = Random.Range(-20, 20);
    }

    [HideInInspector] public int bounces = 0;
    int orientation = 1;
    int startAngle;

    private void Update() {
        transform.eulerAngles = new Vector3(1, 1, ((360 * time) * orientation) + startAngle);

        if (PlayerMove.Instance.applesList.Contains(gameObject)) {
            time = 0;
            canPickup = true;
            Vector2 basket = PlayerMove.Instance.basketPosition;
            transform.position = new Vector2(basket.x, 0.35f + basket.y + (0.15f * pilePosition));
        }

        Vector3 middle;

        if(!canPickup){
            switch (bounces) {
                case 0:
                    middle = endPosition;
                    middle.x = endPosition.x * 2f;
                    Bounce(startPosition, middle, 1);
                    ResetBounce();
                break;

                case 1:
                    middle = endPosition;
                    middle.x = endPosition.x * 1.3f;
                    Bounce(tempPosition, middle, 0.6f);
                    ResetBounce();
                break;

                case 2:
                    Bounce(tempPosition, endPosition, 0.3f);
                    if(time >= 1) canPickup = true;
                break;
            }
        }
    }

    public float time = 0;
    
    protected void Bounce(Vector3 initialPosition, Vector3 destinationPosition, float arcH){
        if(time <= 1) {
            time += (Mathf.Min(time + 0.018f * timeSkip, 0.018f)) * speed;
            transform.position = nextPos;
        }

        //Setting the arc of the parabola;
        float arc = (1.0f - 4.0f * (time - 0.5f) * (time - 0.5f));

        //Creating the next position of the ball;
        nextPos = Vector3.Lerp(initialPosition, destinationPosition, time);
        nextPos.y += arc * arcH;
    }

    private void ResetBounce() {
        if(time >= 1){
            bounces++;
            tempPosition = transform.position;
            time = 0;
        }
    }
}
