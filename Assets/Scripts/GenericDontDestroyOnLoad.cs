using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericDontDestroyOnLoad : MonoBehaviour
{
    private GetToDeath death;
    private GameObject player;
    private MenuPause menu;
    void Start()
    {
        menu = FindObjectOfType<MenuPause>();
        death = FindObjectOfType<GetToDeath>();
        player = GameObject.FindWithTag("Player");
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (player == null && transform.name != "GameController" && transform.name != "CameraHolder")
        {
            Destroy(this.gameObject);
        }
        else if (death.can_destroy_all)
        {
            menu.pauseCanvas.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
