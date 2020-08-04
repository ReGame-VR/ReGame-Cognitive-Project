using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : MonoBehaviour
{
    [SerializeField] private FinalSimon finalSimon;
    [SerializeField] private AudioClip[] beeps;
    

    public GameObject[] Cubes;
    public Material[] Color;
    public GameObject startLightGameObject;
    public GameObject Buttons;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlaySequence()
    {
        foreach (var cube in Cubes)
        {
            cube.SetActive(true);
        }
        
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++)
        {
            if (finalSimon)
            {
                AudioClip beep = null;
                if (beeps.Length > i) beep = beeps[i];
                finalSimon.PlayFeedback(i, beeps[i]);
                finalSimon.PlayFeedback(i + 3, beep);
                finalSimon.PlayFeedback(i + 6, beep);
                finalSimon.PlayFeedback(i + 9, beep);
            }
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
        Cubes[0].GetComponent<Renderer>().material = Color[0];
        Cubes[1].GetComponent<Renderer>().material = Color[3];
        Cubes[2].GetComponent<Renderer>().material = Color[6];
        Cubes[3].GetComponent<Renderer>().material = Color[9];
        Cubes[4].GetComponent<Renderer>().material = Color[1];
        Cubes[5].GetComponent<Renderer>().material = Color[4];
        Cubes[6].GetComponent<Renderer>().material = Color[7];
        Cubes[7].GetComponent<Renderer>().material = Color[10];
        Cubes[8].GetComponent<Renderer>().material = Color[2];
        Cubes[9].GetComponent<Renderer>().material = Color[5];
        Cubes[10].GetComponent<Renderer>().material = Color[8];
        Cubes[11].GetComponent<Renderer>().material = Color[11];
        startLightGameObject.SetActive(false);
        Buttons.SetActive(true);
        finalSimon.gameObject.SetActive(true);
    }

    void OnEnable()
    {
        //Sets colors: Red, Yellow, Green
        Cubes[0].GetComponent<Renderer>().material = Color[2];
        Cubes[1].GetComponent<Renderer>().material = Color[2];
        Cubes[2].GetComponent<Renderer>().material = Color[2];
        Cubes[3].GetComponent<Renderer>().material = Color[2];
        Cubes[4].GetComponent<Renderer>().material = Color[3];
        Cubes[5].GetComponent<Renderer>().material = Color[3];
        Cubes[6].GetComponent<Renderer>().material = Color[3];
        Cubes[7].GetComponent<Renderer>().material = Color[3];
        Cubes[8].GetComponent<Renderer>().material = Color[1];
        Cubes[9].GetComponent<Renderer>().material = Color[1];
        Cubes[10].GetComponent<Renderer>().material = Color[1];
        Cubes[11].GetComponent<Renderer>().material = Color[1];
        StartCoroutine(PlaySequence());
    }
}
