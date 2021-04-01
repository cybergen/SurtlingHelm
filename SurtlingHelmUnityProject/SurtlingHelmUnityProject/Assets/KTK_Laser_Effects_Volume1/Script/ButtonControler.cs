//======================================
/*
@autor ktk.kumamoto
@date 2017.8.11 create
@note ButtonControler
*/
//======================================


using UnityEngine;
using System.Collections;

public class ButtonControler : MonoBehaviour {
	
	public GameObject[] EffectLaser_Set;
	public GameObject Robot;
	private bool isChecked = true;

	private GameObject ShotEffect;
	private int count_Effect2;

	private string EffLabel;	

	//EffectScale Slider
	public float hSliderValue = 1.0f;
	private Vector2 scrollViewVector = Vector2.zero;

	
	void Awake(){
		count_Effect2 = 0;
		EffLabel = EffectLaser_Set[count_Effect2].name;
	}
	
	void Start(){
		for(int i = 0; i < EffectLaser_Set.Length; i++)
		{
			EffectLaser_Set[i].SetActive (false);
		}
		ShotEffect = EffectLaser_Set[0];
	}
	
	void Update(){
		if (Input.GetMouseButtonDown(0)){
			
			ShotEffect.SetActive (true);
		}

		if (Input.GetMouseButtonUp(0)) {
			ShotEffect.SetActive (false);
		}

		if (Input.GetMouseButtonDown(1)){

			NextEffect ();
		}

		for(int i = 0; i < EffectLaser_Set.Length; i++)
		{
			EffectLaser_Set[i].GetComponent<LaserController>().OvarAll_Size =  hSliderValue;
		}


	}
	
	void OnGUI()
	{
		//Light Intencity Slider
		hSliderValue = GUI.HorizontalSlider(new Rect(525, 30, 160, 30), hSliderValue, 0.0F, 5.0F);
		GUI.Label(new Rect(700, 30,  200, 20), "Laser Scale: " + hSliderValue);

		GUI.Label(new Rect(200, 30, 150, 50), EffLabel);
		GUI.Label(new Rect(200, 0, 500, 25), "Mouse LeftClick laser firing!   Mouse RightClick NextEffect");

		// robot on/off
		Rect rect1 = new Rect(80, 80, 400, 30);
		isChecked = GUI.Toggle(rect1, isChecked, "Robot On/Off");
		if (isChecked) Robot.SetActive(true);
		else Robot.SetActive(false);

		//Effect Change
		if (GUI.Button (new Rect (25, 30, 160, 30), "NextEffect"))
		{
			NextEffect ();			
		}
	}

	void NextEffect(){
		EffectLaser_Set[count_Effect2].SetActive (false);

		count_Effect2 ++;

		if(count_Effect2 == EffectLaser_Set.Length){
			count_Effect2 = 0;
		}

		EffLabel = EffectLaser_Set[count_Effect2].name;

		ShotEffect = EffectLaser_Set[count_Effect2];
	}
}