using UnityEngine;
using System.Collections;

public class loadOnClick : MonoBehaviour {

	public void LoadScene(string scene){
		Application.LoadLevel (scene);
	}
}
