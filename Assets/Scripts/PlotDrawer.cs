using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlotDrawer
{
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
