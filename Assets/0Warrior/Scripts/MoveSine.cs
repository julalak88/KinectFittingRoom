using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSine : MonoBehaviour
{
    public Vector2 power;
    public float speed;

    Vector2 startPosition;
    RectTransform rectTransform;

    int cc = 19;
    float _power;

    private void Awake() {
        rectTransform = (RectTransform)transform;
        startPosition = rectTransform.anchoredPosition;
    }
    
    void Update()
    {
        //if(++cc == 20) {
            //cc = 0;
            _power = UnityEngine.Random.Range(power.x, power.y);
        //}
        rectTransform.anchoredPosition = startPosition + new Vector2(Mathf.Sin(Time.time * speed) * _power, Mathf.Sin(Time.time * speed) * _power);
    }
}
