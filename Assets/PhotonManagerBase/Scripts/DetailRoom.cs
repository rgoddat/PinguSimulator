using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetailRoom : MonoBehaviour
{

    public Text txtRoomName;
    public Text txtRoomNbPlayers;

    public void UpdateUI(RoomInfo infos)
    {
        txtRoomName.text = infos.Name;
        txtRoomNbPlayers.text = string.Format("Players: {0}/{1}", infos.PlayerCount, infos.MaxPlayers);
    }
}
