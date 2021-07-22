using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorManager : MonoBehaviour
{
    public GameObject vectorArea;

    public void ToggleArea()
    {
        vectorArea.SetActive(!vectorArea.activeSelf);
    }
}
