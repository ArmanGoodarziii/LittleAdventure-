using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class UsernameManager : MonoBehaviour
{
    public InputField inputField;
    public GameObject userNamePage;
    public Text userName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PlayerPrefs.GetString("Username") == "" || PlayerPrefs.GetString("Username") == null)
        {
            userNamePage.SetActive(true);
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("Username");
            userName.text = "Username: " + PlayerPrefs.GetString("Username");
            userNamePage.SetActive(false);
        }
    }

    public void saveUserName()
    {
        PhotonNetwork.NickName = inputField.text;

        PlayerPrefs.SetString("Username", inputField.text);
        userName.text = "Username: " + inputField.text;
        userNamePage.SetActive(false);
    }
}
