using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryAnim : MonoBehaviour
{
    public bool onEntryAnimation { get; private set; } = true;

    public List<GameObject> objects = new List<GameObject>();

    public Animator anim;

    private PlayerStateManager player;

    private void Awake()
    {
        DesactivateObjects();

        player = FindObjectOfType<PlayerStateManager>();
        if (player != null) player.canMove = false;
    }

    private void Start()
    {
        anim.Play("entry_anim", 0, 0f);

        MenuPause.can_change_canvas = false;
    }

    private void DesactivateObjects() 
    {
        if (objects.Count == 0) return;

        objects.ForEach(obj => obj.SetActive(false));
    }

    public void ActivateObjects()
    {
        onEntryAnimation = false;

        if (objects.Count == 0) return;

        objects.ForEach(obj => obj.SetActive(true));

        if (player != null) player.canMove = true;

        MenuPause.can_change_canvas = true;
    }
}
