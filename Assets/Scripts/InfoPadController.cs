using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPadController : MonoBehaviour
{

    public Text _plane_signature, _curve_signature, _point_signature;

    private string currentPlot;
    
    //anzupassende Textfelder auf dem Pad
    public Text _plane_f, _curve_f1, _curve_f2, _point_f1, _point_f2, _point_f3;
    

    private string plane_ftext, curve_f1text, curve_f2text, point_f1text, point_f2text, point_f3text;


    public GameObject _plane_panel, _curve_panel, _point_panel;
    private Text _current_signature;

    


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnScanChange += ChangeScanHandler;
        

    }

    public void SetPlaneF(string f)
    {
        plane_ftext = f;
    }
    public void SetCurveF(string f1, string f2)
    {
        curve_f1text = f1;
        curve_f2text = f2;
    }

    public void SetPointF(string f1, string f2, string f3)
    {
        point_f1text = f1;
        point_f2text = f2;
        point_f3text = f3;
    }



    private void ChangeScanHandler(float newVal)
    {
        _current_signature.text = newVal.ToString();
        if (currentPlot == "plane") 
        { 
            _plane_f.text = InsertValue_s(plane_ftext, newVal); 
        }
        else if (currentPlot == "curve")
        {
            _curve_f1.text = InsertValue_s(curve_f1text, newVal);
            _curve_f2.text = InsertValue_s(curve_f2text, newVal);
        }
        else if (currentPlot == "point")
        {
            _point_f1.text = InsertValue_s(point_f1text, newVal);
            _point_f2.text = InsertValue_s(point_f2text, newVal);
            _point_f3.text = InsertValue_s(point_f3text, newVal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string InsertValue_s(string f, float s)
    {
        string temp = f.Replace("sin", "_");
        temp = temp.Replace("cos", "#");
        temp = temp.Replace("s", s.ToString());
        temp = temp.Replace("#", "cos");
        temp = temp.Replace("_", "sin");
        return temp;
    }


    public void SetPlaneActive()
    {
        _curve_panel.SetActive(false);
        _point_panel.SetActive(false);
        _plane_panel.SetActive(true);
        _current_signature = _plane_signature;
        //_plane_f.text = plane_f.text;
        currentPlot = "plane";
    }
    public void SetCurveActive()
    {
        _point_panel.SetActive(false);
        _plane_panel.SetActive(false);
        _curve_panel.SetActive(true);
        _current_signature = _curve_signature;
        //_point_f1
        currentPlot = "curve";
    }
    public void SetPointActive()
    {
        _curve_panel.SetActive(false);
        _plane_panel.SetActive(false);
        _point_panel.SetActive(true);
        _current_signature = _point_signature;
        currentPlot = "point";
    }


}
