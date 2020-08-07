using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColors : MonoBehaviour
{
    public List<GameObject> objectsToChange = new List<GameObject>();
    public Material levelMaterial;
    public GameObject levelCube;

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

    public void ChangeAllObjectsMaterial()
    {
        foreach (var t in objectsToChange)
        {
            SwapMaterial(t);
        }
    }
}
