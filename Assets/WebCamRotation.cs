using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class WebCamRotation : MonoBehaviour {

	public GUITexture myCameraTexture;
	private WebCamTexture webCameraTexture;

	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.Android) {
			rotateCamera ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.platform == RuntimePlatform.Android) {
			rotateCamera ();
		}
	}
	private void rotateCamera(){
		// Checks how many and which cameras are available on the device
		for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++) {
			if (!WebCamTexture.devices [cameraIndex].isFrontFacing) {
				webCameraTexture = new WebCamTexture (cameraIndex, Screen.width, Screen.height);

				if (Screen.orientation == ScreenOrientation.Portrait) {
					myCameraTexture.transform.localScale = new Vector3(-1,1,1);
				} else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
					myCameraTexture.transform.localScale = new Vector3(-1,1,1);
				} else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
					myCameraTexture.transform.localScale = new Vector3(1,-1,1);
				} else if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
					myCameraTexture.transform.localScale = new Vector3(1,-1,1);
				}
			}
		}
	}
}
