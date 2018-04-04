using UnityEngine;

public class mainNetworkManager : Photon.PunBehaviour
{
    private PhotonView myPhotonView;
    public roomManager roommanager;
    public string servername;
    Vector3 playerPos;
    GameObject myPlayer;
    Vector3 forwardTransform;
    Quaternion shipRotation;

    public void joinSector()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }
    private void Update()
    {
        if (myPlayer != null)
        {
            playerPos = myPlayer.transform.position;
            forwardTransform = myPlayer.transform.forward;
            shipRotation = transform.rotation;


        }
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
        GameObject mySpaceship = PhotonNetwork.Instantiate("Spaceship", playerPos + forwardTransform * 2, shipRotation, 0);
        myPlayer = mySpaceship;
        mySpaceship.GetComponent<spaceShipControls>().enabled = true;
        GameObject.Find("CameraOrbitPoint").GetComponent<cameraControls>().myShip = mySpaceship.transform;
        roommanager = GameObject.Find("Room Manager").GetComponent<roomManager>();
        roommanager.myShip = mySpaceship.transform;
        roommanager.networkManager = this;



    }


}
