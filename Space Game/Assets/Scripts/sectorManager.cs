using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class sectorManager : MonoBehaviour {

    public Sector sector;
    public serverScript dataServerManager;
    bool alreadyEdited;

    private void Start()
    {
         //If null then index does not exist.
         
    }
    private void Update()
    {
        if (Time.time > 30 && !alreadyEdited) 
        {
            alreadyEdited = true;
            sector = formatSector("Tom2");
            print("Component 0 position: " + sector.x);
            dataServerManager.editSector("Tom2", new Sector("EditedTom", Vector3.down, 21.5f));
            
        }
       
    }
    
    public Sector formatSector (string sectorName)
    {
        Sector formattedSector = new Sector(string.Empty, Vector3.zero, 0);
        foreach (string unformattedSector in dataServerManager.savedUnformattedSectors)
        {
            Sector formatThisSector = JsonConvert.DeserializeObject<Sector>(unformattedSector);
            string curName = formatThisSector.name;
            if (curName == sectorName)
            {
                formattedSector = formatThisSector;
                break;
            }
        }
     
        return formattedSector;
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

    public buildingComponent[] components;

}
[System.Serializable]
public class buildingComponent : ScriptableObject
{
    public string componentType;
    public float xPos,yPos,zPos;
    public float xRot,yRot,zRot;


}