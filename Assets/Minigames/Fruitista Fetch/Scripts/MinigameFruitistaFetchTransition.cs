using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameFruitistaFetchTransition : MinigameTransition {
    int applesCreated = 0, applesToCreate = 70;
    //[SerializeField] GameObject countdown;
    //[SerializeField] AudioClip transitionSound;

    // Update is called once per frame
    void LateUpdate(){
        //Check the transition instance type;
        switch(type){
            case ObjectType.Container:
                if(!isCoroutineRunning) StartCoroutine(SpawnApples());

                if(IsTimeUp()){
                    try{
                        MinigameStates currentState = minigameReference.GetCurrentState();

                        if(currentState == MinigameStates.MinigameInTransition){
                            minigameReference.SetCurrentState(MinigameStates.MinigameEndScreen);
                        } else {
                            minigameReference.SetCurrentState(MinigameStates.MinigamePlaying);
                        }
                    } catch { }

                    Destroy(gameObject);
                }
            break;

            case ObjectType.Instance:
                Vector2 position = transformReference.position;

                if(position.y < instanceDestroyPosition.y) Destroy(gameObject);
            break;
        }
    }

    private IEnumerator SpawnApples(){
        isCoroutineRunning = true;

        while(applesCreated < applesToCreate){
            GameObject appleReference = instanceList[Random.Range(0, instanceList.Count - 1)];
            GameObject appleToCreate = Instantiate(appleReference, instanceCreatePosition, Quaternion.identity, transform.parent);

            appleToCreate.GetComponent<SpriteRenderer>().sortingOrder += applesCreated;
            applesCreated++;
            yield return null;
        }
    }
}
