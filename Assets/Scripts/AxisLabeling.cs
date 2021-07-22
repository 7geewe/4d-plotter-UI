using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxisLabeling : MonoBehaviour
{

    public GameObject axisDot;
    private float box_radius = 0.5f;
    private Vector3 x_axis_position, x_axis_rotation, y_axis_position, y_axis_rotation, z_axis_position, z_axis_rotation;
    private GameObject _box;
    private List<GameObject> _labeling = new List<GameObject>();
    private Camera _mainCamera;



    private void Start()
    {
        _box = GameObject.Find("HologramBox");
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();



        x_axis_position = new Vector3(0f, box_radius, box_radius);
        x_axis_rotation = new Vector3(45f, 0f, 90f);


        y_axis_position = new Vector3(box_radius, 0f, box_radius);
        y_axis_rotation = new Vector3(0f, -45f, 0f);

        z_axis_position = new Vector3(box_radius, box_radius, 0f);
        z_axis_rotation = new Vector3(0f, 0f, 45f);

        //CreateFullLegend(new Vector3(-2f, -4f, -2f), new Vector3(2f, 4f, 2f));
    }

    public void UpdateLabeling(Vector3 range_min, Vector3 range_max)
    {
        foreach (GameObject g in _labeling)
        {
            Destroy(g);
        }

        CreateFullLegend(range_min, range_max);
    }






    private void CreateDot(float t, Vector3 pos, Vector3 rot)
    {
        GameObject dot = Instantiate(axisDot);
        dot.GetComponentInChildren<Canvas>().worldCamera = _mainCamera;
        dot.GetComponentInChildren<Text>().text = t.ToString();
        dot.transform.SetParent(_box.transform);
        dot.transform.localPosition = pos;
        dot.transform.localRotation *= Quaternion.Euler(rot);
        _labeling.Add(dot);
    }


    private float CalcNewPoint(Vector2 range, float point)
    {
        return (point - range.x) / (range.y - range.x) * 2 * box_radius - box_radius; //Berechnet n-Koordinate für Beschriftungspunkt auf n-Achse, abhängig von der Achsenskalierung und Boxgröße
    }




    private void CreateFullLegend(Vector3 range_min, Vector3 range_max)
    {
        Vector2 x_range = new Vector2(range_min.x, range_max.x);
        Vector2 y_range = new Vector2(range_min.y, range_max.y);
        Vector2 z_range = new Vector2(range_min.z, range_max.z);




        foreach (float i in LegendPoints(x_range, 0.5f))
        {
            CreateDot_x(i, x_range);
        }

        foreach (float i in LegendPoints(y_range, 0.5f))
        {
            CreateDot_y(i, y_range);
        }

        foreach (float i in LegendPoints(z_range, 0.5f))
        {
            CreateDot_z(i, z_range);
        }



    }

    private List<float> LegendPoints(Vector2 range, float space)
    {
        List<float> res = new List<float>();

        float start = Mathf.Ceil(range.x * 2f) / 2f;
        float end = Mathf.Floor(range.y * 2f) / 2f;


        for (float i = start; i <= end; i += space)
        {
            res.Add(i);
        }

        return res;

    }



    private void CreateDot_x(float point, Vector2 range)
    {
        Vector3 x_pos = x_axis_position;
        x_pos.x = CalcNewPoint(range, point);
        CreateDot(point, x_pos, x_axis_rotation);
    }
    private void CreateDot_y(float point, Vector2 range)
    {
        Vector3 y_pos = y_axis_position;
        y_pos.y = CalcNewPoint(range, point);
        CreateDot(point, y_pos, y_axis_rotation);
    }
    private void CreateDot_z(float point, Vector2 range)
    {
        Vector3 z_pos = z_axis_position;
        z_pos.z = CalcNewPoint(range, point);
        CreateDot(point, z_pos, z_axis_rotation);
    }


}
