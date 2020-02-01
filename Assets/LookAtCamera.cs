using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        if(Camera.main != null) transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }
}