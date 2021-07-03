using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendRotation : MonoBehaviour
{

    private GameObject user;
    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
        gameObject.transform.LookAt(user.transform);
        gameObject.transform.localRotation *= Quaternion.Euler(0, 180, 0);
    }
}
