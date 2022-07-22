using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimator : MonoBehaviour {

    float offset = 0;
    Renderer rend;
    [Header("Time Dealy in seconds between flash")]
    public float blinkSpeed = 0.2f;
    void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine(Animate());

    }

  
    IEnumerator Animate()
    {
        while (true)
        {
            if (offset == 0)
                offset = 0.5f;
            else
                offset = 0;
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}
