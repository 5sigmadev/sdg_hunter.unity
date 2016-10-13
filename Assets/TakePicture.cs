using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class TakePicture : MonoBehaviour {

	private static AndroidJavaObject _admobPlugin;
	string photoPath;
	string myDefaultLocation;
	bool inCourtine = true, saving = true;

	// Taking screenshot
	public void takePicture () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		Debug.Log("SDG [Unity] Starting...");
		StartCoroutine(CaptureScreen());
		StartCoroutine(MoveAndUpdateFileInGallery());
		StartCoroutine (FinishActivity());
	}

	public IEnumerator CaptureScreen()
	{	
		// Wait till the last possible moment before screen rendering to hide the UI
		yield return null;
		GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

		// Wait for screen rendering to complete
		yield return new WaitForEndOfFrame();
		int rand = new System.Random().Next();
		photoPath = string.Format("JPEG{0}_{1}.jpeg", 
									rand,
									System.DateTime.Now.Ticks);
		
		Debug.Log (photoPath);
		// Take screenshot
		Application.CaptureScreenshot(photoPath);
		Debug.Log ("SDG [Unity] Screenshot taken");
		// Show UI after we're done
		GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
		inCourtine = false;
	}

	public IEnumerator MoveAndUpdateFileInGallery()
	{	
		while(inCourtine)       
			yield return new WaitForSeconds(0.1f);
		
		if (Application.platform == RuntimePlatform.Android){
			myDefaultLocation = string.Format ("{0}/{1}",
				                           Application.persistentDataPath,
				                           photoPath);
			//REFRESHING THE ANDROID PHONE PHOTO GALLERY IS BEGUN
			AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");        
			AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");        
			AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", 
				new object[2]{
					"android.intent.action.MEDIA_SCANNER_SCAN_FILE", 
					classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myDefaultLocation)
				}
			);        
			objActivity.Call ("sendBroadcast", objIntent);
			//REFRESHING THE ANDROID PHONE PHOTO GALLERY IS COMPLETE
			Debug.Log ("SDG [Unity] Default path file broadcast sent");
		}

		saving = false;
	}

	public IEnumerator FinishActivity()
	{
		while(saving)       
			yield return new WaitForSeconds(0.1f);
		if (Application.platform == RuntimePlatform.Android){
			AndroidJavaClass jc = new AndroidJavaClass("com.fivesigmagames.sdghunter.view.ar.UnityPlayerActivity");  
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("instance");
			jo.Call("setNewFilePath", myDefaultLocation);
			Debug.Log ("SDG [Unity] setNewFilePath called");
			jo.Call("finishActivity");
			Debug.Log ("SDG [Unity] finishActivity callerd");
		}
		Debug.Log ("SDG [Unity] Finished...");
	}
}
