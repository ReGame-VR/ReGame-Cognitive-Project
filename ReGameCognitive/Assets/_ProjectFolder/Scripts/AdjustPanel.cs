using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustPanel : MonoBehaviour
{
    [SerializeField] private Transform simonGamePanel;
    [SerializeField] private Transform ui;
    [SerializeField] private Transform player;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float gamePanelModifier;
    [SerializeField] private float uiPanelModifier;
    [SerializeField] private float timetowait;
    

    private void Start()
    {
        StartCoroutine(WaitToSetHead());
    }

    private void Update()
    {
        
    }

    private IEnumerator WaitToSetHead()
    {
        yield return new WaitForSeconds(timetowait);
        MovePanel();
    }

    private void MovePanel()
    {
        simonGamePanel.position = new Vector3(simonGamePanel.position.x, simonGamePanel.position.y, player.position.z + distanceFromPlayer + gamePanelModifier);
        ui.position = new Vector3(ui.position.x, ui.position.y, player.position.z + distanceFromPlayer + uiPanelModifier);
    }
}
