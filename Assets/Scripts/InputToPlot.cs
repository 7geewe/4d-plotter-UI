using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputToPlot : MonoBehaviour
{
    public Text text;
    private PlotRtoR3 plot_RtoR3;
    private PlotR2toR2 plot_R2toR2;
    private PlotR3toR plot_R3toR;
    public List<string> R1toR3;
    public List<string> R2toR2;
    public List<string> R3toR1;
    public List<string> variables;
    public float scale;
    public List<float> direction;

    public Vector3 range_min, range_max;


    public Vector3[] range;

    public Vector2 domain_x;
    public Vector2 domain_y;
    public Vector2 domain_z;


    // Start is called before the first frame update
    void Start()
    {
        direction.Add(0f);
        direction.Add(0f);
        direction.Add(0f);
        R1toR3 = new List<string>();
        R1toR3.Add(""); R1toR3.Add(""); R1toR3.Add("");
        R2toR2.Add(""); R2toR2.Add("");
        R3toR1.Add("");
        variables = new List<string>();
        variables.Add("x");
        variables.Add("y");
        variables.Add("z");

        range_min = new Vector3(-5f, -5f, -5f);
        range_max = new Vector3(5f, 5f, 5f);

        range = new Vector3[] { range_min, range_max };

        domain_x = new Vector2(-2f, 4f);
        domain_y = new Vector2(-3f, 5f);
        domain_z = new Vector2(-5f, 5f);

        scale = (domain_x.x + domain_x.y) / 2f;


    }
    
    public void TestPlot()
    {
        
        string[] input = { variables[0] };
        string[] inputR2 = { variables[0], variables[1] };
        string[] inputR3 = { variables[0], variables[1], variables[2] };

        //R3toR1[0] = text.text;
        //plot_R3toR = new PlotR3toR(inputR3, R3toR1.ToArray(), range, domain_x, domain_y, domain_z);


        R2toR2[0] = "y^2*x";
        R2toR2[1] = "sin(x)*y";

        Debug.Log(R2toR2.ToArray()[0]+ "  " + R2toR2.ToArray()[1] );
        plot_R2toR2 = new PlotR2toR2(inputR2, R2toR2.ToArray(), range, domain_x, domain_y);

    }


    /**
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            string[] input = { variables[0] };
            string[] inputR2 = { variables[0], variables[1] };
            string[] inputR3 = { variables[0], variables[1], variables[2] };
            if (plot_RtoR3 != null) Destroy(plot_RtoR3.plot.curves);
            if (R1toR3[2] != "") plot_RtoR3 = new PlotRtoR3(input, R1toR3.ToArray(), range, domain_x);


            if (plot_R2toR2 != null) Destroy(plot_R2toR2.plot.curves);
            if (R2toR2[0] != "") plot_R2toR2 = new PlotR2toR2(inputR2, R2toR2.ToArray(), range, domain_x, domain_y);

            if (plot_R3toR != null) Destroy(plot_R3toR.plot.curves);
            if (R3toR1[0] != "") plot_R3toR = new PlotR3toR(inputR3, R3toR1.ToArray(), range, domain_x, domain_y, domain_z);
            //if (R3toR1[0] != "") plot_R3toR = new PlotR3toR(inputR3, R3toR1.ToArray(), direction.ToArray(), range, domain_x, domain_y, domain_z);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            scale -= 0.1f;
            if (plot_RtoR3 != null)
            {
                //Destroy(plot.plot.curves);
                plot_RtoR3.UpdatePlot(scale);
            }
            if (plot_R3toR != null)
            {
                Destroy(plot_R3toR.plot.curves);
                plot_R3toR.UpdatePlot(scale);
            }
            if (plot_R2toR2 != null)
            {
                Destroy(plot_R2toR2.plot.curves);
                plot_R2toR2.UpdatePlot(scale);
            }
            //plot.UpdatePlot(scale);
            //Destroy(plotR3.plot.curves);
            //plotR3.UpdatePlot(scale);
            //plotR2.UpdatePlot(scale);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            scale += 0.1f;
            if (plot_RtoR3 != null)
            {
                //Destroy(plot.plot.curves);
                plot_RtoR3.UpdatePlot(scale);
            }
            if (plot_R3toR != null)
            {
                Destroy(plot_R3toR.plot.curves);
                plot_R3toR.UpdatePlot(scale);
            }
            if (plot_R2toR2 != null)
            {
                Destroy(plot_R2toR2.plot.curves);
                plot_R2toR2.UpdatePlot(scale);
            }
            //plot.UpdatePlot(scale);
            //Destroy(plotR3.plot.curves);
            //plotR3.UpdatePlot(scale);
            //plotR2.UpdatePlot(scale);
        }
    }**/
}
