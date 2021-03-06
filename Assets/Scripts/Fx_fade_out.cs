﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fx_fade_out : MonoBehaviour {

    Image img;
    public float time = 2f;
    float time_initial;

    private void Awake()
    {
        img = gameObject.GetComponent<Image>();
        img.color = new Color(0, 0, 0, 1);
    }

    // Use this for initialization
    void Start()
    {
        time_initial = time;
        

        if (img != null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            img.color = new Color(0, 0, 0, (time / time_initial));
            time -= Time.deltaTime;
        }
    }

}