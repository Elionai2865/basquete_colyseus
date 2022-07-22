using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Colyseus;

public class GameSceneManager : MonoBehaviour
{

	public uint playerNumber;
	public Text textoCentral;
	public Text texto2;
	public Text topLeftText;
	public GameObject[] playersConnected;
	public int numberOfPlayers;
	private int prevNumber;

	public bool itStarted = false;
	public bool showResult = false;
	public bool myTurn = false;

	public ShootAI shootDaMinhaBola;
	public GameObject minhaBola;
	public GameObject bolaDeTeste;

	public RectTransform rt;
	public RectTransform[] pTexts = new RectTransform[10];
	//rT.sizeDelta = new Vector2(rT.sizeDelta.x, rT.sizeDelta.y + 0.5f);

	[Header("UIeTextos")]
	public GameObject initialText;
	public GameObject scoreboard;
	public int[] playersScores = new int[10];
	public Text p1Score;
	public Text p2Score;
	public Text p3Score;
	public Text p4Score;
	public Text p5Score;
	public Text p6Score;
	public Text p7Score;
	public Text p8Score;
	public Text p9Score;
	public Text p10Score;

	[Header("Results")]
	public Text YouWin;
	public Text YouLose;
	public Text Draw;
	public Text TiedForFirst;
	public GameObject playAgain;

	private bool oneTimeOnly = false;
	private bool checkSeat = true;

	public GameObject Tries;

	void Awake()
    {
		YouWin.text = "";
		YouLose.text = "";
		Draw.text = "";
		TiedForFirst.text = "";
		Tries.SetActive(false);
		//pTexts[0].anchoredPosition = new Vector2(22.0f, -15.0f);
	}

	void Start()
    {
		playerNumber = GameObject.Find("Manager").GetComponent<ExampleManager>().clientsNumber + 1;
		if (playerNumber == 1) checkSeat = false;
	}

    void Update()
    {
		playersConnected = GameObject.FindGameObjectsWithTag("plusOnePlayer");
		prevNumber = numberOfPlayers;
		numberOfPlayers = playersConnected.Length + 1;

		//if (checkSeat && playersConnected.Length > 1 && !itStarted) checkTheSeat(); //NMODH
			
		if (prevNumber > numberOfPlayers && !oneTimeOnly) {
			adjustPlayerSeat();
		}

		if (!itStarted)
		{
			initialText.SetActive(true);
			scoreboard.SetActive(false);
			waitingMode();
		}
		else if (itStarted && !showResult)
        {
			playingMode();
			getScore();
        }
		else if (!oneTimeOnly)
        {
			getScore();
			sessionIsOver();
			oneTimeOnly = true;
			myTurn = false;
			minhaBola.GetComponent<Renderer>().enabled = false;
		}

		if(myTurn && itStarted)
        {
			shootDaMinhaBola.willBeShootByUser = true;
		}
		else
        {
			shootDaMinhaBola.willBeShootByUser = false;
		}
	}

	void checkTheSeat()
    {
		int i = 0;
		bool erro = false;
		while (i < playersConnected.Length)
        {
			if (playersConnected[i].GetComponent<BasketballView>().seat == playerNumber)
			{
				erro = true;
				playerNumber++;
			}
			i++;
		}
		//if (erro) playerNumber++;
		checkSeat = !erro;
    }

	void waitingMode()
    {
		texto2.text = "Players Connected: " + numberOfPlayers;
		textoCentral.text = "You Are Player " + playerNumber;
		if (playerNumber == 1 && numberOfPlayers >= 2)
		{
			topLeftText.text = "Press E to start";
			if (Input.GetKeyDown(KeyCode.E))
			{
				ExampleManager.NetSend("lockOrUnlockTheRoom", true);
				waitingModeIsOver();
			}
		}
		else if (numberOfPlayers == 1)
		{
			topLeftText.text = "Waiting for more players to join";
		}
		else if (playerNumber > 1)
		{
			topLeftText.text = "Waiting for P1 to start";
		}
		int j = 0;
		while (j < playersConnected.Length)
        {
			if(playersConnected[j].GetComponent<BasketballView>().theGameStarted)
            {
				waitingModeIsOver();
			}
			playersConnected[j].GetComponent<Renderer>().enabled = false; //MODH
			j++;
        }
	}

	void waitingModeIsOver()
    {
		bolaDeTeste.SetActive(false);
		minhaBola.GetComponent<Renderer>().enabled = true;
		minhaBola.GetComponent<Collider>().enabled = true;
		minhaBola.transform.localPosition = new Vector3(-8.686579e-07f, -0.4234794f, -14.0f);
		itStarted = true;
		textoCentral.text = "";
		texto2.text = "";
		topLeftText.text = "";
		initialText.SetActive(false);
		scoreboard.SetActive(true);
		Tries.SetActive(true);
		rt.sizeDelta = new Vector2(((numberOfPlayers + 1)/2) * 260.0f, rt.sizeDelta.y);
		int i = 0;
		while(i<10)
        {
			if(numberOfPlayers == 1 || numberOfPlayers == 2)
            {
				pTexts[i].anchoredPosition = new Vector2((542.0f + 271.0f * (((i+2)/2)-1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			else if (numberOfPlayers == 3 || numberOfPlayers == 4)
			{
				pTexts[i].anchoredPosition = new Vector2((406.5f + 271.0f * (((i + 2) / 2) - 1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			else if (numberOfPlayers == 5 || numberOfPlayers == 6)
			{
				pTexts[i].anchoredPosition = new Vector2((271.0f + 271.0f * (((i + 2) / 2) - 1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			else if (numberOfPlayers == 7 || numberOfPlayers == 8)
			{
				pTexts[i].anchoredPosition = new Vector2((135.5f + 271.0f * (((i + 2) / 2) - 1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			else if (numberOfPlayers == 9 || numberOfPlayers == 10)
			{
				pTexts[i].anchoredPosition = new Vector2((0.0f + 271.0f * (((i + 2) / 2) - 1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			i++;
        }
	}

	void playingMode()
    {
		int sumOfTries = 0;
		int i = 0;
		int j = 0;
		while (i < playersConnected.Length)
        {
			sumOfTries+= playersConnected[i].GetComponent<BasketballView>().tTries;
			i++;
		}
		sumOfTries += shootDaMinhaBola.tries;
		while (j < playersConnected.Length)
		{
			if (playersConnected[j].GetComponent<BasketballView>().seat != ((sumOfTries % numberOfPlayers) + 1))
			{
				playersConnected[j].GetComponent<Renderer>().enabled = false;
			}
			else
			{
				playersConnected[j].GetComponent<Renderer>().enabled = true;
			}
			j++;
		}
		if (playerNumber == ((sumOfTries % numberOfPlayers) + 1))
		{
			myTurn = true;
			minhaBola.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			myTurn = false;
			minhaBola.GetComponent<Renderer>().enabled = false;
		}
		if(myTurn)
        {
			topLeftText.text = "Your turn";
			topLeftText.GetComponent<Text>().color = Color.green;
		}
        else
        {
			topLeftText.GetComponent<Text>().color = Color.yellow;
			switch (((sumOfTries % numberOfPlayers) + 1))
            {
				case 1:
					topLeftText.text = "P1 Turn";
				break;
				case 2:
					topLeftText.text = "P2 Turn";
				break;
				case 3:
					topLeftText.text = "P3 Turn";
				break;
				case 4:
					topLeftText.text = "P4 Turn";
				break;
				case 5:
					topLeftText.text = "P5 Turn";
				break;
				case 6:
					topLeftText.text = "P6 Turn";
				break;
				case 7:
					topLeftText.text = "P7 Turn";
				break;
				case 8:
					topLeftText.text = "P8 Turn";
				break;
				case 9:
					topLeftText.text = "P9 Turn";
				break;
				case 10:
					topLeftText.text = "P10 Turn";
				break;
			}
        }
		if(sumOfTries == (numberOfPlayers*shootDaMinhaBola.maxTries))
        {
			showResult = true;
        }
	}

	void sessionIsOver()
    {
		topLeftText.GetComponent<Text>().color = Color.white;
		topLeftText.text = "SESSION IS OVER";
		scoreboard.SetActive(true);
		int j = 0;
		while (j < playersConnected.Length)
		{
			playersConnected[j].GetComponent<Renderer>().enabled = false; //MODH
			j++;
		}
		comparaPontuacao();
		playAgain.SetActive(true);
	}

	void comparaPontuacao()
    {
		int higherScore = 0;
		int i = 0;
		int myScore;
		while (i < playersConnected.Length)
		{
			if (playersConnected[i].GetComponent<BasketballView>().theScore > higherScore)
			{
				higherScore = playersConnected[i].GetComponent<BasketballView>().theScore;
			}
			i++;
		}
			myScore = minhaBola.GetComponent<UiManager>().myScore;
			if(myScore < higherScore)
            {
				YouLose.text = "YOU LOSE";
            }
			else if(myScore == higherScore)
            {
				if(playersConnected.Length == 1)
                {
					Draw.text = "DRAW";
                }
                else
                {
					TiedForFirst.text = "YOU TIED FOR FIRST PLACE";
                }
            }
			else if (myScore > higherScore)
            {
				YouWin.text = "YOU WIN";
				higherScore = myScore;
            }

		if(playersScores[0] >= higherScore) p1Score.GetComponent<Text>().color = Color.green;
		else p1Score.GetComponent<Text>().color = Color.red;

		if (playersScores[1] >= higherScore) p2Score.GetComponent<Text>().color = Color.green;
		else p2Score.GetComponent<Text>().color = Color.red;

		if (playersScores[2] >= higherScore) p3Score.GetComponent<Text>().color = Color.green;
		else p3Score.GetComponent<Text>().color = Color.red;

		if (playersScores[3] >= higherScore) p4Score.GetComponent<Text>().color = Color.green;
		else p4Score.GetComponent<Text>().color = Color.red;

		if (playersScores[4] >= higherScore) p5Score.GetComponent<Text>().color = Color.green;
		else p5Score.GetComponent<Text>().color = Color.red;

		if (playersScores[5] >= higherScore) p6Score.GetComponent<Text>().color = Color.green;
		else p6Score.GetComponent<Text>().color = Color.red;

		if (playersScores[6] >= higherScore) p7Score.GetComponent<Text>().color = Color.green;
		else p7Score.GetComponent<Text>().color = Color.red;

		if (playersScores[7] >= higherScore) p8Score.GetComponent<Text>().color = Color.green;
		else p8Score.GetComponent<Text>().color = Color.red;

		if (playersScores[8] >= higherScore) p9Score.GetComponent<Text>().color = Color.green;
		else p9Score.GetComponent<Text>().color = Color.red;

		if (playersScores[9] >= higherScore) p10Score.GetComponent<Text>().color = Color.green;
		else p10Score.GetComponent<Text>().color = Color.red;
	}

	void adjustPlayerSeat()
    {
		int x = 0;
		int y = 0;
		int deletedPlayerNumber;
		int i = 0;

		switch (playerNumber)
		{
			case 1:
				p1Score.GetComponent<Text>().color = Color.black;
				break;
			case 2:
				p2Score.GetComponent<Text>().color = Color.black;
				break;
			case 3:
				p3Score.GetComponent<Text>().color = Color.black;
				break;
			case 4:
				p4Score.GetComponent<Text>().color = Color.black;
				break;
			case 5:
				p5Score.GetComponent<Text>().color = Color.black;
				break;
			case 6:
				p6Score.GetComponent<Text>().color = Color.black;
				break;
			case 7:
				p7Score.GetComponent<Text>().color = Color.black;
				break;
			case 8:
				p8Score.GetComponent<Text>().color = Color.black;
				break;
			case 9:
				p9Score.GetComponent<Text>().color = Color.black;
				break;
			case 10:
				p10Score.GetComponent<Text>().color = Color.black;
				break;
		}

		y = (prevNumber * (1 + prevNumber))/2; //Soma de Progressão Aritmética
		while(i < playersConnected.Length)
        {
			x = x + playersConnected[i].GetComponent<BasketballView>().seat; //Soma as seats dos outros players, menos a do próprio player
				i++;
		}
		x = x + (int)playerNumber; //Soma com a seat do próprio player
		deletedPlayerNumber = y - x;
			if (deletedPlayerNumber < playerNumber)
			{
				playerNumber--;
			}
		rt.sizeDelta = new Vector2(((numberOfPlayers + 1) / 2) * 260.0f, rt.sizeDelta.y);
		i = 0;
		while (i < 10)
		{
			if (numberOfPlayers == 1 || numberOfPlayers == 2)
			{
				pTexts[i].anchoredPosition = new Vector2((542.0f + 271.0f * (((i + 2) / 2) - 1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			else if (numberOfPlayers == 3 || numberOfPlayers == 4)
			{
				pTexts[i].anchoredPosition = new Vector2((406.5f + 271.0f * (((i + 2) / 2) - 1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			else if (numberOfPlayers == 5 || numberOfPlayers == 6)
			{
				pTexts[i].anchoredPosition = new Vector2((271.0f + 271.0f * (((i + 2) / 2) - 1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			else if (numberOfPlayers == 7 || numberOfPlayers == 8)
			{
				pTexts[i].anchoredPosition = new Vector2((135.5f + 271.0f * (((i + 2) / 2) - 1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			else if (numberOfPlayers == 9 || numberOfPlayers == 10)
			{
				pTexts[i].anchoredPosition = new Vector2((0.0f + 271.0f * (((i + 2) / 2) - 1) - 520.0f), pTexts[i].anchoredPosition.y);
			}
			i++;
		}
	}

	/*public void theScoreboard()
    {
		if(scoreboard.activeSelf)
        {
			scoreboard.SetActive(false);
        }
        else
        {
			scoreboard.SetActive(true);
		}
    }*/

	void getScore()
    {
		int i = 0;
		playersScores[playerNumber-1] = minhaBola.GetComponent<UiManager>().myScore;
		while (i < playersConnected.Length)
        {
			switch(playersConnected[i].GetComponent<BasketballView>().seat)
			{
				case 1:
					playersScores[0]= playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
				case 2:
					playersScores[1] = playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
				case 3:
					playersScores[2] = playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
				case 4:
					playersScores[3] = playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
				case 5:
					playersScores[4] = playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
				case 6:
					playersScores[5] = playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
				case 7:
					playersScores[6] = playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
				case 8:
					playersScores[7] = playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
				case 9:
					playersScores[8] = playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
				case 10:
					playersScores[9] = playersConnected[i].GetComponent<BasketballView>().theScore;
				break;
			}
			i++;
        }
		i = 9;
		while(i > (playersConnected.Length))
        {
			playersScores[i] = -1;
			i--;
        }
		p1Score.text = printScore(playersScores[0],1);
		p2Score.text = printScore(playersScores[1],2);
		p3Score.text = printScore(playersScores[2],3);
		p4Score.text = printScore(playersScores[3],4);
		p5Score.text = printScore(playersScores[4],5);
		p6Score.text = printScore(playersScores[5],6);
		p7Score.text = printScore(playersScores[6],7);
		p8Score.text = printScore(playersScores[7],8);
		p9Score.text = printScore(playersScores[8],9);
		p10Score.text = printScore(playersScores[9],10);
	}

	string printScore(int playerScore,int theSeat)
    {
		string scoreText;
		if(playerScore == -1)
        {
			return "";
        }
		else
        {
			if(theSeat == playerNumber)
            {
				scoreText = "You: " + playerScore.ToString();
				switch(theSeat)
                {
					case 1:
						p1Score.GetComponent<Text>().color = Color.blue;
						break;
					case 2:
						p2Score.GetComponent<Text>().color = Color.blue;
						break;
					case 3:
						p3Score.GetComponent<Text>().color = Color.blue;
						break;
					case 4:
						p4Score.GetComponent<Text>().color = Color.blue;
						break;
					case 5:
						p5Score.GetComponent<Text>().color = Color.blue;
						break;
					case 6:
						p6Score.GetComponent<Text>().color = Color.blue;
						break;
					case 7:
						p7Score.GetComponent<Text>().color = Color.blue;
						break;
					case 8:
						p8Score.GetComponent<Text>().color = Color.blue;
						break;
					case 9:
						p9Score.GetComponent<Text>().color = Color.blue;
						break;
					case 10:
						p10Score.GetComponent<Text>().color = Color.blue;
						break;
				}
			}
			else
            {
				scoreText = "Player" + theSeat.ToString();
				scoreText = scoreText + ": ";
				scoreText = scoreText + playerScore.ToString();
			}
			return scoreText;
        }
    }
}
