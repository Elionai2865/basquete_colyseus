using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public enum Direction
{
	Left = 0,
	Right,
	Both
}

public class ShootAI : Shoot
{


	public static Action<float> EventChangeCurveRange = delegate {
	};

	private List<Vector3> _ballPath;
	public int _index;
	private bool _isShootByAI = false;
	//	private MyMultiSelection _multiSelection;
	public float _remainTimeToNextPointZ;
	private float yVelocityTarget;
	private float angleZY;
	private bool _isDown;

	public bool _isTestShootAI = false;

	public float _maxLeft = 0f;
	public float _maxRight = 0f;

	public bool djamba = true;
	
	
	
	
	/*************** Debug ****************/
	public float _curveLevel = 0;
	public float _difficulty = 0.5f;
	public bool willBeShootByUser = true;
	public Direction _shootDirection = Direction.Left;
	private List<Vector3> _debugPath = new List<Vector3> ();

	public GameObject gameover;
	void OnDrawGizmos ()
	{
		if (_ballPath != null && _ballPath.Count > 0) {
			//iTween.DrawLineGizmos(_debugPath.ToArray(), Color.red);
		}
	}
	
	/*******************************/
	bool UC;
	public bool BasSucess;
	int scores;
	float time;

	void OnTriggerEnter(Collider col){
		//		print (BasSucess.ToString());

		if (col.gameObject.CompareTag("outOfBounds"))
		{
			StopCoroutine("resetCoroutine");
			reset();
			enableTouch();
			isThrown = false;
			if (!isTestBall)
			{
				UiM.xMark(); //MODH
				tries++;
			}
			GameObject.Find("Main Camera").GetComponent<CameraManager>().ResetState();
		}

		if (col.name == "UC" && !isTestBall)
		{
			UC = true;
		}
		if (col.name == "DC" && UC) {
			BasSucess = true;
			scores++;
			Debug.Log("Cesta");
			base.UiM.UpdateScores (scores);
		}
	}

	protected override void Awake ()
	{
		base.Awake ();
		gameover.SetActive (false);
		scores = 0;
	}

	void Gameover(){
		//gameover.SetActive (true);
		//scores = 0;
		//time = 30.0f;

	}

	public void replay(){
		tries = 0;
		base.UiM.UpdateScores (0);
		if (SceneManager.GetActiveScene ().name == "TimeInterval") {
			base.UiM.reduceSlider (time);
		}
		gameover.SetActive (false);
		reset ();	
	}

	protected override void Start ()
	{
		base.Start ();

	}

	protected override void Update ()
	{
			if (willBeShootByUser && tries < maxTries)
			{
				base.Update();
			}
			else
			{
				Gameover();
			}
	}

	public override void goalEvent (bool isGoal)
	{
		base.goalEvent (isGoal);
	}

	public void shoot ()
	{
		shoot (_shootDirection, _curveLevel, _difficulty);
	}

	public void shoot (Direction shootDirection, float curveLevel, float difficulty)
	{
//		Debug.Log("AI shoot");
		//EventShoot();

		//_isShootByAI = true;

		_ballPath.Clear ();

		//	_ballPath = createPath(shootDirection, curveLevel, difficulty, _cachedTrans.position); 
		_index = _ballPath.Count - 1;			// di nguoc tu cuoi' mang~ ve dau` mang~


	}

	public float yEnd = 2.36f;
	public float yMiddle = 2.8f;
	public float xEnd = 3.5f;

	public float yExtra = 0.5f;
	//public Interpolate.EaseType easeType = Interpolate.EaseType.EaseInOutCubic;
	public int slideTest = 6;

	public AnimationCurve _animationCurve;

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Finish") {
			Destroy (col.gameObject);
		}
	}

	public override void reset (float x, float z)
	{
//		Debug.Log("Reser Shoot AI : x = " + x + "\t z = " + z);
		base.reset (x, z);

		UC = false;
		BasSucess = false;
		_isShootByAI = false;
	}

	public override void reset ()
	{
//		Debug.Log("Reser Shoot AI");
		base.reset ();
		_isShootByAI = false;
	}



	[ System.Serializable]
	public class DataShootAI
	{
		public float _distance;

		public float _yMid_Min;
		public float _yEnd_Min_When_Mid_Min;
		public float _yEnd_Max_When_Mid_Min;

		public float _yMid_Max;
		public float _yEnd_Min_When_Mid_Max;
		public float _yEnd_Max_When_Mid_Max;

		public DataShootAI (float distance, float yMid_Min, float yEnd_Min_When_Mid_Min, float yEnd_Max_When_Mid_Min, float yMid_Max, float yEnd_Min_When_Mid_Max, float yEnd_Max_When_Mid_Max)
		{

		}
	}

	private DataShootAI[] dataShootAI = new DataShootAI[] {
		
		
		new DataShootAI (16.5f, 2.6f, 2.1f, 2.4f, 4.3f, 0.145f, 0.76f)
		, new DataShootAI (19f, 2.8f, 2.2f, 2.4f, 4.3f, 0.145f, 1.5f)
		, new DataShootAI (21f, 2.8f, 2.2f, 2.4f, 4.3f, 0.145f, 1.8f)
		, new DataShootAI (23f, 2.8f, 2.3f, 2.55f, 4.3f, 0.145f, 2.2f)
		, new DataShootAI (27f, 3f, 1.8f, 2.6f, 4.3f, 0.145f, 2.6f)
		, new DataShootAI (35f, 3.2f, 1.6f, 2.8f, 4.3f, 0.145f, 2.8f)
	};
}
