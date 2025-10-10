using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    [SerializeField] private WhichAnim whichAnim;

    public List<GameObject> objects = new List<GameObject>();

    [Header("Animator")]
    public Animator anim;
    public bool onAnimation { get; private set; } = false;

    private PlayerStateManager player;

    private void Awake()
    {
        DesactivateObjects();

        player = FindObjectOfType<PlayerStateManager>();
        if (player != null) player.canMove = false;
    }

    private void Start()
    {
        if (whichAnim == WhichAnim.None)
        {
            onAnimation = true;
            anim.Play("entry_anim", 0, 0f);
        }

        MenuPause.can_change_canvas = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onAnimation = true;
            DesactivateObjects();
            CheckTypeAnim();
        }
    }

    private void CheckTypeAnim()
    {
        if(whichAnim == WhichAnim.DashTutAnim)
        {
            anim.Play("dash_anim", 0, 0f);
        }
    }

    private void DesactivateObjects() 
    {
        if (objects.Count == 0) return;

        objects.ForEach(obj => obj.SetActive(false));
    }

    public void ActivateObjects()
    {
        onAnimation = false;

        if (objects.Count == 0) return;

        objects.ForEach(obj => obj.SetActive(true));

        if (player != null) player.canMove = true;

        MenuPause.can_change_canvas = true;
    }
}

public enum WhichAnim
{
    None,
    DashTutAnim
}