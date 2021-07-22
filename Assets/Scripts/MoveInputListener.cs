using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveInputListener : MonoBehaviour
{
    public InputActionReference uiPlaceReference = null;
    public GameObject _inputUI;


    public InputActionReference moveReference = null;
    public InputActionProperty leftHandPosition;
    public InputActionProperty playerPosition;

    private int _timesInvoked = 0, framesPerScan;

    private Vector3 _controllerPos, _moveDirection;

    private bool _moving = false;

    // Start is called before the first frame update
    void Start()
    {
        framesPerScan = GameManager.instance._framesPerScan;
    }

    // Update is called once per frame
    void Update()
    {
        if (_moving)
        {
            //Frameabfrage
            if ((_timesInvoked % framesPerScan) == 0)
            {
                Vector3 controllerPos = leftHandPosition.action.ReadValue<Vector3>();
                Debug.Log($"Keep Moving. Controller at {controllerPos.x},{controllerPos.y},{controllerPos.z}");
                _moveDirection = _controllerPos - controllerPos;
                GameManager.instance.UpdateRange(_moveDirection);
                _controllerPos = controllerPos;
            }
            _timesInvoked++;

        }
    }

    private void Awake()
    {
        moveReference.action.started += StartMoving;
        moveReference.action.canceled += StopMoving;
        uiPlaceReference.action.performed += PlaceUI;
    }

    private void OnDestroy()
    {
        moveReference.action.started -= StartMoving;
        moveReference.action.canceled -= StopMoving;
        uiPlaceReference.action.performed -= PlaceUI;
    }

    private void StartMoving(InputAction.CallbackContext context)
    {

        _moving = true;
        _controllerPos = leftHandPosition.action.ReadValue<Vector3>();
        _timesInvoked = 0;
        //Debug.Log($"Start Moving. Controller at {_controllerPos.x},{_controllerPos.y},{_controllerPos.z}");
    }
    private void StopMoving(InputAction.CallbackContext context)
    {
        _moving = false;
    }

    private void PlaceUI(InputAction.CallbackContext context)
    {
        _inputUI.transform.position = leftHandPosition.action.ReadValue<Vector3>();

        Vector3 pos = playerPosition.action.ReadValue<Vector3>();

        _inputUI.transform.LookAt(pos);
        _inputUI.transform.localRotation *= Quaternion.Euler(0, 180, 0);
    }
}
