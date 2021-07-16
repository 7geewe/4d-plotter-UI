using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPadController : MonoBehaviour
{

    public Text _sText;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnScanChange += ChangeScanHandler;
    }

    private void ChangeScanHandler(float newVal)
    {
        _sText.text = newVal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
