using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Time.timeScale = 1f;
    }

    #region Player Win / Lose

    // وقتی بازیکن به خط پایان رسید
    public void PlayerWon()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;

        // پنل Win فقط برای بازیکن برنده
        photonView.RPC("RPC_ShowWin", photonView.Owner);

        // پنل Lose برای بقیه بازیکن‌ها
        photonView.RPC("RPC_ShowLose", RpcTarget.Others);

        // برگشت به Lobby بعد از 2 ثانیه
        LeaveRoomAndReturnLobbyDelayed();
    }

    // وقتی بازیکن سقوط کرد
    public void PlayerLost()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;

        // بازیکن خودش Lose ببیند
        photonView.RPC("RPC_ShowLose", photonView.Owner);

        // بازیکن مقابل Win ببیند
        photonView.RPC("RPC_ShowWin", RpcTarget.Others);

        // برگشت به Lobby بعد از 2 ثانیه
        LeaveRoomAndReturnLobbyDelayed();
    }

    #endregion

    #region RPCs

    [PunRPC]
    private void RPC_ShowWin()
    {
        Time.timeScale = 0f;

        if (winPanel != null)
            winPanel.SetActive(true);

        FindLocalPlayerMovement()?.StopMovement();
    }

    [PunRPC]
    private void RPC_ShowLose()
    {
        Time.timeScale = 0f;

        if (losePanel != null)
            losePanel.SetActive(true);

        FindLocalPlayerMovement()?.StopMovement();
    }

    #endregion

    #region Helper Methods

    private PlayeroMovement FindLocalPlayerMovement()
    {
        PlayerState[] players = FindObjectsByType<PlayerState>(FindObjectsSortMode.None);
        foreach (var p in players)
        {
            if (p.photonView.IsMine)
                return p.GetComponent<PlayeroMovement>();
        }
        return null;
    }

    #endregion

    #region Leave Room

    private void LeaveRoomAndReturnLobbyDelayed()
    {
        StartCoroutine(LeaveAfterDelay(2f));
    }

    private IEnumerator LeaveAfterDelay(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds); // استفاده از Realtime تا Time.timeScale تاثیری نداشته باشد
        Time.timeScale = 1f;

        // بررسی وضعیت قبل از ترک روم
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.LogWarning("Cannot leave room: client not connected or not in a room.");
            PhotonNetwork.LoadLevel("Lobby"); // در صورت مشکل، مستقیم به Lobby برگرد
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

    #endregion
}
