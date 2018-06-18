using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mySceneManager : MonoBehaviour {
 
	[SerializeField]private Button restart;
	[SerializeField]private Button exit;

	public void Restart()
	{
		SceneManager.LoadScene ("Race"); 
	}

	public void Exit()
	{
		Application.Quit ();
	}
}
