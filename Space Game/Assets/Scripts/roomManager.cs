using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class roomManager : MonoBehaviour {

    public mainNetworkManager networkManager;
    public Transform myShip;
    public Text sectorNameDisplay;
    public Text sectorPositionDisplay;
    public sectorManager sM;
    public Sector curSector;
    public long xPos, yPos, zPos;
    public string servername;

    private void Start()
    {
        Invoke ("calculateCurSector", 3);

    }
    void Update()
    {

        if (myShip == null) return;
        calculateCurSector();

        print(PhotonNetwork.room.Name);
        
        sectorNameDisplay.text = curSector.name;
        sectorPositionDisplay.text = "Position Relative To Center: " + curSector.x.ToString () + ", " + curSector.y.ToString () + ", " + curSector.z.ToString();

    }
    void calculateCurSector()
    {
        foreach (Sector sector in sM.allSectors)
        {
            if (sector == null)
            {
                Sector _sector = new Sector("Space", Vector3.zero, 100000);

                if (curSector.name != _sector.name)
                {
                    curSector = _sector;
                    if (PhotonNetwork.inRoom)
                    {
                        PhotonNetwork.LeaveRoom();
                        PhotonNetwork.JoinRoom(servername + "_sector_" + _sector.name);
                    }
                    else
                    {
                        networkManager.joinSector();
                    }
                }
                return;
            }
            if (sector.name  == curSector.name)
            {
                print("it's equal");
            }else
            {
                print("it's not equal sector " + sector.name + " curSector " + curSector.name);
            }
            try { 
            if (Vector3.Distance (getPlayerPos () , new Vector3 (sector.x, sector.y, sector.z)) <= sector.sectorRadius)
            {
                    if (curSector.name != sector.name)
                    {
                        curSector = sector;
                        if (PhotonNetwork.inRoom)
                        {
                            PhotonNetwork.LeaveRoom();
                            try
                            {
                                PhotonNetwork.JoinRoom(servername + "_sector_" + curSector.name);

                            }catch
                            {

                            }
                        }
                        else
                        {
                            networkManager.joinSector();
                        }
                      
                    }
                    return;

                }
            }catch
            {
                Sector _sector = new Sector("Space", Vector3.zero, 100000);

                if (curSector.name != _sector.name)
                {
                    curSector = _sector;
                    if (PhotonNetwork.inRoom)
                    {
                        PhotonNetwork.LeaveRoom();
                        PhotonNetwork.JoinRoom(servername + "_sector_" + _sector.name);
                    }
                    else
                    {
                        networkManager.joinSector();
                    }
                }
                return;
            }
        }
    }
    Vector3 getPlayerPos()
    {
        if (myShip != null)
        {
            return myShip.position;
        }
        else
        {
            return (Vector3.zero);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector3.zero, curSector.sectorRadius);
    }
    
}
