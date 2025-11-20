using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]private InputField createInputField;

    // Create a new Room
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInputField.text, new RoomOptions(){MaxPlayers = 4, IsVisible = true, IsOpen = true}, TypedLobby.Default, null);
    }

    
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Level");
    }
    public void JoinRoomInList(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}
