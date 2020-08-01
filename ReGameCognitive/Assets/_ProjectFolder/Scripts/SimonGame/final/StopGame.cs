using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGame : MonoBehaviour
{
    public GameObject[] Cubes;
    public Material[] Color;
    public GameObject DifficultyButtons;
    public GameObject StopLights;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(PlaySequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlaySequence()
    {
        for (int i = 0; i < 12; i++)
        {
            Cubes[i].GetComponent<Renderer>().material = Color[2];
            Cubes[i].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        }

        yield return new WaitForSeconds(4);

        for (int i = 0; i < 5; i++)
        {
            Cubes[i].GetComponent<Renderer>().material = Color[i];
        }

        for (int i = 5; i < 12; i++)
        {
            Cubes[i].GetComponent<Renderer>().material = Color[6];
        }

        DifficultyButtons.SetActive(true);
        StopLights.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(PlaySequence());
    }
}
