using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomListing;

    void Update()
    {
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // foreach (RectTransform child in _content)
        // {
        //     Destroy(child.gameObject);
        // }

        foreach (RoomInfo info in roomList)
        {
            RoomListing listing = Instantiate(_roomListing, _content);
            if (listing != null)
            {
                listing.SetRoomInfo(info);
            }
            else return;
        }
    }
}
