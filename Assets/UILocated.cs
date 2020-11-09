using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocated : MonoBehaviour {

	private int xRes;
	private int yRes;
	Canvas canvas;
	public Button[] Enter;
	void Start () {
		Resolution[] screenRes = Screen.resolutions;
		foreach(var res in screenRes)
        {
			xRes = res.height;
			yRes = res.width;
        }
		foreach (var but in Enter)
		{
			switch (but.name)
			{
				case "BwithG":
					break;
				case "BwithFB":
					break;
				case "Enter":
					break;
			}
		}
		//gameObject.transform 
		//canvas.scaleFactor = new 

	}
}
