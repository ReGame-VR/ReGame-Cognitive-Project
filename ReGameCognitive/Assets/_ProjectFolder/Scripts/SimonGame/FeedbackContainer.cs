using Sirenix.OdinInspector;
using UnityEngine;

public class FeedbackContainer : MonoBehaviour
{
    [SerializeField] private Feedback feedback;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Material material;
    

    public delegate void SelectHandler();

    public event SelectHandler ContainerWasSelected;


    [Button]
    public void InitializeContainer()
    {
        if (!feedback) return;

        if (!material)
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                material = meshRenderer.sharedMaterial;
            }
        }
        
        if (material)
        {
            material.color = feedback.unlitColor;
        }

        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    
    [Button]
    public void Play(bool playHaptics)
    {
        if (feedback)
        {
            feedback.Play(audioSource, material, playHaptics);
        }
    }

    public void SelectContainer()
    {
        ContainerWasSelected?.Invoke();
    }
}
