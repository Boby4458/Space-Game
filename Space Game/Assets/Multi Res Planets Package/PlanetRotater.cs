using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotater : MonoBehaviour {

    public Vector3 spinAxis;
	
	void Update () {
        transform.Rotate(spinAxis * Time.deltaTime,Space.World);
	}
}
