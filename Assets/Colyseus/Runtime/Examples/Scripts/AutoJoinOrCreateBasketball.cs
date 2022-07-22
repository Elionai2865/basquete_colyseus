using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoJoinOrCreateBasketball : MonoBehaviour
{

	public GameObject theRoomEntry;
	public GameObject cover;
	public LobbyController myLobby;

	private bool yes = false;
	private bool onlyOnce = true;

	public RoomSelectionMenu myRoomSelection;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (yes && myRoomSelection.roomsHanded)
		{
			StartCoroutine(timeAuto());
		}
	}

	public void joinOrEntry()
	{
		if (onlyOnce)
		{
			theRoomEntry = GameObject.FindWithTag("roomEntry");
			if (theRoomEntry == null)
			{
				myLobby.CreateRoomAuto();
			}
			else
			{
				theRoomEntry.GetComponent<RoomListItem>().TryJoin();
			}
			onlyOnce = false;
		}
	}

	IEnumerator timeAuto()
	{
		yield return new WaitForSeconds(0.1f);
		joinOrEntry();
	}

	public void no()
	{
		yes = true;
		cover.SetActive(true);
	}
}