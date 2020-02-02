﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fx_anim_up : MonoBehaviour
{
    public float get_window_size = 0;
    public float initial_x       = 0;

    // Start is called before the first frame update
    void Start()
    {
        get_window_size = transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        // initial_x       = GetComponent<RectTransform>().localPosition.x;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void move_up() {
        Debug.Log("...");
        StartCoroutine(MoveFromTo(gameObject.GetComponent<RectTransform>(), 0, get_window_size, 100f  ));
    }

    IEnumerator MoveFromTo(RectTransform objectToMove, float a, float b, float speed)
    {
        // float step = (speed / (a - b)) * Time.fixedDeltaTime;
        float step = 0.1f;
        float t = 0;

        Vector2 aa = new Vector2(0, 0);
        Vector2 bb = new Vector2(0, get_window_size);

        while (t <= 1.0f) {
            
            Debug.Log(t);

            t += step; // Goes from 0 to 1, incrementing by step each time
            // objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            // objectToMove.anchoredPosition = new Vector2(0, Vector3.Lerp(a, b, t)); 
            // objectToMove.anchoredPosition = new Vector2(0, Vector3.Lerp(a, b, t)); 
            objectToMove.anchoredPosition = Vector2.Lerp(aa, bb, t);
            yield return new WaitForFixedUpdate();
        }
        objectToMove.anchoredPosition = new Vector2(0, get_window_size);
 }
}
