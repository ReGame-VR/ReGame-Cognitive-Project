using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelColors", order = 1)]
public class LevelColors : ScriptableObject
{
    [SerializeField] private Material levelMaterial;

    public void SetLevelColor(Transform parentTransform)
    {
        if (!parentTransform) return;
        
        var childCount = parentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = parentTransform.GetChild(i);
            SwapMaterial(child.gameObject);
            SetLevelColor(child);
        }
    }

    private void SwapMaterial(GameObject objectToChange)
    {
        if (!objectToChange) return;
        
        MeshRenderer meshRenderer = objectToChange.GetComponent<MeshRenderer>();
        if (!meshRenderer) return;
        
        Material[] oldMaterials = meshRenderer.materials;
        for(int i = 0; i < oldMaterials.Length; i++)
        {
            oldMaterials[i] = levelMaterial;
        }
        meshRenderer.materials = oldMaterials;
    }
}
