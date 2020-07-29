using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyManager : MonoBehaviour
{
    [SerializeField] private CSVManager csvManager;

    private User _currentUser;
    private Session _currentSession;
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartStudy();
        EndStudy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartStudy()
    {
        _currentUser = new User();
        csvManager.Initialize(_currentUser);
    }

    private void EndStudy()
    {
        csvManager.UpdateData(_currentUser);
        csvManager.AppendReport();
    }

    private void StartSession()
    {
        if (_currentSession == null) return;
        
        _currentSession = new Session(_currentUser);
    }

    private void EndSession()
    {
        
    }

    private void StoreData()
    {
        
    }
}
