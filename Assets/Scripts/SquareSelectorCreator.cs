using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    [SerializeField] private Material freeSquareMaterial;
    [SerializeField] private Material opponentSquareMaterial;
    [SerializeField] private GameObject selectPrefab;
    private List<GameObject> instantiatedSelectors = new List<GameObject>();

    public void ShowSelection(Dictionary<Vector3, bool> quareData)
    {
        ClearSelection();
        foreach (var data in quareData)
        {
            GameObject selector = Instantiate(selectPrefab, data.Key, Quaternion.identity);
            instantiatedSelectors.Add(selector);
            foreach (var setter in selector.GetComponentsInChildren<MaterialSetter>())
            {
                setter.SetSingleMaterial(data.Value ? freeSquareMaterial : opponentSquareMaterial);
            }
            
        }
    }
    public void ClearSelection()
    {
        foreach (var selector in instantiatedSelectors)
        {
            Destroy(selector.gameObject);
        }
        instantiatedSelectors.Clear();
    }
}
