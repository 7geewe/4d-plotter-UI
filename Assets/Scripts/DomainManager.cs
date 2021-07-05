using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainManager : MonoBehaviour
{
    public GameObject domainArea;

    public void ToggleArea()
    {
        domainArea.SetActive(!domainArea.activeSelf);
    }

}
