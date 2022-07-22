using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {
	public Text Scores;
	public Slider slider;
	public int myScore = 0;
	public GameObject niceThrow;

	public ShootAI shootDaMinhaBola;

	private bool justScored = false;

	[Header("Ball Marks")]
	public GameObject ballMark1;
	public GameObject ballMark2;
	public GameObject ballMark3;
	public GameObject ballMark4;
	public GameObject ballMark5;

	[Header("Empty Marks")]
	public GameObject emptyMark1;
	public GameObject emptyMark2;
	public GameObject emptyMark3;
	public GameObject emptyMark4;
	public GameObject emptyMark5;

	[Header("X Marks")]
	public GameObject xMark1;
	public GameObject xMark2;
	public GameObject xMark3;
	public GameObject xMark4;
	public GameObject xMark5;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateScores(int scores){
		switch(shootDaMinhaBola.tries)
        {
			case 0:
				ballMark1.SetActive(true);
				emptyMark1.SetActive(false);
				break;
			case 1:
				ballMark2.SetActive(true);
				emptyMark2.SetActive(false);
				break;
			case 2:
				ballMark3.SetActive(true);
				emptyMark3.SetActive(false);
				break;
			case 3:
				ballMark4.SetActive(true);
				emptyMark4.SetActive(false);
				break;
			case 4:
				ballMark5.SetActive(true);
				emptyMark5.SetActive(false);
				break;
		}
		Scores.text = "Points : " + scores.ToString ();
		justScored = true;
		if(myScore!=scores)
        {
			StartCoroutine(enableScore());
		}
		myScore = scores;
	}

	public void xMark()
    {
		if (!justScored)
		{
			switch (shootDaMinhaBola.tries)
			{
				case 0:
					xMark1.SetActive(true);
					emptyMark1.SetActive(false);
					break;
				case 1:
					xMark2.SetActive(true);
					emptyMark2.SetActive(false);
					break;
				case 2:
					xMark3.SetActive(true);
					emptyMark3.SetActive(false);
					break;
				case 3:
					xMark4.SetActive(true);
					emptyMark4.SetActive(false);
					break;
				case 4:
					xMark5.SetActive(true);
					emptyMark5.SetActive(false);
					break;
			}
		}
		justScored = false;
	}

	public void reduceSlider(float value){
		slider.value = value;
	}

	IEnumerator enableScore()
	{
		niceThrow.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		niceThrow.SetActive(false);
	}
}
