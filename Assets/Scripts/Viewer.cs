using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using defaultNamespace;

public class Viewer : MonoBehaviour
{
    public Transform viewerTransform;
    public GameObject viewer;

    private float rotX = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        rotateViewer();
    }

    void rotateViewer()
    {
        var rotY = Input.GetAxis("Mouse X") * Constants.MouseSensitivity * Time.deltaTime;
        rotX += Input.GetAxis("Mouse Y") * Constants.MouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -90f, 90f);
        viewerTransform.Rotate(Vector3.up * rotY);
        viewer.transform.localRotation = Quaternion.Euler(-rotX, 0f, 0f);
    }
    Vector3 getLookDir()
    {
        Vector3 lookDir = viewerTransform.eulerAngles;
        Debug.Log(lookDir);
        return Vector3.zero;
    }
}
