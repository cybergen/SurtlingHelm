//======================================
/*
@autor ktk.kumamoto
@date 2015.3.6 create
@note UvScroll
*/
//======================================

using UnityEngine;
using System.Collections;

public class UvScroll : MonoBehaviour {
	
	public float scrollSpeed = 0.5F;
	private Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
	}
	void Update() {
		float offset = Time.time * - scrollSpeed;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
	}
}