using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class start : MonoBehaviour
{
    public Text getReady;
    public Text score;
    public Text number;
    public GameObject simon;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(countdown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator countdown()
    {
        yield return new WaitForSeconds(3);
        getReady.text = "3";
        yield return new WaitForSeconds(1);
        getReady.text = "2";
        yield return new WaitForSeconds(1);
        getReady.text = "1";
        yield return new WaitForSeconds(1);
        getReady.text = "";
        score.text = "Round: ";
        number.text = "1";
        simon.GetComponent<SimonPrototype>().enabled = true;
    }
}
