using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionInputManager : MonoBehaviour
{
    public VRInputField _planeInputField, _curveInputField1, _pointInputField1;
    private VRInputField _currentInputField;

    // Start is called before the first frame update


    public GameObject _plane_p, _curve_p, _point_p;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToExpr(string input)
    {
        if (_currentInputField != null)
        {
            int pos = _currentInputField.caretPosition;
            _currentInputField.text = _currentInputField.text.Insert(pos, input);
            _currentInputField.caretPosition += input.Length;
        }

    }

    public void CaretToLeft()
    {
        if (_currentInputField != null && _currentInputField.caretPosition > 0) _currentInputField.caretPosition -= 1;
    }

    public void CaretToRight()
    {
        if (_currentInputField != null && _currentInputField.caretPosition < _currentInputField.text.Length) _currentInputField.caretPosition += 1;
    }

    public void DeleteOne()
    {
        if (_currentInputField != null && _currentInputField.caretPosition != 0)
        {
            _currentInputField.caretPosition -= 1;
            _currentInputField.text = _currentInputField.text.Remove(_currentInputField.caretPosition, 1);

        }
    }

    private string CreateJaceExpr(string expr)
    {
        //stille Mulitplikation ermöglichen // Sonderfall ')'
        char[] blocks = {'x', 'y', 'z', 'e', 'π',
                           's', 'c', 't', 'l', '√'};

        char[] operands = { '+', '-', '*', '/', '^', '.', '('};

        int end = expr.Length;
        for (int i = end; i > 1; i--)
        {
            char c = expr[i-1];
            if (blocks.Contains(c) && !operands.Contains(expr[i-2]) && expr[i - 2] != 'o') //dritter fall für cos sonderfall
            {
                expr = expr.Insert(i-1, "*");
            }
            if (c == ')' && i != end && !operands.Contains(expr[i])) //für fälle wie 'cos(3)x -> cos(3)*x
            {
                expr = expr.Insert(i, "*");
            }
        }



        if (expr[0] == '-') expr = expr.Insert(1, "1*"); //Jace-Bug mit '-' in erster Stelle wird abgefangen
        expr = expr.Replace("√", "sqrt");
        expr = expr.Replace("π", "pi");
        expr = expr.Replace("log", "loge");
        return expr;

    }

    public void ClearExpr()
    {
        if (_currentInputField != null) _currentInputField.text = "";

    }

    public void SetActiveField(VRInputField t)
    {
        if (_currentInputField != null) _currentInputField.DeactivateInputField();
        _currentInputField = t;
        Debug.Log("ChangeActiveInputFieldTo:   " + t.ToString()) ;
        _currentInputField.ActivateInputField();
        
        StartCoroutine(MoveTextEnd_NextFrame());
        
        
    }

    IEnumerator MoveTextEnd_NextFrame()
    {
        yield return 0; // Skip the first frame in which this is called.
        _currentInputField.MoveTextEnd(false); // Do this during the next frame.
    }



    public void PlanePressed()
    {
        _curve_p.SetActive(false);
        _point_p.SetActive(false);
        _plane_p.SetActive(true);
    }
    public void CurvePressed()
    {
        _point_p.SetActive(false);
        _plane_p.SetActive(false);
        _curve_p.SetActive(true);
    }


    public void PointPressed()
    {
        _curve_p.SetActive(false);
        _plane_p.SetActive(false);
        _point_p.SetActive(true);

    }

    public void Test()
    {
        Debug.Log("Knopf ist Heile");
    }



}
