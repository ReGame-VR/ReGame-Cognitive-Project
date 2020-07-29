using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadHard : MonoBehaviour
{

    private void OnTriggerEnter()
    {
        SceneManager.LoadScene("simonFive");
    }
}
