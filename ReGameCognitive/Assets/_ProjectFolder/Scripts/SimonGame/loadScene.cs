using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    public Object sceneToLoad;

    private void OnTriggerEnter()
    {
        SceneManager.LoadScene("simonThree");
    }
}
