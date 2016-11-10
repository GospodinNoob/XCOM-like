using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "New Game"))
        {
            PlayerPrefs.SetInt("NewGame", 1);
            Application.LoadLevel("PlaningScene");
        }
    }
}
