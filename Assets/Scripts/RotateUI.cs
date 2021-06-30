using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUI : MonoBehaviour
{

    public GameObject user;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = user.transform.position;
        pos.y = 0f;
        Quaternion direction = Quaternion.LookRotation(pos);

        gameObject.transform.rotation = direction;
        //gameObject.transform.position.
    }
}
