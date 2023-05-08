using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketWaveMovement : MonoBehaviour{
    Vector3 startPos;
    Transform t;

    private void Start() {
        startPos = transform.position;
        t = transform;
    }

    [SerializeField] float amplitude = 1, speed = 1;

    void Update(){
        float sin = amplitude * Mathf.Sin(Time.time / speed);
        Vector3 pos = new Vector3(startPos.x, startPos.y + sin, startPos.z);
        t.position = pos;
    }
}
