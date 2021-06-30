using System.Collections;
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
        text.text = ReplaceMathSymbols(temp);
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

    private string ReplaceMathSymbols(string expr)
    {
        return expr.Replace("√", "sqrt");

    }


}
