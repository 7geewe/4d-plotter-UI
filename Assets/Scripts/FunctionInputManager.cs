using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionInputManager : MonoBehaviour
{

    public string currentExpression;
    public Text text;
    public InputField text3;
    public VRInputField text2;
    // Start is called before the first frame update
    void Start()
    {
        text2.ActivateInputField();
        //text2.caretBlinkRate = 1;

        Debug.Log(CreateJaceExpr("xxx"));
        Debug.Log(CreateJaceExpr("xxyy"));
        Debug.Log(CreateJaceExpr("xxyyyyx"));
        Debug.Log(CreateJaceExpr("xxyyyyyyyx"));
        Debug.Log(CreateJaceExpr("xxyyyyyyyyyyyyyx"));
    }

    // Update is called once per frame
    void Update()
    {
        //if (!text2) text2.ActivateInputField();
    }

    public void AddToExpr(string input)
    {
        //currentExpression += input;
        //text.text = currentExpression;
        string temp = text2.text;
        int pos = text2.caretPosition;
        Debug.Log(temp + " " + pos);
        temp = temp.Insert(pos, input);
        Debug.Log(input + " " + temp);
        text2.text = temp;
        text2.caretPosition += input.Length;

        //Test
        text.text = CreateJaceExpr(temp);
    }

    public void CaretToLeft()
    {
        if (text2.caretPosition > 0) text2.caretPosition -= 1;
    }

    public void CaretToRight()
    {
        if (text2.caretPosition < text2.text.Length) text2.caretPosition += 1;
    }

    public void DeleteOne()
    {
        if (text2.caretPosition != 0)
        {
            text2.caretPosition -= 1;
            text2.text = text2.text.Remove(text2.caretPosition, 1);

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


}
