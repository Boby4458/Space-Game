using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class sectorManager : MonoBehaviour {

    public Sector[] allSectorsOne;
    public Sector[] allSectors;

    public serverScript dataServerManager;
    bool alreadyEdited;
    Vector3 forwardTransform;

    public SkyboxGenerator skyboxGenerator;

    private void Awake()
    {
        GenerateSkybox(Vector3.zero);
    }

    public void GenerateSkybox(Vector3 curPos)
    {
        if (skyboxGenerator != null)
        {

            List<Vector3> positionsToAssign = new List<Vector3>();

            foreach (Sector s in allSectors)
            {
                positionsToAssign.Add(new Vector3(s.x, s.y, s.z));
            }

            skyboxGenerator.skyObjects = positionsToAssign;
            skyboxGenerator.currentPosition = curPos;

        }
    }
    private void Update()
    {
       
       
    }
    
    public void formatAllSectors ()
    {
        int loopCounter = -1;
        allSectorsOne = new Sector[dataServerManager.savedUnformattedSectors.Length];
        foreach (string unformattedSector in dataServerManager.savedUnformattedSectors)
        {
            loopCounter++;
            allSectorsOne[loopCounter] = JsonConvert.DeserializeObject<Sector>(unformattedSector);

        }
        allSectors = allSectorsOne;
    }
}
[System.Serializable]
public class Sector
{
    public string name;
    public float x,y,z; //SectorPosition
    public float sectorRadius;

    public Sector (string _name, Vector3 sectorPosition, float _sectorRadius)
    {
        name = _name;
        x = sectorPosition.x;
        y = sectorPosition.y;
        z = sectorPosition.z;
        sectorRadius = _sectorRadius;

    }


}
[System.Serializable]
public class buildingComponent : ScriptableObject
{
    public string componentType;
    public float xPos,yPos,zPos;
    public float xRot,yRot,zRot;


}