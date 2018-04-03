using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roomManager : MonoBehaviour {

    public float sectorSize;
    public mainNetworkManager networkManager;
    public Transform myShip;
    public Text sectorNameDisplay;
    public Text sectorPositionDisplay;
    public sectorManager sM;
    public Sector curSector;
    public Vector3 spaceshipPositionRelativeToCenterOfUniverse;

    void Update()
    {
        calculateCurSector();

        if (myShip == null) return;

        if (Vector3.Distance (myShip.position, Vector3.zero) > sectorSize)
        {
            PhotonNetwork.Disconnect();
        }

        sectorNameDisplay.text = curSector.name;
        sectorPositionDisplay.text = curSector.x.ToString () + ", " + curSector.y.ToString () + ", " + curSector.z.ToString();

    }
    void calculateCurSector()
    {
        foreach (Sector sector in sM.allSectors)
        {
            if (Vector3.Distance (spaceshipPositionRelativeToCenterOfUniverse, new Vector3 (sector.x, sector.y, sector.z)) <= sector.sectorRadius)
            {
                curSector = sector;
                networkManager.joinSector();
                print("Name " + curSector.name);
                return;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector3.zero, sectorSize);
    }
}
