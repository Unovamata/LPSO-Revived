using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {
    [HideInInspector] public static PlayerMove Instance;
    [HideInInspector] public Vector2 basketPosition;
    Vector2 startPosition;
    public List<GameObject> applesList;

    public GameObject AppleCollect;
    public GameObject Apple, BasketApple, CollectPrefab, ShadowPrefab;

    //Moving the area;
    [HideInInspector] public GameObject CollectorArea, ShadowBasket;
    public List<ParticleSystem> rings;
    [HideInInspector] public SpriteRenderer ringRender;
    private readonly float smoothTime = 0.06f;
    private readonly float basketYSeparator = 0.4f; //Separation between the basket and the object's pivot;

    ////////////////////////////////////////////////////////////////////////////////

    private void Awake() {
        Instance = this;
    }
    
    Transform basketTransform;

    private void Start() {
        basketTransform = transform;

        startPosition = new Vector2(basketTransform.position.x, basketTransform.position.y - basketYSeparator);
        CollectorArea = Instantiate(CollectPrefab, startPosition, basketTransform.rotation, basketTransform);
        ShadowBasket = Instantiate(ShadowPrefab, startPosition, basketTransform.rotation, basketTransform);
    }


    ////////////////////////////////////////////////////////////////////////////////

    void Update() {
        basketPosition = basketTransform.position;
        if(!GameManagerType.Instance.canLoadResultsScreen) {
            if(GameManagerType.Instance.gameStart) PlayerFollowMouse(true);
        } else {
            transform.position = startPosition;
            applesList = new List<GameObject>();
        }
    }


    ////////////////////////////////////////////////////////////////////////////////


    //Functions;
    //PlayerFollowMouse();
    private void PlayerFollowMouse(bool start) {
        Vector2 mousePosition = start ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : basketPosition;

        transform.position = start ? Vector2.Lerp(basketPosition, mousePosition, smoothTime) : basketPosition;
    }
}
