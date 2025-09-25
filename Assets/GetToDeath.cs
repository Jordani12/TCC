using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GetToDeath : MonoBehaviour
{
    [HideInInspector] public bool can_destroy_all;
    [HideInInspector] public bool can_change;
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        can_change = false;
    }
    private void Update()
    {
        if (can_change) StartCoroutine(Cam_Anim_Time());
    }
    public IEnumerator Cam_Anim_Time()
    {
        can_change = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuGameOver");
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(2f);
        can_destroy_all = true;
        toggle_cursor();

        asyncLoad.allowSceneActivation = true;

        // Espera a ativação completa
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
    public IEnumerator Back_Menu(GameObject optCanvas, GameObject mainCanvas)
    {
        if (optCanvas == null || mainCanvas == null) yield return null;
        Destroy(optCanvas);
        Destroy(mainCanvas);
        if(player != null) Destroy(player); 

        can_change = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        asyncLoad.allowSceneActivation = false;

        can_destroy_all = true;
        toggle_cursor();

        asyncLoad.allowSceneActivation = true;

        // Espera a ativação completa
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    private void toggle_cursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
