using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SequenceGame : MonoBehaviour
{
    [SerializeField] private List<FeedbackContainer> feedbackContainers;
    [SerializeField] [Range(.1f, 3)] private float timeBetweenFeedback;
    [SerializeField] [Range(.2f, 5)] private float timeLimit;
    
    private List<FeedbackContainer> _currentSequence;
    private int _numContainers;
    private bool _isTimeUp;


    private void Awake()
    {
        _numContainers = feedbackContainers.Count;
        foreach (var feedbackContainer in feedbackContainers)
        {
            feedbackContainer.InitializeContainer();
        }
    }

    public void PlaySequence()
    {
        StartCoroutine(PlaySequenceCoroutine());
    }

    public void ReceiveSequence()
    {
        StartCoroutine(ReceiveSequenceCoroutine());
    }

    private IEnumerator PlaySequenceCoroutine()
    {
        _currentSequence.Add(GetRandomContainer());
        
        foreach (var feedbackContainer in _currentSequence)
        {
            feedbackContainer.Play();
            yield return new WaitForSeconds(timeBetweenFeedback);
        }
    }

    private IEnumerator ReceiveSequenceCoroutine()
    {
        while (!_isTimeUp)
        {
            
        }
        
        yield break;
    }

    private FeedbackContainer GetRandomContainer()
    {
        var randomIndex = Random.Range(0, _numContainers);
        return feedbackContainers[randomIndex];
    }

    private void RegisterFeedback(FeedbackContainer feedbackContainer)
    {
        
    }
}
