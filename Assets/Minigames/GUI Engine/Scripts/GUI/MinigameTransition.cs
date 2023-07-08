using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTransition : MonoBehaviour{
    /*
     * Containers: They hold the transition instances until the specific time has been reached for destroying itself;
     * Instances: The objects that will constitute the transition, they have the specific parameters to know where to 
                  destruct;

        For example: In the Fruitista Fetch Minigame there's an invisible basket holding all the apples falling from
        the top of the screen, that's the container;
        The apples themselves are the instances;
     */
    public enum ObjectType {Container, Instance};
    [SerializeField] protected ObjectType type;

    //Instance management;
    [SerializeField] protected List<GameObject> instanceList = new List<GameObject>(); //Objects to hold inside the container;
    [SerializeField] protected Vector2 instanceCreatePosition, instanceDestroyPosition;


    //Transition duration;
    [SerializeField] protected float transitionDurationInSeconds;
    protected float deltaTimeTimer;
    protected bool isCoroutineRunning;

    protected void Countdown(){ deltaTimeTimer += Time.deltaTime; }
    protected bool IsTimeUp(){ return deltaTimeTimer > transitionDurationInSeconds; }

    //Positioning;
    protected Transform transformReference;

    //Minigame Reference
    protected MinigameType minigameReference;

    void Start(){ 
        transformReference = transform; 
        minigameReference = MinigameType.Instance;
    }

    // Update is called once per frame
    void Update(){ Countdown(); }
}
