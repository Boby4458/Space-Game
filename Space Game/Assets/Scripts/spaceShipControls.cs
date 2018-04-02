using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceShipControls : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 turningVelocity; 
    public Vector3 wantedVelocity;
    public Vector3 wantedTurningVelocity;

    public float curVelocity;

    public float accelerationSpeed;
    public float turningAccelerationSpeed;

    public float maxForwardVelocity;
    bool allowTurning = true;

    void Update() { 
        if (Input.GetMouseButtonDown (1))
        {
            allowTurning = !allowTurning;
        }
        
        if (Input.GetAxisRaw ("Vertical") != 0)
        {
            wantedVelocity = Input.GetAxis("Vertical") * (transform.forward * maxForwardVelocity);

        }
        if (Input.GetKey(KeyCode.E))
        {
            wantedVelocity = Vector3.zero;

        }
        
        velocity = Vector3.Lerp(velocity, wantedVelocity, accelerationSpeed);
        curVelocity = velocity.magnitude;
        transform.position += velocity * Time.deltaTime;
        Vector3 mousePosRelativeToCenter = Vector3.zero;
        if (allowTurning)
        {
            mousePosRelativeToCenter = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
        }
        
        //print("mouse relative pos " + mousePosRelativeToCenter.ToString());
        wantedTurningVelocity = new Vector3(-mousePosRelativeToCenter.y, mousePosRelativeToCenter.x, 0) / 10;
        turningVelocity = Vector3.Lerp(turningVelocity, wantedTurningVelocity, turningAccelerationSpeed);
        transform.Rotate(turningVelocity * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        
        
    }
    public void OnGUI()
    {
        GUILayout.Label("Velocity: " + curVelocity.ToString () + " Velocity Vector " + velocity + " Position " + transform.position.ToString ());
    }
}
