using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScanInputListener : MonoBehaviour
{

    public InputActionReference scanReference = null;


    private int _timesInvoked = 0, _timesChanged = 0, framesPerScan;

    private Vector3 _controllerPos, _moveDirection;



    private void Awake()
    {
        scanReference.action.performed += ScanPlot;




    }

    private void OnDestroy()
    {
        scanReference.action.performed -= ScanPlot;


    }

    // Start is called before the first frame update
    void Start()
    {
        framesPerScan = GameManager.instance._framesPerScan;
    }

    // Update is called once per frame
    void Update()
    {

    }




    private void ScanPlot(InputAction.CallbackContext context)
    {
        Vector2 joystickPosition = scanReference.action.ReadValue<Vector2>();
        float pos = joystickPosition.x;
        _timesInvoked++;

        if (pos != 0f)
        {
            if ((_timesInvoked % framesPerScan) == 0)
            {
                _timesChanged++;
                GameManager.instance.ScanPlot(pos);
                Debug.Log("change plot " + _timesChanged);
            }

        }
    }
}
