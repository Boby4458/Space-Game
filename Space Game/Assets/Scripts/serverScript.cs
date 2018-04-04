using UnityEngine;
using System.Collections;
using DatabaseControl;
using System.Collections.Generic;
using Newtonsoft.Json;

public class serverScript : MonoBehaviour
{

    public string serverName;
    public string[] savedUnformattedSectors;
    public sectorManager sectorManagment;

    private void Start()
    {
        InvokeRepeating("downloadSectors", 0, 5);
    }
    private void Update()
    {
        
    }
   
    void createNewSector(Sector newSector)
    {
        string data = JsonConvert.SerializeObject(newSector);
        StartCoroutine(createNewSectorCoroutine(data + "+" ));
    }
    void downloadSectors()
    {
        StartCoroutine(getAllSectors());
    }
    public void resetServer()
    {
        StartCoroutine(resetServerCoroutine());
    }
    public void editSector(string sectorName, Sector replacementData)
    {
        string replacement = JsonConvert.SerializeObject(replacementData);
        StartCoroutine(editSectorCoroutine(sectorName, replacement));
    }
    IEnumerator editSectorCoroutine (string sectorName, string replacementData)
    {
        IEnumerator downloadData = DCF.GetUserData(serverName, serverName); 
        while (downloadData.MoveNext())
        {
            yield return downloadData.Current;
        }
        string dataJsonFormat = downloadData.Current.ToString ();
        string[] unformattedSectors = dataJsonFormat.Split('+');
        
        print("DataJsonFormat " + unformattedSectors [0]);

        string originalData = string.Empty;
        int originalDataIndex = -1;

        foreach (string unformattedSector in unformattedSectors)
        {
            originalDataIndex++;
            Sector formatThisSector = JsonConvert.DeserializeObject<Sector>(unformattedSector);
            string curName = formatThisSector.name;
            if (curName == sectorName)
            {
                originalData = unformattedSector;
                break;
            }
        }
        print("Original Data " + originalData);
        unformattedSectors[originalDataIndex] = replacementData;
        string newData = string.Empty;
        int loopIndex = -1;
        foreach (string _sector in unformattedSectors)
        {
            
            loopIndex++;
            if (_sector == string.Empty) continue;
            newData += unformattedSectors[loopIndex] + "+" ;
        }
        print("New Data " + newData);

        IEnumerator setData = DCF.SetUserData(serverName, serverName, newData); 
        {
            yield return setData.Current;
        }
        print("Edit finished");

    }
    IEnumerator resetServerCoroutine()
    {
        IEnumerator downloadData = DCF.GetUserData(serverName, serverName); 
        while (downloadData.MoveNext())
        {
            yield return downloadData.Current;
        }
        print("Existing data " + downloadData.Current.ToString());
        IEnumerator e = DCF.SetUserData(serverName, serverName, string.Empty); 
        {
            yield return e.Current;
        }
        string response = e.Current as string;
        print("Reset " + response);
    }
    IEnumerator createNewSectorCoroutine(string data)
    {
        IEnumerator downloadData = DCF.GetUserData(serverName, serverName); 
        while (downloadData.MoveNext())
        {
            yield return downloadData.Current;
        }
        print("Existing data " + downloadData.Current.ToString ());
        IEnumerator e = DCF.SetUserData(serverName, serverName, downloadData.Current + data); 
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; 
        print("Upload Status: " + response);
        StartCoroutine(getAllSectors());
    }
    IEnumerator getAllSectors()
    {
        IEnumerator e = DCF.GetUserData(serverName, serverName);
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; 
        formatDataIntoUnformattedSectors(response);
    }
    void formatDataIntoUnformattedSectors(string rawData)
    {
        string[] unformattedSectors = rawData.Split('+');
        savedUnformattedSectors = unformattedSectors;
        sectorManagment.formatAllSectors();
    }

}

    //        DCF.GetUserData("publicserver1", "publicserver1");

    /*
     const string privateCode = "FgeAQUh--kSaWBR8WGRTtwi-qwiUFXvU-g8F-TCx-ZJQ";
     const string publicCode = "5ac0c785012b2e1068e85bc2";
     const string webURL = "http://dreamlo.com/lb/";

     public unformattedSector[] unformattedSectorsList;
     public string displaydata;

     void Awake()
     {

         Sector newSector = new Sector();
         newSector.name = "Tom";
         newSector.sectorRadius = 50;
         newSector.sectorPosition = new Vector3(20, 122, 12);
         AddNewSector(newSector, 2);
         // AddNewSector(newSectorTwo, 2);
         //AddNewSector(newSectorThree, 4);

         DownloadSectors();
     }

     public void AddNewSector(Sector newSector, int index)
     {
         string data = JsonUtility.ToJson(newSector);
         displaydata = data;
         data = data.Replace(':', '+');
         data = data.Replace('"', '-');
         print("Data: " + data);

         StartCoroutine(UploadNewSector(data, index));
     }

     IEnumerator UploadNewSector(string data, int index)
     {
         WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(data) + "/" + index);
         yield return www;

         if (string.IsNullOrEmpty(www.error))
             print("Upload Successful");
         else
         {
             print("Error uploading: " + www.error);
         }
     }

     public void DownloadSectors()
     {
         StartCoroutine("DownloadSectorsFromDatabase");
     }

     IEnumerator DownloadSectorsFromDatabase()
     {
         WWW www = new WWW(webURL + publicCode + "/pipe/");
         yield return www;

         if (string.IsNullOrEmpty(www.error))
             FormatData(www.text);
         else
         {
             print("Error Downloading: " + www.error);
         }
     }

     void FormatData(string textStream)
     {
         Debug.LogError("Working");
         string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
         unformattedSectorsList = new unformattedSector[entries.Length];

         for (int i = 0; i < entries.Length; i++)
         {
             string[] entryInfo = entries[i].Split(new char[] { '|' });
             string data = entryInfo[0];
             int index = int.Parse(entryInfo[1]);
             unformattedSectorsList[i] = new unformattedSector(data, index);
             print("data: " + unformattedSectorsList[i].data + " index: " + unformattedSectorsList[i].index);
         }

     }

 }
 [System.Serializable]
 public class unformattedSector
 {
     public int index;

     public string data;

     public unformattedSector(string _data, int _index)
     {
         data = _data;
         index = _index;
     }
     */


