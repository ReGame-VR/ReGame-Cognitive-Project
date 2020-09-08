using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPanel : MonoBehaviour
{
    [SerializeField] private CustomTextCanvas customTextCanvas;
    [SerializeField] private string instructionBeforeColor;
    [SerializeField] private string instructionAfterColor;
    [SerializeField] private string instructionBeforeKey;
    [SerializeField] private string instructionAfterKey;

    private const string FINAL_INSTRUCTIONS_PC = "Select the level that the researcher suggested.\n" +
                                                 "Press the 'B' for <color=blue>Blue</color>, 'G' for <color=green>Green</color>, 'R' for <color=red>Red</color>,\n" +
                                                 "'Y' for <color=yellow>Yellow</color>, and 'O' for <color=orange>Orange</color>";
    private const string FINAL_INSTRUCTIONS_VR = "Select the level that the researcher suggested.";
    private const string END_OF_STUDY_PC = "Nice work! This is the end.\nPlease talk to the researcher.";
    private const string END_OF_STUDY_VR = "Nice work! This is the end.\nPlease take off the headset and talk to the researcher."; 

    public void SetDifficulty(Difficulty difficulty, bool isVrVersion)
    {
        if (!difficulty || !customTextCanvas) return;

        var vrString = instructionBeforeColor + $" {difficulty.colorString} " + instructionAfterColor;
        var pcString = instructionBeforeKey + $" {difficulty.keyLetter} key " + "for the" + 
                       $" <color={difficulty.colorString.ToLower()}>{difficulty.colorString}</color> \n" + instructionAfterKey;
        var setDifficultyString = isVrVersion ? vrString : pcString;
        
        customTextCanvas.SetBody(setDifficultyString);
        customTextCanvas.Enable();
    }

    public void FinalInstructions(bool isVrVersion)
    {
        if (!customTextCanvas) return;

        var finalInstructionsString = isVrVersion ? FINAL_INSTRUCTIONS_VR : FINAL_INSTRUCTIONS_PC;
        customTextCanvas.SetBody(finalInstructionsString);
        customTextCanvas.Enable();
    }

    public void Disable()
    {
        if (!customTextCanvas) return;
        
        customTextCanvas.Disable();
    }
    
    public void EndOfStudy(bool isVrVersion)
    {
        if (!customTextCanvas) return;
        
        var endOfStudyString = isVrVersion ? END_OF_STUDY_VR : END_OF_STUDY_PC;
        customTextCanvas.SetBody(endOfStudyString);
        customTextCanvas.Enable();
    }
}
