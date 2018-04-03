using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour {

    public Transform myShip;
    public float cameraFollowSmooth;
    public float rotationSmooth;
    public float turningSpeed;

    bool lockMode = true;

    private void Update()
    {
        if (myShip == null) return;
        transform.position = Vector3.Lerp(transform.position, myShip.position, cameraFollowSmooth * Time.deltaTime); 
        if (Input.GetMouseButtonDown(1))
        {
            lockMode = !lockMode;
        }
        
        
        if (lockMode)
        {
            transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, myShip.eulerAngles.x, rotationSmooth * Time.deltaTime), Mathf.LerpAngle(transform.eulerAngles.y, myShip.eulerAngles.y, rotationSmooth * Time.deltaTime), 0);
        }
        else
        {
            transform.Rotate(-Input.GetAxis("Mouse Y") * turningSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * turningSpeed * Time.deltaTime, 0);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
    }
}