using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using defaultNamespace;
using UnityEngine.Experimental.GlobalIllumination;
using LightType = UnityEngine.LightType;

public class Viewer : MonoBehaviour
{
    private Transform _viewerTransform;
    private GameObject _root;
    private GameObject _cam;
    private GameObject _light;
    private Camera _viewer;
    private Rigidbody _rb;
    private Light _viewerLight;
    private bool _rotation = true;
    
    private float _rotX;
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

    /// <summary>
    /// Initializes the objects of the viewer group in the actual unity
    /// </summary>
    void init()
    {
        //Creates the root empty object to group all the viewer items together
        _root = new GameObject();
        _rb = _root.AddComponent(typeof(Rigidbody)) as Rigidbody;
        _rb.useGravity = false;
        _viewerTransform = _root.transform;
        _root.name = "Viewer Group";
        
        //Creates the main camera object 
        _cam = new GameObject();
        _viewer = _cam.AddComponent(typeof(Camera)) as Camera;
        System.Diagnostics.Debug.Assert(_viewer != null, nameof(_viewer) + " != null");
        var transform1 = _viewer.transform;
        var rootTransform = _root.transform;
        transform1.position = rootTransform.position;
        transform1.rotation = _root.transform.rotation;
        transform1.parent = _root.transform;
        _viewer.name = "ViewerCamera";

        //Creates the light so the viewer can always see
        _light = new GameObject();
        _viewerLight = _light.AddComponent(typeof(Light)) as Light;
        System.Diagnostics.Debug.Assert(_viewerLight != null, nameof(_viewerLight) + " != null");
        var transform2 = _viewerLight.transform;
        _viewerLight.type = LightType.Directional;
        // ReSharper disable once Unity.InefficientPropertyAccess
        transform2.position = rootTransform.position;
        transform2.rotation = rootTransform.rotation;
        transform2.parent = rootTransform;
        _viewerLight.name = "ViewerLight";


    }

    /// <summary>
    /// Handles the looking around of the viewer
    /// </summary>
    void rotateViewer()
    {
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            _rotation = !_rotation;
        }

        if (!_rotation) return;
        var rotY = Input.GetAxis("Mouse X") * Constants.MouseSensitivity * Time.deltaTime;
        _rotX += Input.GetAxis("Mouse Y") * Constants.MouseSensitivity * Time.deltaTime;
        _rotX = Mathf.Clamp(_rotX, -90f, 90f);
        _viewerTransform.Rotate(Vector3.up * rotY);
        _viewer.transform.localRotation = Quaternion.Euler(-_rotX, 0f, 0f);
    }
    
    /// <summary>
    /// Handles all the movement of the viewer group
    /// </summary>
    void viewerMove()
    {
        var trueForward = _viewer.transform.forward + _viewerTransform.forward;
        trueForward = trueForward.normalized;
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        
        Debug.Log(horizontal + " " + vertical + " " + _viewer.transform.right);

        if (vertical != 0 || horizontal != 0 )
            _rb.velocity = (trueForward * vertical + 
                            (_viewer.transform.right + _viewerTransform.right).normalized * horizontal)
                            * Constants.MovementSpeed;
        else
            _rb.velocity = Vector3.zero;
    }
}
