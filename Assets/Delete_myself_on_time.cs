using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete_myself_on_time : MonoBehaviour
{
    float time = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0) { Destroy(gameObject); }
    }
}
