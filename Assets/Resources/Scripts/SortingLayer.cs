using UnityEngine;
using System.Collections;

public class SortingLayer : MonoBehaviour
{

    public string SortingLayerName = "Default";
    public int SortingOrder;

    void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }
}
