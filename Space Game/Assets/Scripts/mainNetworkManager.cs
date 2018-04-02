using UnityEngine;

public class mainNetworkManager : Photon.PunBehaviour
{
    private PhotonView myPhotonView;

    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    public void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        GameObject mySpaceship = PhotonNetwork.Instantiate("Spaceship", Vector3.zero + Vector3.up * 10, Quaternion.identity, 0);
        mySpaceship.GetComponent<spaceShipControls>().enabled = true;
        GameObject.Find("CameraOrbitPoint").GetComponent<cameraControls>().myShip = mySpaceship.transform;
    }

   
}
