using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public Text textName;
    private LobbyManager lobbyManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lobbyManager = FindFirstObjectByType<LobbyManager>();
    }

    public void JoinRoom()
    {
        lobbyManager.JoinRoomInList(textName.text);
    }
}
