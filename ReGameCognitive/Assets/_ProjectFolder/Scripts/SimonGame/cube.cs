using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    public GameObject cubeObject;
    public Material color;
    public Material colorPressed;
    public SimonPrototype simonPrototype;
    public int cubeNumber;
    private AudioSource beep;

    // Start is called before the first frame update
    void Start()
    {
        beep = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter()
    {
        cubeObject.GetComponent<Renderer>().material = colorPressed;
        beep.Play();
        simonPrototype.pushed = cubeNumber;
    }

    private void OnTriggerExit()
    {
        cubeObject.GetComponent<Renderer>().material = color;
        simonPrototype.pushed = 0;
    }
}
