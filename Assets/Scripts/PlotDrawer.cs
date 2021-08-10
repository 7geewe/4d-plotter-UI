using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlotDrawer
{


    private Material plotMat = (Material)Resources.Load("PlotMaterial", typeof(Material));

    public GameObject DrawCurves(List<List<Vector3>> curves)
    {
        GameObject plot = new GameObject();
        foreach (List<Vector3> curve in curves)
        {
            GameObject curve_obj = DrawCurve(curve);
            curve_obj.transform.SetParent(plot.transform);

        }
        return plot;
    }

    public GameObject DrawCurve(List<Vector3> curve)
    {
        GameObject curve_obj = new GameObject();
        LineRenderer lr = curve_obj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lr.positionCount = curve.Count;
        lr.SetPositions(curve.ToArray());
        lr.startWidth = 0.01f;
        lr.useWorldSpace = false;
        lr.generateLightingData = true;
        lr.startColor = Color.cyan;
        lr.material = plotMat;
        return curve_obj;
    }

}

