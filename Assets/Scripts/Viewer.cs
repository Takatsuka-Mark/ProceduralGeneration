using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using defaultNamespace;
using UnityEngine.Experimental.GlobalIllumination;

public class Viewer : MonoBehaviour
{
    private Transform _viewerTransform;
    private GameObject _root;
    private GameObject _cam;
    private GameObject _light;
    private Camera _viewer;
    private Rigidbody _rb;
    private Light _viewerLight;
    

    private float rotX = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        init();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        rotateViewer();
        viewerMove();
    }

    void init()
    {
        _root = new GameObject();
        _rb = _root.AddComponent(typeof(Rigidbody)) as Rigidbody;
        _rb.useGravity = false;
        _viewerTransform = _root.transform;
        _root.name = "Viewer Group";
        
        _cam = new GameObject();
        _viewer = _cam.AddComponent(typeof(Camera)) as Camera;
        System.Diagnostics.Debug.Assert(_viewer != null, nameof(_viewer) + " != null");
        var transform1 = _viewer.transform;
        var rootTransform = _root.transform;
        transform1.position = rootTransform.position;
        transform1.rotation = _root.transform.rotation;
        transform1.parent = _root.transform;
        _viewer.name = "ViewerCamera";

        _light = new GameObject();
        _viewerLight = _light.AddComponent(typeof(Light)) as Light;
        System.Diagnostics.Debug.Assert(_viewerLight != null, nameof(_viewerLight) + " != null");
        var transform2 = _viewerLight.transform;
        // ReSharper disable once Unity.InefficientPropertyAccess
        transform2.position = rootTransform.position;
        transform2.rotation = rootTransform.rotation;
        transform2.parent = rootTransform;
        _viewerLight.name = "ViewerLight";


    }

    void rotateViewer()
    {
        var rotY = Input.GetAxis("Mouse X") * Constants.MouseSensitivity * Time.deltaTime;
        rotX += Input.GetAxis("Mouse Y") * Constants.MouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -90f, 90f);
        _viewerTransform.Rotate(Vector3.up * rotY);
        _viewer.transform.localRotation = Quaternion.Euler(-rotX, 0f, 0f);
    }
    void viewerMove()
    {
        var trueForward = _viewer.transform.forward + _viewerTransform.forward;
        trueForward = trueForward.normalized;
        
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (Math.Abs(Input.GetAxisRaw("Vertical")) != 0)
        {
            _rb.velocity = trueForward * (Constants.MovementSpeed);
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }
}
