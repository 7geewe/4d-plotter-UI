using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlotDrawer : MonoBehaviour
{

    //List<List<Vector3>> curves = new List<List<Vector3>>();
    PlotCalculation calculator;
    FunctionCreator creator;
    float scale;
    List<GameObject> ActivePlots;
    PlotRtoR3 plot;
    PlotR3toR plotR3;
    PlotR2toR2 plotR2;
    PlotRtoRn plot_RtoR2;
    PlotR2toR plot_R2toR;

    // Start is called before the first frame update
    void Start()
    {
        calculator = FindObjectOfType<PlotCalculation>();
        creator = FindObjectOfType<FunctionCreator>();
        scale = 0f;
        ActivePlots = new List<GameObject>();
        string[] functions = { "x^2", "x^3", "cos(x)" };
        string[] function = { "x*y + y^2 / cos(z)"};
        string[] input3 = { "x", "y", "z" };
        string[] input = { "x" };
        string[] input2 = { "x", "y" };
        string[] function2 = { "x^2 + y^2", "sin(x) / cos(y)" };
        string[] function1 = { "x^2", "x^2" };
        float[] direction = { 1f, 2f, 3f };
        //plot = new PlotRtoR3(input, functions);


        //plotR3 = new PlotR3toR(input3, function, direction);


        //plotR2 = new PlotR2toR2(input2, function2);

        //plot_RtoR2 = new PlotRtoR2(input, function1);

        //plot_R2toR = new PlotR2toR(input2, function);

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            string[] input = { "x", "y", "z"};
            string[] function = { "x^2 - y^2" };
            string[] function3 = { "x^2 + cos(y) - z" };
            string[] functions = { "x^2", "x * 3", "cos(x)" };
            string[] functions2 = { "x^2 / y^2", "-(x^2) - y^2" };

            Func<float, float, float, float[]> f3 = creator.R3toRm(input[0], input[1], input[2], function3);
            Func<float, float, float, float[]> f3_scaled = creator.R3toRnScalars(f3, new Vector3(1f, 2f, -3f));
            //scale += 0.1f;


            Func<float, float, float[]> f2to2 = creator.R2toRm(input[0], input[1], functions2);
            Func<float, float, float[]> f2tof = (x, y) =>
            {
                float[] temp;
                float[] temp2 = { 0 };
                temp = f2to2(x, y);
                temp2[0] = temp[0];
                return temp2;
            };

            Func<float, float, float[]> f2tofTwo = (x, y) =>
            {
                float[] temp2 = { 0 };
                temp2[0] = f2to2(x, y)[1];
                return temp2;
            };

            //R2->R2, beide Ergebnisebenen in einem Graph
            //List<List<Vector3>> curvesTest = calculator.CalculatePlotR2toR(f2tof);



            //R3->R scaled, skaliert nach S
            //List<List<Vector3>> curvesTest = calculator.CalculatePlotR2toR((y, z) => f3_scaled(scale, y, z));


            //R3->R skaliert nach S
            //List<List<Vector3>> curvesTest = calculator.CalculatePlotR2toR((x, y) => f3(x, y, scale));


            //normale Funktion
            //List<List<Vector3>> curvesTest = calculator.CalculatePlotR2toR(creator.R2toRm(input[0], input[1], function));


            //skalierte Funktion nach Geschkes Algorithmus           
            //List<List<Vector3>> curvesTest = calculator.CalculatePlotR2toR(creator.R2toRnScalars(creator.R2toRm(input[0], input[1], function), new Vector2(0.2f, 0.6f)));

            //ActivePlots.Add(DrawCurves(calculator.CalculatePlotR2toR(f2tof)));
            //ActivePlots.Add(DrawCurves(calculator.CalculatePlotR2toR(f2tofTwo)));

            
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            scale -= 0.1f;
            //plot.UpdatePlot(scale);
            //Destroy(plotR3.plot.curves);
            //plotR3.UpdatePlot(scale);
            //plotR2.UpdatePlot(scale);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            scale += 0.1f;
            //plot.UpdatePlot(scale);
            //Destroy(plotR3.plot.curves);
            //plotR3.UpdatePlot(scale);
            //plotR2.UpdatePlot(scale);
        }
    }


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
        //lr.startColor = Color.cyan;
        Material m = lr.material;
        m.color = Color.cyan;
        return curve_obj;
    }

}

/**
 * TODOS:
 *      Farbauswahl ermöglichen
 *      Material erzeugen und standardmäßig zuweisen
 *          -Prefab für Curve-Obejct? Name von Material suchen und zuweisen? Textur??
 *      Koordinatensystem bauen
 *          dynamische Achsenbeschriftungen
 *          
 *      
 *      Creator, Calculator, Drawer als normale Klassen -> keine Monobehaviors
 *    
 *          
 *      neue Funktion: beim R2->R2 skalieren Kurven stehenlassen an/aus ODER von vornherein gesamten Plot zeichnen mit markiertem aktuellen Kurve
 *          R2->R2 Doppelebene ermöglichen
 *      
 *      Anzeigen aktueller Parameter
 *      
 *      Klasse für Skalieren entlang Gerade -> einbauen als eigenen Konstruktor in R3->R -> Done
 *      Klassen für 3, 2 und 1-Dimensionale Plots -> DONE
 *      Fallunterscheidung Nutzereingabe
 *          möglichst direkt halten -> lieber eine lange unterscheidung, die eibnmalig stattfindet, als dauerhafte Unterscheidung
 *          
 *      Klasse für Skalieren
 *      
 *      
 *      STILL OFFEN:
            Überprüfung von Punkt-Koordínaten -> außerhalb des Bereichs?? Keine Zahl?? Div durch 0???
                -außerhalb des Bereichs abschneiden DONE
 *      
 * 
 * 
 * */
