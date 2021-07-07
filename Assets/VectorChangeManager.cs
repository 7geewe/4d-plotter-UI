using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VectorChangeManager : MonoBehaviour
{

    public Text _xText, _yText, _zText;
    private float _x, _y, _z;

    public Button _decX, _decY, _decZ;

    // Start is called before the first frame update
    void Start()
    {
        _x = 1f;
        _y = 1f;
        _z = 1f;

        _xText.text = _x.ToString();
        _yText.text = _y.ToString();
        _zText.text = _z.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseX()
    {
        _x += 0.1f;
        _decX.interactable = true;
        _xText.text = _x.ToString();
    }
    public void DecreaseX()
    {
        _x -= 0.1f;

        _xText.text = _x.ToString();
        if (_x <= 0.11f) _decX.interactable = false;
    }
    public void IncreaseY()
    {
        _y += 0.1f;
        _decY.interactable = true;
        _yText.text = _y.ToString();
    }
    public void DecreaseY()
    {
        _y -= 0.1f;
        if (_y <= 0.11f) _decY.interactable = false;
        _yText.text = _y.ToString();
    }
    public void IncreaseZ()
    {
        _z += 0.1f;
        _decZ.interactable = true;
        _zText.text = _z.ToString();
    }
    public void DecreaseZ()
    {
        _z -= 0.1f;
        if (_z <= 0.11f) _decZ.interactable = false;
        _zText.text = _z.ToString();
    }
}
