using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomManager : MonoBehaviour {

    public float sectorSize;

    void Update()
    {
        if (!PhotonNetwork.inRoom)
        {
            return;
        }
        bool playerInSector = false;

       foreach (Collider _GameObject in Physics.OverlapSphere(Vector3.zero, sectorSize)){
            if (_GameObject.GetComponent<spaceShipControls> () != null)
            {
                if (_GameObject.GetComponent<spaceShipControls>().enabled) //Ensuring that this is my ship.
                {
                    playerInSector = true;

                }
            }

        }


        if (!playerInSector)
        {
             PhotonNetwork.Disconnect();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector3.zero, sectorSize);
    }
}
