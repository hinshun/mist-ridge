using UnityEngine;
using System.Collections;

public class ScrollUV2 : MonoBehaviour {

	public float scrollSpeed = 2.5F;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float offset = Time.time * scrollSpeed;
		GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
	
	}
}
