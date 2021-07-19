using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager instance = null;


    //plotting
    public int _plotAccuracy = 35;
    
    
    //box
    public GameObject _hologramBox;
    public Vector3 _boxPos, _boxScale;
    private AxisLabeling _axisLabeling;
    private float _moveSpeed = 8f;

    //scanning
    public float _scanningSpeed = 40f;
    public int _framesPerScan = 5;

    //plane
    private PlotR3toR _plane;
    private string _f;
    private Vector2 _domain_s_plane, _domain_x_plane, _domain_z_plane;
    private bool _scaleDirectionEnabled;
    private Vector3 _scaleDirection;

    //curve
    private PlotR2toR2 _curve;
    private string _f1_curve, _f2_curve;
    private Vector2 _domain_s_curve, _domain_x_curve;

    //point
    private PlotRtoR3 _point;
    private string _f1_point, _f2_point, _f3_point;
    private Vector2 _domain_s_point;

    //all
    private Vector3 _range_min = new Vector3(-2f, -4f, -2f), _range_max = new Vector3(2f, 4f, 2f);
    private float _s = 0;
    private Vector2 _s_current_domain = new Vector2(-2f, 2f);
    private ScalingPlot _activePlot;


    //events
    public float scan
    {
        get { return _s; }
        set
        {
            if (_s == value) return;
            _s = value;
            OnScanChange?.Invoke(_s);
        }
    }

    public delegate void OnScanChangeDelegate(float newVal);
    public event OnScanChangeDelegate OnScanChange;

    //Singleton
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        _boxPos = _hologramBox.transform.position;
        _boxScale = _hologramBox.transform.localScale;
    }





    // Start is called before the first frame update
    void Start()
    {
        //initialize box labeling
        _axisLabeling = _hologramBox.GetComponent<AxisLabeling>();
        _axisLabeling.UpdateLabeling(_range_min, _range_max);





    }

    public void ScanPlot(float s)
    {
        scan += s * Time.deltaTime / (_s_current_domain.y - _s_current_domain.x) * _scanningSpeed / (float)_framesPerScan;
        if(!_scaleDirectionEnabled && _s > _s_current_domain.y)
        {
            scan -= (_s_current_domain.y - _s_current_domain.x);
        }
        else if (!_scaleDirectionEnabled && _s < _s_current_domain.x)
        {
            scan += (_s_current_domain.y - _s_current_domain.x);
        }



        if (_activePlot != null)
        {
            if (!(_activePlot is PlotRtoR3)) Destroy(_activePlot.plot.curves);
            _activePlot.UpdatePlot(_s);
            Debug.Log($"changed s-value to {_s}");
        }
    }


    public void SetPlaneActive()
    {
        if (_plane != null) _plane.plot.curves.SetActive(true);
        if (_curve != null) _curve.plot.curves.SetActive(false);
        if (_point != null) _point.plot.curves.SetActive(false);

        _activePlot = _plane;
        _s_current_domain = _domain_s_plane;

    }

    public void SetCurveActive()
    {
        if (_plane != null) _plane.plot.curves.SetActive(false);
        if (_curve != null) _curve.plot.curves.SetActive(true);
        if (_point != null) _point.plot.curves.SetActive(false);

        _activePlot = _curve;
        _s_current_domain = _domain_s_curve;

    }

    public void SetPointActive()
    {
        if (_plane != null) _plane.plot.curves.SetActive(false);
        if (_curve != null) _curve.plot.curves.SetActive(false);
        if (_point != null) _point.plot.curves.SetActive(true);

        _activePlot = _point;
        _s_current_domain = _domain_s_point;

    }



    public void UpdatePlane(string f, Vector2 domain_s, Vector2 domain_x, Vector2 domain_z)
    {
        UpdatePlane(f, domain_s, domain_x, domain_z, Vector3.zero);
    }
    
    
    public void UpdatePlane(string f, Vector2 domain_s, Vector2 domain_x, Vector2 domain_z, Vector3 direction)
    {
        _f = f;
        _domain_s_plane = domain_s;
        _domain_x_plane = domain_x;
        _domain_z_plane = domain_z;
        _scaleDirection = direction;
        _scaleDirectionEnabled = (direction != Vector3.zero);
        //Debug.Log(_scaleDirection);
        //Debug.Log(_scaleDirectionEnabled);


        if (_plane != null)
        {
            Destroy(_plane.plot.curves);
        }

        _plane = CreatePlane();

        SetPlaneActive();
        scan = (_s_current_domain.x + _s_current_domain.y) / 2;
    }

    public void UpdateCurve(string f1, string f2, Vector2 domain_s, Vector2 domain_x)
    {
        _f1_curve = f1;
        _f2_curve = f2;
        _domain_s_curve = domain_s;
        _domain_x_curve = domain_x;

        if(_curve != null)
        {
            Destroy(_curve.plot.curves);
        }

        _curve = CreateCurve();

        SetCurveActive();
        scan = (_s_current_domain.x + _s_current_domain.y) / 2;
    }

    public void UpdatePoint(string f1, string f2, string f3, Vector2 domain_s)
    {
        _f1_point = f1;
        _f2_point = f2;
        _f3_point = f3;
        _domain_s_point = domain_s;

        if(_point != null)
        {
            Destroy(_point.plot.curves);
        }

        _point = CreatePoint();




        SetPointActive();
        scan = (_s_current_domain.x + _s_current_domain.y) / 2;
        _point.UpdatePlot(_s);
    }

    public void UpdateRange(Vector3 direction)
    {


        //aktuellen Scan-Faktor merken
        //float s_temp = _s;



        //Verschiebung an Boxgröße und Skalierungsfaktor anpassen
        Vector3 normalizedDirection = Vector3.Scale(direction, _boxScale);
        normalizedDirection = Vector3.Scale(normalizedDirection, new Vector3(0.25f, 0.125f, 0.25f));



        //MoveSpeed
        normalizedDirection = Vector3.Scale(normalizedDirection, new Vector3(_moveSpeed, _moveSpeed, _moveSpeed));



        _range_min += normalizedDirection;
        _range_max += normalizedDirection;

        _axisLabeling.UpdateLabeling(_range_min, _range_max);

        if (_activePlot is PlotR3toR)
        {
            if (_plane != null)
            {
                Destroy(_plane.plot.curves);
            }

            _plane = CreatePlane();

            SetPlaneActive();
        }

        if (_activePlot is PlotR2toR2)
        {
            Destroy(_activePlot.plot.curves);
            _curve = CreateCurve();
            SetCurveActive();
        }

        if (_activePlot is PlotRtoR3)
        {
            Destroy(_activePlot.plot.curves);
            _point = CreatePoint();
            SetPointActive();
            
        }

        if (_activePlot != null)
        {
            if (!(_activePlot is PlotRtoR3)) Destroy(_activePlot.plot.curves);
            _activePlot.UpdatePlot(_s);
            Debug.Log($"changed s-value to {_s}");
        }


    }





    private PlotRtoR3 CreatePoint()
    {
        string[] variables = { "x" };
        string[] formulas = { _f1_point, _f2_point, _f3_point };
        Vector3[] range = { _range_min, _range_max };
        return new PlotRtoR3(variables, formulas, range, _domain_s_point);
    }
    private PlotR2toR2 CreateCurve()
    {
        string[] variables = { "x", "y"};
        string[] formulas = { _f1_curve, _f2_curve};
        Vector3[] range = { _range_min, _range_max };
        return new PlotR2toR2(variables, formulas, range, _domain_s_curve, _domain_x_curve);
    }
    private PlotR3toR CreatePlane()
    {
        string[] variables = { "x", "y", "z"};
        string[] formulas = { _f};
        Vector3[] range = { _range_min, _range_max };
        if(!_scaleDirectionEnabled)
        {
            return new PlotR3toR(variables, formulas, range, _domain_s_plane, _domain_x_plane, _domain_z_plane);
        }
        else
        {
            return new PlotR3toR(variables, formulas, _scaleDirection, range, _domain_s_plane, _domain_x_plane, _domain_z_plane);
        }
        
    }



}
