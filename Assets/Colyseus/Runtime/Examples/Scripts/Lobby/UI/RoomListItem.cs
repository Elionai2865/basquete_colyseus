using Colyseus;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField]
    private Text clientCount = null;

    [SerializeField]
    private Button joinButton = null;

    private RoomSelectionMenu menuRef;

    [SerializeField]
    private Text roomName = null;

    private ColyseusRoomAvailable roomRef;

	public uint clientes; //MODH

	public GameObject Manager; //MODH

	ExampleManager managerScript;

    public void Initialize(ColyseusRoomAvailable roomReference, RoomSelectionMenu menu)
    {
        menuRef = menu;
        roomRef = roomReference;
        roomName.text = roomReference.roomId;
		clientes = roomReference.clients; //MODH

		Manager = GameObject.Find("Manager"); //MODH
		managerScript = Manager.GetComponent<ExampleManager>(); //MODH
		managerScript.clientsNumber = clientes; //MODH

		string maxClients = roomReference.maxClients > 0 ? roomReference.maxClients.ToString() : "--";
        clientCount.text = $"{roomReference.clients} / {maxClients}";
        //TODO: if we want to lock rooms, will need to do so here
        if (roomReference.maxClients > 0 && roomReference.clients >= roomReference.maxClients)
        {
            joinButton.interactable = false;
        }
        else
        {
            joinButton.interactable = true;
        }
    }

    public void TryJoin()
    {
		Manager = GameObject.Find("Manager"); //MODH
		managerScript = Manager.GetComponent<ExampleManager>(); //MODH
		//managerScript.clientsNumber = clientes; //MODH
		menuRef.JoinRoom(roomRef.roomId);
    }
}