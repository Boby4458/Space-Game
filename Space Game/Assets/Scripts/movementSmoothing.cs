using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class movementSmoothing : Photon.MonoBehaviour
{
    public float SmoothingDelay;
  

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; 
    private Quaternion correctPlayerRot = Quaternion.identity;

    public void Update()
    {
        if (!photonView.isMine)
        {
            
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
        }
    }

}