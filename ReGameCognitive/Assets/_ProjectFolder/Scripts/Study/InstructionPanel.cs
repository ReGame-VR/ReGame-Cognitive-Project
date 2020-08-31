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

    public void FinalInstructions()
    {
        if (!customTextCanvas) return;
        
        customTextCanvas.SetBody("Select the level that the researcher suggested.");
        customTextCanvas.Enable();
    }

    public void Disable()
    {
        if (!customTextCanvas) return;
        
        customTextCanvas.Disable();
    }
    
    public IEnumerator EndOfStudy(float timeToWait)
    {
        customTextCanvas.Enable();
        customTextCanvas.SetBody("Nice work! This is the end.\nPlease take off the headset and talk to the researcher.");

        while (true)
        {
            yield return new WaitForSeconds(timeToWait);
        }
    }
}
