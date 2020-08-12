using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Feedback", order = 1)]
public class Feedback : ScriptableObject
{
    public Color litColor;
    public Color unlitColor;
    public AudioClip audioClip;
    [Range(0, 1)] public float volume;
    [Range(-3, 3)] public float pitch;
    [Range(0, 1)] public float hapticAmplitude;


    public void Play(AudioSource audioSource, Material material, bool playHaptics)
    {
        if (audioSource)
        {
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();
        }

        if (material)
        {
            material.color = litColor;
        }
        
        if (playHaptics)
        {
            ControllerHaptics.ActivateHaptics(hapticAmplitude, 1, true);
            ControllerHaptics.ActivateHaptics(hapticAmplitude, 1, false);
        }
    }

    public void Stop(AudioSource audioSource, Material material)
    {
        if (audioSource)
        {
            audioSource.Stop();
        }

        if (material)
        {
            material.color = unlitColor;
        }

        ControllerHaptics.ActivateHaptics(0, 0, true);
        ControllerHaptics.ActivateHaptics(0, 0, false);
    }
}
