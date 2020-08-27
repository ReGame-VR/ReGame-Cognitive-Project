using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPanel : MonoBehaviour
{
    [SerializeField] private CustomTextCanvas customTextCanvas;
    [SerializeField] private string instructionBeforeColor;
    [SerializeField] private string instructionAfterColor;

    
    public void SetDifficulty(Difficulty difficulty)
    {
        if (!difficulty || !customTextCanvas) return;
        
        customTextCanvas.SetBody(instructionBeforeColor + $" {difficulty.colorString} " + instructionAfterColor);
        customTextCanvas.Enable();
    }

    public void Disable()
    {
        if (!customTextCanvas) return;
        
        customTextCanvas.Disable();
    }
}
