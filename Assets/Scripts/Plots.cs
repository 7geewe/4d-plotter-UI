using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plot
{

    public GameObject curves { get; protected set; }

    protected PlotCalculation calculator;
    protected FunctionCreator creator;
    protected PlotDrawer drawer;

    protected Vector3 range_min, range_max; 

    public Plot(Vector3[] range)
    {
        //curves = new GameObject();

        calculator = new PlotCalculation();
        creator = new FunctionCreator();
        drawer = new PlotDrawer();

        range_min = range[0];
        range_max = range[1];

    }

    public void MoveAndScale()
    {
        Vector3 newSize = range_max - range_min;
        Vector3 newMiddle = (range_min+range_max)/2f;
        Vector3 middle = GameObject.Find("HologramBox").transform.position;
        Vector3 size = GameObject.Find("HologramBox").transform.localScale;

        curves.transform.position = middle - newMiddle;
        GameObject newPivotPoint = new GameObject();
        newPivotPoint.transform.position = middle;
        curves.transform.SetParent(newPivotPoint.transform);
        newPivotPoint.transform.localScale = new Vector3(size.x/newSize.x, size.y/newSize.y, size.z/newSize.z);
        
        //neuer Mittelpunkt zum drehen/skalieren, nicht zerstören wenn gebraucht
        curves.transform.parent = null;
        GameObject.Destroy(newPivotPoint);
        
    }

}

public class PlotR2toR : Plot
{

    private Func<float, float, float[]> f;
    public PlotR2toR(string[] input, string[] formula, Vector3[] range, Vector2 domain_1, Vector2 domain_2) : base(range)
    {
        f = creator.R2toRm(input[0], input[1], formula);
        
        //curves = drawer.DrawCurves(calculator.CalculatePlotR2toR(f));
        curves = drawer.DrawCurves(calculator.ShapeCurves(calculator.CalculatePlotR2toR(f, domain_1, domain_2), range_min, range_max));
        
    }

    public PlotR2toR(Func<float, float, float[]> f, Vector3[] range, Vector2 domain_1, Vector2 domain_2) : base(range)
    {
        //curves = drawer.DrawCurves(calculator.CalculatePlotR2toR(f));
        curves = drawer.DrawCurves(calculator.ShapeCurves(calculator.CalculatePlotR2toR(f, domain_1, domain_2), range_min, range_max));

    }
}

public class PlotRtoRn : Plot
{
    private Func<float, float[]> f;

    public PlotRtoRn(string[] input, string[] formulas, Vector3[] range, Vector2 domain) : base(range)
    {
        f = creator.R1toRm(input[0], formulas);
        //curves = drawer.DrawCurve(calculator.CalculatePlotR1toRn(f));
        curves = drawer.DrawCurves(calculator.ShapeCurve(calculator.CalculatePlotR1toRn(f, domain.x, domain.y), range_min, range_max));
        
    }

    public PlotRtoRn(Func<float, float[]> f, Vector3[] range, Vector2 domain) : base(range)
    {
        //curves = drawer.DrawCurve(calculator.CalculatePlotR1toRn(f));
        curves = drawer.DrawCurves(calculator.ShapeCurve(calculator.CalculatePlotR1toRn(f, domain.x, domain.y), range_min, range_max));
        //List<List<Vector3>> l = new List<List<Vector3>>();
        //l.Add(calculator.CalculatePlotR1toRn(f, dom_min.x, dom_max.x));
        //curves = drawer.DrawCurves(l);
    }
}


public class ScalingPlot
{
    public Plot plot;
    protected FunctionCreator creator;
    protected Vector3[] _range;
    protected Vector2 _domain_s;

    public ScalingPlot(Vector3[] range, Vector2 domain_s)
    {
        creator = new FunctionCreator();
        _range = range;
        _domain_s = domain_s;
    }

}



public class PlotR2toR2 : ScalingPlot
{

    private Func<float, float, float[]> f;
    private Vector2 _dom_y;

    public PlotR2toR2(string[] input, string[] formulas, Vector3[] range, Vector2 domain_s, Vector2 dom_y) : base(range, domain_s)
    {
        f = creator.R2toRm(input[0], input[1], formulas);
        _dom_y = dom_y;
        UpdatePlot((_domain_s.x+_domain_s.y)/2f);
    }

    public void UpdatePlot(float s)
    {
        plot = new PlotRtoRn(y => f(s, y), _range, _dom_y);
        plot.MoveAndScale();
    }

}




public class PlotR3toR : ScalingPlot
{
    private Func<float, float, float, float[]> f, f_scaled;
    private bool scaled = false;
    private Vector2 _dom_y, _dom_z;

    public PlotR3toR(string[] input, string[] formula, Vector3[] range, Vector2 domain_s, Vector2 domain_y, Vector2 domain_z) : base(range, domain_s)
    {
        f = creator.R3toRm(input[0], input[1], input[2], formula);
        _dom_y = domain_y;
        _dom_z = domain_z;
        UpdatePlot((_domain_s.x + _domain_s.y) / 2f);
    }

    public PlotR3toR(string[] input, string[] formula, Vector3 scan_direction, Vector3[] range, Vector2 domain_s, Vector2 domain_y, Vector2 domain_z) : base(range, domain_s)
    {
        f = creator.R3toRm(input[0], input[1], input[2], formula);
        CreateScaleFunction(scan_direction);
        scaled = true;
        _dom_y = domain_y;
        _dom_z = domain_z;
        UpdatePlot((_domain_s.x + _domain_s.y) / 2f);
    }
    public void UpdatePlot(float s)
    {
        if (!scaled) plot = new PlotR2toR((y, z) => f(s, y, z), _range, _dom_y, _dom_z);
        else plot = new PlotR2toR((y, z) => f_scaled(s, y, z), _range, _dom_y, _dom_z);

        plot.MoveAndScale();
    }

    public void CreateScaleFunction(Vector3 scan_direction)
    {
        f_scaled = creator.R3toRnScalars(f, scan_direction);
    }

    public void Switch_To_Scaled_Mode(bool scale)
    {
        scaled = scale;
    }
}


public class PlotRtoR3 : ScalingPlot
{
    private GameObject sphere;
    private Func<float, float[]> f;

    public PlotRtoR3(string[] input, string[] formulas, Vector3[] range, Vector2 domain) : base(range, domain)
    {
        f = creator.R1toRm(input[0], formulas);
        plot = new PlotRtoRn(input, formulas, _range, _domain_s);
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(1f, 1f, 1f);

        plot.MoveAndScale();
        UpdatePlot((_domain_s.x + _domain_s.y) / 2f);
    }

    public void UpdatePlot(float s)
    {
        float[] r = f(s);
        sphere.transform.SetParent(plot.curves.transform);
        sphere.transform.position = plot.curves.transform.position;
        sphere.transform.localPosition = new Vector3(r[0], r[2], r[1]);

    }

}
