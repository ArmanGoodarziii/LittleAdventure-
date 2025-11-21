using UnityEngine;
using Photon.Pun;
public class PlayerSpowner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(-3,1,0), Quaternion.identity);
    }
}
