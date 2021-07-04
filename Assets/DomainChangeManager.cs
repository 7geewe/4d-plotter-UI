using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DomainChangeManager : MonoBehaviour
{
    private float _min, _max;
    public Text min, max;


    private void Start()
    {
        _min = -2f;
        _max = 2f;
        min.text = _min.ToString();
        max.text = _max.ToString();
    }

    public void increaseMin()
    {
        if (_min + 0.5f < _max)
        {
            _min += 0.5f;
            min.text = _min.ToString();
        }
    }

    public void increaseMax()
    {
        _max += 0.5f;
        max.text = _max.ToString();
    }

    public void decreaseMin()
    {
        _min -= 0.5f;
        min.text = _min.ToString();
    }

    public void decreaseMax()
    {
        if (_max - 0.5f > _min)
        {
            _max -= 0.5f;
            max.text = _max.ToString();
        }
    }


}
