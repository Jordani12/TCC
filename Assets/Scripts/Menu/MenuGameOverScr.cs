using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOverScr : MonoBehaviour
{
    public void Reborn()
    {
        toggle_cursor();
        SceneManager.LoadScene("Parte1");
    }

    private void toggle_cursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Back_Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // Só funciona em builds
        #endif
    }
}
