using UnityEngine;
using Photon.Pun;

public class PlayerState : MonoBehaviourPun
{
    [SerializeField] private float fallY; // ارتفاع سقوط برای باخت

    private bool isFinished = false;

    private void Update()
    {
        if (!photonView.IsMine) return;

        // اگر بازیکن سقوط کرد
        if (!isFinished && transform.position.y < fallY)
        {
            isFinished = true; // جلوگیری از فراخوانی چندباره
            GameManager.Instance.PlayerLost();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine) return;

        // وقتی بازیکن به Finish رسید
        if (!isFinished && other.CompareTag("Finish"))
        {
            isFinished = true; // جلوگیری از فراخوانی چندباره
            GameManager.Instance.PlayerWon();
        }
    }
}
