using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    public GameObject roomButtonPrefab;       // Prefab of Room Button 
    public Transform contentParent;           // Parent for UI buttons
    private List<GameObject> roomButtons = new List<GameObject>(); // holds buttons

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 1. Remove old buttons
        foreach (var btn in roomButtons)
        {
            Destroy(btn);
        }
        roomButtons.Clear();

        // 2. Create new buttons for updated room list
        foreach (var room in roomList)
        {
            if (room.IsOpen && room.IsVisible && room.PlayerCount >= 1)
            {
                GameObject newButton = Instantiate(roomButtonPrefab, contentParent);
                newButton.GetComponent<RoomButton>().textName.text = room.Name;

                roomButtons.Add(newButton);
            }
        }
    }
}
