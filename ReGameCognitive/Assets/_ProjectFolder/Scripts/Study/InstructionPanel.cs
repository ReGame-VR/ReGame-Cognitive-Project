using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPanel : MonoBehaviour
{
    [SerializeField] private SimonGame simonGame;
    [SerializeField] private CustomTextCanvas customTextCanvas;
    [SerializeField] private string instructionBeforeColor;
    [SerializeField] private string instructionAfterColor;
    [SerializeField] private string instructionBeforeKey;
    [SerializeField] private string instructionAfterKey;

    public void SetDifficulty(Difficulty difficulty)
    {
        if (!difficulty || !customTextCanvas) return;

        if (simonGame.usePredeterminedSequences)
        {
            customTextCanvas.SetBody(instructionBeforeKey + $" {difficulty.keyLetter} key " + "for the" + $" {difficulty.colorString} \n" + instructionAfterKey);
            customTextCanvas.Enable();
        }
        else
        {
            customTextCanvas.SetBody(instructionBeforeColor + $" {difficulty.colorString} " + instructionAfterColor);
            customTextCanvas.Enable();
        }
        
    }

    public void FinalInstructions()
    {
        if (!customTextCanvas) return;

        if (simonGame.usePredeterminedSequences)
        {
            customTextCanvas.SetBody("Select the level that the researcher suggested.\n" +
                                     "Press the 'B' for Blue, 'G' for Green, 'R' for Red,\n" +
                                     "'Y' for Yellow, and 'O' for Orange");
            customTextCanvas.Enable();
        }
        else
        {
            customTextCanvas.SetBody("Select the level that the researcher suggested.");
            customTextCanvas.Enable();
        }
    }

    public void Disable()
    {
        if (!customTextCanvas) return;
        
        customTextCanvas.Disable();
    }
    
    public IEnumerator EndOfStudy(float timeToWait)
    {
        customTextCanvas.Enable();

        if (simonGame.usePredeterminedSequences)
        {
            customTextCanvas.SetBody("Nice work! This is the end.\nPlease talk to the researcher.");
        }
        else
        {
            customTextCanvas.SetBody("Nice work! This is the end.\nPlease take off the headset and talk to the researcher.");
        }
        
        while (true)
        {
            yield return new WaitForSeconds(timeToWait);
        }
    }
}
