using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_controller : MonoBehaviour
{
    public string scene_name;

    public void change_scene(string levelName = "")
    {
        if (levelName != "")
        {
            //SceneManager.LoadScene(cosa);
            LoadingManager.Instance.Load(levelName);
        }
    }
}
