using Colyseus;
using LucidSightTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLobbyAutoBasketball : MonoBehaviour
{

	public GameObject manager;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		manager = GameObject.Find("Manager");
	}

	public void lobbyAuto()
	{
		manager.GetComponent<ExampleManager>().LeaveAllRooms(null);
		Application.LoadLevel(2);
		//SceneManager.LoadScene("FootballLobby");
	}
}
