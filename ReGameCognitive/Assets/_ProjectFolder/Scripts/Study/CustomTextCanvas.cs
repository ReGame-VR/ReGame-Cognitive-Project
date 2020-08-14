using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CustomTextCanvas : MonoBehaviour
{
    [SerializeField] private GameObject canvasParentGameObject;
    [SerializeField] private TextMesh titleTextMesh;
    [SerializeField] private TextMesh bodyTextMesh;

    public void Enable()
    {
        canvasParentGameObject.SetActive(true);
    }

    public void Disable()
    {
        canvasParentGameObject.SetActive(false);
    }

    public void SetTitle(string text)
    {
        titleTextMesh.text = text;
    }

    public void SetBody(string text)
    {
        bodyTextMesh.text = text;
    }
    
    public void AppendBody(string text)
    {
        bodyTextMesh.text += text;
    }

    public void ClearBody()
    {
        bodyTextMesh.text = "";
    }

    public int GetBodyTextLength()
    {
        return bodyTextMesh.text.Length;
    }

    public static string FormatTimeToString(float time)
    {
        var minutes = ((int)time / 60).ToString();
        var seconds = (time % 60).ToString("00");
        var timeString = minutes + ":" + seconds;
        return timeString;
    }

    public static string FormatDecimalToPercent(float someFloat)
    {
        var percentString = someFloat.ToString("P1");
        return percentString;
    }
}
