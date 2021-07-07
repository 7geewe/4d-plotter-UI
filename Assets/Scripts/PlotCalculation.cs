using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
public class PlotCalculation
{
    private int plotAccuracy;
    private List<List<Vector3>> KurvenTest = new List<List<Vector3>>();
    

    //plot für R->R mit Konstante; relevant für Triviale funktionen sowie zum Plotten von Ebenen
    public List<Vector3> CalculatePlotR1toRn(Func<float, float[]> f, float c = 0f, int cpos = 1) 
    {
        return CalculatePlotR1toRn(f, -5f, 5f, c, cpos);
    }


    public List<Vector3> CalculatePlotR1toRn(Func<float, float[]> f, float dom_min, float dom_max, float c = 0f, int cpos = 1) //f(x, 0) = x^2 + 0
    {
        
        Vector3 vertice = new Vector3();
        List<Vector3> curve = new List<Vector3>();

        plotAccuracy = GameManager.instance._plotAccuracy;
        for (int i = 0; i <= plotAccuracy; i++)
        {
            float input = (float)i / plotAccuracy * (dom_max - dom_min) + dom_min; //Normalisierter Wert anhand der Intervallgrenzen

            float[] output = f(input);
            List<float> point = new List<float>();

            point.Add(input);
            point.AddRange(output);
            

            //Sonderfall: R -> R3
            if (point.Count == 4) 
            {
                point.RemoveAt(0);
                //CreateScalar
            };
            
            if(point.Count ==2) { point.Insert(cpos, c); };
            vertice.Set(point[0], point[2], point[1]);
            
            
            //Debug.Log(vertice);
            
            curve.Add(vertice);
        };

        return curve;

    }


    public List<List<Vector3>> CalculatePlotR2toR(Func<float, float, float[]> f)
    {
        return CalculatePlotR2toR(f, new Vector3(-5f, -5f, 0f), new Vector3(5f, 5f, 0f));
    }

    public List<List<Vector3>> CalculatePlotR2toR(Func<float, float, float[]> f, Vector2 domain_1, Vector2 domain_2) //f(x, 0) = x^2 + 0
    {

        List<List<Vector3>> curves = new List<List<Vector3>>();

        plotAccuracy = GameManager.instance._plotAccuracy;
        for (int i = 0; i <= plotAccuracy; i++)
        {
            float input1 = (float)i / plotAccuracy * (domain_1.y - domain_1.x) + domain_1.x; //Normalisierter Wert anhand der Intervallgrenzen
            float input2 = (float)i / plotAccuracy * (domain_2.y - domain_2.x) + domain_2.x; //Normalisierter Wert anhand der Intervallgrenzen


            curves.Add(CalculatePlotR1toRn(x => f(input1, x), domain_2.x, domain_2.y, input1, 1));
            curves.Add(CalculatePlotR1toRn(x => f(x, input2), domain_1.x, domain_1.y, input2, 0));

        };

        return curves;

    }

    public List<List<Vector3>> ShapeCurves(List<List<Vector3>> curves)
    {
        Vector3 min = new Vector3(0, -5, -5), max = new Vector3(5, 5, 5);
        return ShapeCurves(curves, min, max);
    }

    public List<List<Vector3>> ShapeCurves(List<List<Vector3>> curves, Vector3 min, Vector3 max)
    {
        List<List<Vector3>> shapedCurves = new List<List<Vector3>>();

        foreach (List<Vector3> curve in curves)
        {     
            shapedCurves.AddRange(ShapeCurve(curve, min, max));
        }

        return shapedCurves;
    }



    public List<List<Vector3>> ShapeCurve(List<Vector3> curve)
    {
        Vector3 min = new Vector3(-5, -5, -5), max = new Vector3(5, 5, 5);
        return ShapeCurve(curve, min, max);
    }
    public List<List<Vector3>> ShapeCurve(List<Vector3> curve, Vector3 min, Vector3 max)
    {
        List<List<Vector3>> segments = new List<List<Vector3>>();
        List<Vector3> segment = new List<Vector3>();

        Vector3 v_current, v_next, bound_v_current, bound_v_next = new Vector3();
        Vector3 v_zeros = new Vector3(0, 0, 0);

        for(int i = 0; i < curve.Count - 1; i++)
        {
            v_current = curve[i];
            v_next = curve[i + 1];

            bound_v_current = BoundVector(v_current, min, max);
            bound_v_next = BoundVector(v_next, min, max);
            
            if (bound_v_current == v_zeros)
            {
                segment.Add(v_current);
                if (bound_v_next != v_zeros)
                {

                    //Add a intersectionpoint, but only when the value is not NaN
                    if (!float.IsNaN(bound_v_next.x)) segment.Add(IntersectionPoint(v_current, v_next, bound_v_next, min, max));
                    segments.Add(new List<Vector3>(segment));
                    segment.Clear();
                }
            }
        }
        Vector3 v_last = curve[curve.Count - 1];
        if (BoundVector(v_last, min, max) == v_zeros) segment.Add(v_last);
        segments.Add(new List<Vector3>(segment));


        //Split Segments with Connected Function Jumps

        /**List<List<Vector3>> splittedSegments = new List<List<Vector3>>();
        foreach (List<Vector3> s in segments)
        {


            int lastJump = 0;
            for (int i = 0; i < s.Count-1; i++)
            {
                Vector3 jumpTest = s[i] - s[i + 1];

                if(Math.Abs(jumpTest.x) > Math.Abs(max.x-min.x)*0.5f ^ Math.Abs(jumpTest.y) > Math.Abs(max.y - min.y) * 0.5f ^ Math.Abs(jumpTest.z) > Math.Abs(max.z - min.z) * 0.5f)
                {
                    splittedSegments.Add(s.GetRange(lastJump, i - lastJump));
                    lastJump = i;
                }

            }
            splittedSegments.Add(s.GetRange(lastJump, s.Count - lastJump));

            
        }**/




        return segments;
    }

    private Vector3 BoundVector(Vector3 v, Vector3 min, Vector3 max)
    {
        Vector3 result, minTest, maxTest = new Vector3();
        result = new Vector3(0, 0, 0);
        minTest = v - min;
        maxTest = v - max;

        if (float.IsNaN(v.x) ^ float.IsNaN(v.y) ^ float.IsNaN(v.z)) return new Vector3(float.NaN, 0f, 0f);


        if (minTest.x < 0f) result.x = -1;
        else if (maxTest.x > 0f) result.x = 1;
        

        if (minTest.y < 0f) result.y = -1;
        else if (maxTest.y > 0f) result.y = 1;

        if (minTest.z < 0f) result.z = -1;
        else if (maxTest.z > 0f) result.z = 1;

        return result;

    }

    private Vector3 IntersectionPoint(Vector3 in_bound, Vector3 out_of_bound, Vector3 bound_vector, Vector3 min, Vector3 max)
    {
        Vector3 result = out_of_bound;

        if (bound_vector.x == 1) result.x = max.x;
        else if (bound_vector.x == -1) result.x = min.x;

        if (bound_vector.y == 1) result.y = max.y;
        else if (bound_vector.y == -1) result.y = min.y;

        if (bound_vector.z == 1) result.z = max.z;
        else if (bound_vector.z == -1) result.z = min.z;

        return result;


    }


}

/**
 * TODOS:
 * Skalierung von Wertebereich
 *      -> automatisch anhand Intervall? -> find highest point
 *      -> vorerst via Nutzereingabe bzw festgelegt?
 *      -> angabe von Definitionsbereich bei jeder Funktion? z.B. logarithmus
 *      -> Error Handling für falsche Definitionsbereiche (z.B. log mit Zahlen unter 0)
 *      
 * Aufschneiden von Funktionskurve in merhere Kurven, wenn Limit erreicht ist
 *      -> je nach dem, wo das passiert, kann Rückgabetyp von PlotFunction zu Curve anstatt Curves werden
 *      
 *      
 * Fehlerhandling von falschen Punkten
 *      2 Möglichkeiten:
 *          Außerhalb des Wertebereichs
 *          keine Zahl (NaN, inf, Div0 etc.)
 *    --> diese abfangen und Kurven splitten (müssen per Def. zusammenhängende Punkte sein)
 *    --->ggf. neue Punkte an DefbereichGrenze setzen
 *      
 * 
 * 
 * 
 * R->R   f(x) = x^2 -> Kurve
 * R->R2   f(x) = (x^2, x^3) Kurve 
 * R->R3    f(x) = (x^2, x^3, x^4) Kurve
 * R2->R    f(x,y) = x*y -> Ebene
 * 
 * 
 * 
 * 
 * 
 * */
