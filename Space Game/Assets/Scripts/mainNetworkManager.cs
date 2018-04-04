using UnityEngine;

public class mainNetworkManager : Photon.PunBehaviour
{
    private PhotonView myPhotonView;
    public roomManager roommanager;
    public string servername;
    Vector3 playerPos;
    public GameObject myPlayer;
    Vector3 forwardTransform;
    Vector3 shipRotation;
    Vector3 shipVelocity;
    Vector3 shipTurningVelocity;

    public GameObject loadingScreen;
    
    public void joinSector()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }
    private void Update()
    {
        loadingScreen.SetActive(!PhotonNetwork.inRoom);
        if (myPlayer != null)
        {
            playerPos = myPlayer.transform.position;
            forwardTransform = myPlayer.transform.forward;
            shipRotation = myPlayer.transform.eulerAngles;
            shipVelocity = myPlayer.GetComponent<spaceShipControls>().velocity;
            shipTurningVelocity = myPlayer.GetComponent<spaceShipControls>().turningVelocity;


        }
    }
    public override void OnJoinedLobby()
    {

        PhotonNetwork.JoinRoom(servername + "_sector_" + roommanager.curSector.name);

    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        myPlayer = null;
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
        GameObject mySpaceship = PhotonNetwork.Instantiate("Spaceship", playerPos + forwardTransform * 2, Quaternion.identity, 0);
        mySpaceship.transform.eulerAngles = shipRotation;
        mySpaceship.GetComponent<spaceShipControls>().velocity = shipVelocity;
        mySpaceship.GetComponent<spaceShipControls>().turningVelocity = shipTurningVelocity;

        mySpaceship.GetComponent<spaceShipControls>().enabled = true;
        print("rotation: " + shipRotation);
        GameObject.Find("CameraOrbitPoint").GetComponent<cameraControls>().myShip = mySpaceship.transform;
        roommanager = GameObject.Find("Room Manager").GetComponent<roomManager>();
        roommanager.myShip = mySpaceship.transform;
        roommanager.networkManager = this;
        myPlayer = mySpaceship;


    }


}
