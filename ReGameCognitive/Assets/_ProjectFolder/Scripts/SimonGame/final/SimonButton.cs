using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour
{
    public FinalSimon FinalSimon;
    public int CubeNumber;
    public GameObject Button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter()
    {
        Button.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        FinalSimon.ButtonPushed = CubeNumber;
    }

    private void OnTriggerExit()
    {
        Button.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        FinalSimon.ButtonPushed = 0;
    }
}
