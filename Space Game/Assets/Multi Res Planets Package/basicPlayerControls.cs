using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayerControls : MonoBehaviour {

    public  float moveSpeed = 30;
    public float rotSpeed = 30;

	void Update () {

        transform.Translate(new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")) * Time.deltaTime * moveSpeed,Space.Self);

        if (Input.GetMouseButton(1)) {
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"),Input.GetAxis("Mouse X"),0) * Time.deltaTime * rotSpeed);
        }

        if (Input.GetKey(KeyCode.E)) {
            transform.Rotate(new Vector3(0,0, -rotSpeed * 2) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, 0, rotSpeed * 2) * Time.deltaTime);
        }

        // Debug.Log(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }
}
