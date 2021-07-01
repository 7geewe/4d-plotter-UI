using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jace;
using System;

public class FunctionCreator : MonoBehaviour
{

    private Dictionary<string, double> variables = new Dictionary<string, double>();
    private CalculationEngine engine = new CalculationEngine();


    /// <summary>
    /// Erzeugt eine Funktion in beliebiger Dimension von R1 -> Rm
    /// </summary>
    /// <param name="parameter"></param> nichtkonstantetr Parameter als string
    /// <param name="formulas"></param> Array der Funktionsausdrücke in Sting-Form f(x, y) = (x * y, x / y) -> {"x * y", "x / y"}
    /// <returns> Funktion f(parameter) = formulas in float-Array Form.</returns>
    public Func<float, float[]> R1toRm(string parameter, string[] formulas)
    {
        Func<float, float[]> function = (x) =>
        {
            Dictionary<string, double> tempVars = variables;
            List<float> result = new List<float>();

            variables[parameter] = x;

            foreach (string f in formulas)
            {
                result.Add((float)engine.Calculate(f, variables));
            }
            variables = tempVars;
            return result.ToArray();
        };
        return function;
    }

    public Func<float, float, float, float[]> R3toRm(string p1, string p2, string p3, string[] formulas)
    {
        Func<float, float, float, float[]> function = (x, y, z) =>
        {
            Dictionary<string, double> tempVars = variables;
            List<float> result = new List<float>();

            variables[p1] = x;
            variables[p2] = y;
            variables[p3] = z;

            foreach (string f in formulas)
            {
                result.Add((float)engine.Calculate(f, variables));
            }
            variables = tempVars;
            return result.ToArray();
        };
        return function;
    }

    public Func<float, float, float[]> R2toRm(string p1, string p2, string[] formulas)
    {
        Func<float, float, float[]> function = (x, y) =>
        {
            Dictionary<string, double> tempVars = variables;
            List<float> result = new List<float>();

            variables[p1] = x;
            variables[p2] = y;

            foreach (string f in formulas)
            {
                result.Add((float)engine.Calculate(f, variables));
            }
            variables = tempVars;
            return result.ToArray();
        };
        return function;
    }


    /// <summary>
    /// äquivalente Funktion in Richtung eines Vektors rotiert, bzw Ebene durchgescannt entlang Vektor
    /// </summary>
    /// <param name="f"></param> funktion, die skaliert werden soll
    /// <param name="vec"></param> Richtungsvektor
    /// <returns>neue, gleich-dimensionale Funktion</returns>
    public Func<float, float, float[]> R2toRnScalars(Func<float, float, float[]> f, Vector2 vec)
    {
        Vector2 v = vec;
        v.Normalize();

        //zu v senkrechter Vektor
        Vector2 w = new Vector2(-v.y, v.x);

        Func<float, float, float[]> function = (s, t) =>
        {
            Vector2 newInput = s * v + t * w;
            return f(newInput.x, newInput.y);
        };
        return function;

    }

    /// <summary>
    /// äquivalente Funktion in Richtung eines Vektors rotiert, bzw Multiebene durchgescannt entlang Vektor
    /// </summary>
    /// <param name="f"></param> Funktion, die skaliert werden soll
    /// <param name="vec"></param> Richtungsvektor
    /// <returns>neue, gleichdimensionale Funktion</returns>
    public Func<float, float, float, float[]> R3toRnScalars(Func<float, float, float, float[]> f, Vector3 vec)
    {
        Vector3 v_normal, v_tangent, v_binormal;
        v_normal = vec;
        v_tangent = new Vector3();
        v_binormal = new Vector3();
        Vector3.OrthoNormalize(ref v_normal, ref v_tangent, ref v_binormal);
        //Debug.Log("" + v_normal + "  " + v_tangent + "  " + v_binormal);

        Func<float, float, float, float[]> function = (s, t, u) =>
        {
            Vector3 newInput = s * v_normal + t * v_tangent + u * v_binormal;
            return f(newInput.x, newInput.y, newInput.z);
        };
        return function;

    }



    // Start is called before the first frame update
    public void Start()
    {
        variables.Add("x", 0);
        variables.Add("y", 0);
        variables.Add("z", 0);
        //variables.Add("t", 0);


        Debug.Log(engine.Calculate("pi"));
        Debug.Log(engine.Calculate("e"));
        //Debug.Log(engine.Calculate(""));
        //Debug.Log(engine.Calculate("pi"));
    }
    /**public void Update() //-x^2 wird leider nicht korrekt berechnet, nur im ersten Term
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Dictionary<string, double> variablesT = new Dictionary<string, double>();
            variablesT.Add("var1", 1);
            variablesT.Add("var2", 1);

            //CalculationEngine engine = new CalculationEngine();
            double result = engine.Calculate("- var1^2 - var2^2", variablesT);
            Debug.Log(result);
        }
    }**/
}
