using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionInputManager : MonoBehaviour
{
    public VRInputField _planeInputField, _curveInputField1, _pointInputField1;
    private VRInputField _currentInputField;

    public Text _test;

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
            _test.text = CreateJaceExpr(_currentInputField.text);
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
        






        string temp = expr;
        int end = expr.Length;

        //Richtiges Ersetzen von s, x, z -> x, y, z

        temp = temp.Replace("sin", "$");
        temp = temp.Replace("cos", "?");

        temp = temp.Replace('x', 'y');
        temp = temp.Replace('s', 'x');

        temp = temp.Replace("$", "sin");
        temp = temp.Replace("?", "cos");




        //stille Mulitplikation ermöglichen // Sonderfall ')'
        char[] blocks = {'x', 'y', 'z', 'e', 'π',
                           's', 'c', 't', 'l', '√'};

        char[] operands = { '+', '-', '*', '/', '^', '.', '(' };

        for (int i = end; i > 1; i--)
        {
            char c = temp[i-1];
            if (blocks.Contains(c) && !operands.Contains(temp[i-2]) && temp[i - 2] != 'o') //dritter fall für cos sonderfall
            {
                temp = temp.Insert(i-1, "*");
            }
            if (c == ')' && i != end && !operands.Contains(temp[i]) && temp[i] != ')') //für fälle wie 'cos(3)x -> cos(3)*x ; vierter Fall für zwei schließende Klammern hintereinander
            {
                temp = temp.Insert(i, "*");
            }
        }

        //temp = temp.Replace(")*)", "))");



        if (temp[0] == '-') temp = temp.Insert(1, "1*"); //Jace-Bug mit '-' in erster Stelle wird abgefangen
        temp = temp.Replace("√", "sqrt");
        temp = temp.Replace("π", "pi");
        temp = temp.Replace("log", "loge");
        return temp;

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
