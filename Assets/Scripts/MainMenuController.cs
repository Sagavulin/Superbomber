using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void LoadGame()
    {
	    // Delete all stored player data each time game starts
	    PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Level01");
    }
}
