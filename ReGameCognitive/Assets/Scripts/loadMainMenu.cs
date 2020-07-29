using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadMainMenu : MonoBehaviour
{

    private void OnTriggerEnter()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
