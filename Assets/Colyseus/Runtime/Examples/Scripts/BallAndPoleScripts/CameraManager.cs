using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour 
{
	public bool follow = true;
	public GameObject ball;
	public GameObject basket;
	public float cameraDistance = 2;
	public float smoothTime = 0.5f;

	Vector3 vel;
	Vector3 baskertPos;
	Vector3 initialPos;
	Vector3 ballStartPosition;

	void Start ()
	{

		baskertPos = basket.transform.position;
		initialPos = transform.position;
		ballStartPosition = ball.transform.position ;
		cameraDistance = ball.transform.position.z - transform.position.z;
		follow = true;
		ChangeCameraAngle ();
	}
	

	void Update () {

		if (follow) {
			if (transform.position.z > (baskertPos.z - 4f)) {
				transform.position = initialPos;
			}	
			Vector3 target = ball.transform.position;
			if (ball.transform.position.y > this.transform.position.y) {
				target.y = ball.transform.position.y-0.4f;// + 0.61f;
			} else {
				if (ball.GetComponent<ShootAI>().isThrown) target.y = transform.position.y;// + 0.61f; //MODH
				else target.y = 1.3f; //MODH
			}
			target.x = ball.transform.position.x;
			target.z = ball.transform.position.z - (cameraDistance + 1);

			transform.position = Vector3.SmoothDamp (transform.position, target, ref vel, smoothTime);
		} else {
			Vector3 target1 = ball.transform.position;
			if (ball.transform.position.y > this.transform.position.y) {
				target1.y = ball.transform.position.y;// + 0.61f;
				target1.x = transform.position.x;
				target1.z = transform.position.z;
				transform.position = Vector3.SmoothDamp (transform.position, target1, ref vel, smoothTime);
			} else {
				target1.y = initialPos.y;// + 0.61f;
				target1.x = transform.position.x;
				target1.z = transform.position.z;
				transform.position = Vector3.SmoothDamp (transform.position, target1, ref vel, smoothTime);
			}
		}
		if (transform.position.z > baskertPos.z - 4f) {
			follow = false;
		}



	}

	public void ChangeCameraAngle()
	{
		//pos = x [-3 , 2] y [1.3 , 3.76]
		//rot = x [ 0 , 5] y [-4.81 , 5.83]

		float randPosX = Random.Range (-3f, 2f);
		float randPosY = Random.Range (1.3f, 3.76f);
		float randRotX = Random.Range (0f, 5f);
		float randRotY = Random.Range (-4.81f, 5.83f);

		//Vector3 position = new Vector3(randPosX,randPosY,transform.position.z);
		//Vector3 rotation = new Vector3 (randRotX,randRotY,transform.rotation.z);

		Vector3 position = new Vector3(0f, 1.0f, transform.position.z);
		Vector3 rotation = new Vector3 (0f,0f,0f);

		transform.position = position;
		transform.rotation = Quaternion.Euler (rotation);
		ResetState ();
	}
	public void ResetState()
	{
		transform.position = initialPos;
		follow = true;
		//print ("chal o parvez khan dia putra");
	}
}
