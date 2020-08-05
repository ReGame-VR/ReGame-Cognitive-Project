using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : MonoBehaviour
{
    [SerializeField] private FinalSimon finalSimon;
    [SerializeField] private Feedback greenfeedback;
    [SerializeField] private Feedback yellowfeedback;
    [SerializeField] private Feedback redfeedback;

    public GameObject[] Cubes;
    public GameObject startLightGameObject;
    public GameObject Buttons;

    private List<Feedback> _feedbacks = new List<Feedback>();

    // Start is called before the first frame update
    void Awake()
    {
        _feedbacks = new List<Feedback>()
        {
            redfeedback,
            yellowfeedback,
            greenfeedback,
        };
    }

    void OnEnable()
    {
        SetColumnColors();
        StartCoroutine(PlaySequence());
    }
    
    IEnumerator PlaySequence()
    {
        EnableCubes();
        
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++)
        {
            if (finalSimon)
            {
                PlayColumnFeedback(i);
            }
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
        
        startLightGameObject.SetActive(false);
        Buttons.SetActive(true);
        finalSimon.gameObject.SetActive(true);
    }

    private void EnableCubes()
    {
        foreach (var cube in Cubes)
        {
            cube.SetActive(true);
        }
    }

    private void PlayColumnFeedback(int i)
    {
        finalSimon.PlayFeedback(i, _feedbacks[i], false);
        finalSimon.PlayFeedback(i + 3, _feedbacks[i], false);
        finalSimon.PlayFeedback(i + 6, _feedbacks[i], false);
        finalSimon.PlayFeedback(i + 9, _feedbacks[i], false);
    }

    private void SetColumnColors()
    {
        for (int i = 0; i < 3; i++)
        {
            if (finalSimon)
            {
                finalSimon.StopFeedback(i, _feedbacks[i]);
                finalSimon.StopFeedback(i + 3, _feedbacks[i]);
                finalSimon.StopFeedback(i + 6, _feedbacks[i]);
                finalSimon.StopFeedback(i + 9, _feedbacks[i]);
            }
        }
    }
}
