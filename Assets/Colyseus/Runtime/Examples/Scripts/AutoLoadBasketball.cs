using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoLoadBasketball : MonoBehaviour
{

	public GameObject botao;
	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(loadScene());
	}

	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator loadScene()
	{
		yield return new WaitForSeconds(1.5f);
		botao.GetComponent<Button>().onClick.Invoke();
	}
}
