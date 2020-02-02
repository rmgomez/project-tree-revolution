using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_controller : MonoBehaviour
{
    public string scene_name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void change_scene(string cosa = "")
    {
        if (cosa != "")
        {
            SceneManager.LoadScene(cosa);
        }
    }
}
