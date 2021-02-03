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
    [SerializeField] private float timeToWait;

    private float _uiPositionY;
    private float _uiPositionX;
    private float _simonGamePanelPositionX;
    private float _simonGamePanelPositionY;
    private bool _adjustmentHasBeenDisabled;


    private void Awake()
    {
        if (!ui)
        {
            Debug.Log($"{nameof(Awake)} | ERROR: {nameof(ui)} is null. Must be set in editor.");
            return;
        }

        var uiPosition = ui.position;
        _uiPositionX = uiPosition.x;
        _uiPositionY = uiPosition.y;
        
        if (!simonGamePanel)
        {
            Debug.Log($"{nameof(Awake)} | ERROR: {nameof(simonGamePanel)} is null. Must be set in editor.");
            return;
        }

        var simonGamePanelPosition = simonGamePanel.position;
        _simonGamePanelPositionX = simonGamePanelPosition.x;
        _simonGamePanelPositionY = simonGamePanelPosition.y;
        
        OVRManager.HMDMounted += MovePanel;
    }

    private void Start()
    {
        StartCoroutine(WaitToSetHead());
    }

    private void OnDestroy()
    {
        OVRManager.HMDMounted -= MovePanel;
    }

    private IEnumerator WaitToSetHead()
    {
        yield return new WaitForSeconds(timeToWait);
        MovePanel();
    }

    private void MovePanel()
    {
        if (_adjustmentHasBeenDisabled) return;
        
        var playerPositionZ = player.position.z;
        
        var simonGamePanelAdjustedZ = playerPositionZ + distanceFromPlayer + gamePanelModifier;
        var simonGamePanelAdjustedPosition = new Vector3(_simonGamePanelPositionX, _simonGamePanelPositionY, simonGamePanelAdjustedZ);
        simonGamePanel.position = simonGamePanelAdjustedPosition;

        var uiAdjustedZ = playerPositionZ + distanceFromPlayer + uiPanelModifier;
        var uiAdjustedPosition = new Vector3(_uiPositionX, _uiPositionY, uiAdjustedZ);
        ui.position = uiAdjustedPosition;
    }

    public void DisableAdjustment()
    {
        _adjustmentHasBeenDisabled = true;
    }
}
