using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadEasy : MonoBehaviour
{

    private void OnTriggerEnter()
    {
        SceneManager.LoadScene("simonThree");
    }
}
