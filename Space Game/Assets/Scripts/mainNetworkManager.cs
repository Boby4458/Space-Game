using UnityEngine;

public class mainNetworkManager : Photon.PunBehaviour
{
    private PhotonView myPhotonView;
    public roomManager roommanager;
    public string servername;

    public void joinSector()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {

        PhotonNetwork.JoinRoom(servername + "_sector_" + roommanager.curSector.name);

    }

    public override void OnConnectedToMaster()
    {

        PhotonNetwork.JoinRoom(servername + "_sector_" + roommanager.curSector.name);
    }
    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        //base.OnPhotonJoinRoomFailed(codeAndMsg);
        PhotonNetwork.CreateRoom(servername + "_sector_" + roommanager.curSector.name);

    }

    public override void OnJoinedRoom ()
    {
        GameObject mySpaceship = PhotonNetwork.Instantiate("Spaceship", Vector3.zero + Vector3.up * 10, Quaternion.identity, 0);
        mySpaceship.GetComponent<spaceShipControls>().enabled = true;
        GameObject.Find("CameraOrbitPoint").GetComponent<cameraControls>().myShip = mySpaceship.transform;
        roommanager = GameObject.Find("Room Manager").GetComponent<roomManager>();
        roommanager.myShip = mySpaceship.transform;
        roommanager.networkManager = this;



    }


}
