using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTransition : MonoBehaviour{
    public enum ObjectType {Bucket = 0, Apple = 1};
    public ObjectType type;
    [SerializeField] List<GameObject> physicsApples = new List<GameObject>();
    [SerializeField] Transform transform;
    int applesCreated = 0;
    [SerializeField] Vector3 createPosition, originalPosition;
    [HideInInspector] public bool restorePosition = false;
    public bool isGameEndTransition = false;
    Rigidbody2D rb;
    int cooldown = 0, originalCooldown = 300;
    [SerializeField] GameObject countdown;
    //[SerializeField] AudioClip transitionSound;

    void Start(){
        transform = gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
        cooldown = originalCooldown;
        //GameManagerType.Instance.sfx2.PlayOneShot(transitionSound);
        //If the game has already started, then it is the end game transition;
        //if(GameManagerType.Instance.gameStart) isGameEndTransition = true;
    }

    // Update is called once per frame
    void LateUpdate(){
        //If it's an apple, destroy it after a certain y axis;
        if(transform.localPosition.y < -600f && (int)type == 1) Destroy(gameObject);
        //But if it's a bucket;
        else if((int)type == 0) {
            if(applesCreated < 70){
                Instantiate(physicsApples[Random.Range(0, physicsApples.Count)], createPosition, Quaternion.identity, transform.parent).
                    GetComponent<SpriteRenderer>().sortingOrder += applesCreated;
                
                applesCreated++;
            } else {
                if(cooldown > 0) cooldown--;
                else {
                    MinigameType game = MinigameType.Instance;

                    //Ending the game and transition;
                    game.SetHasTransitionEnded(true);
                    if(isGameEndTransition){
                        FruitistaFetchGameManager.Instance.SendScores();
                        game.SetHasMinigameEnded(true);
                    } else {
                        MinigameType.Instance.SetIsMinigameReady(true);
                        //Instantiate(countdown, game.canvas);
                    }
                    
                    //game.playPressed = false;
                    Destroy(gameObject);
                }
            }
        }
    }
}
