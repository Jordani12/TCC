using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTutorialActive : MonoBehaviour
{
    public GenericActivateEnum activeEnum;
    private GunController gun;
    private PlayerStateManager player;
    [SerializeField] private GameObject txt_shoot;
    [SerializeField] private GameObject txt_move;
    [SerializeField] private GameObject txt_dash;

    private void Start()
    {
        txt_move.SetActive(true);
        txt_shoot.SetActive(false);
        txt_dash.SetActive(false);
        player = GameObject.FindObjectOfType<PlayerStateManager>();
        gun = GameObject.FindObjectOfType<GunController>();
    }
    private void Update()
    {
        if (player.isMoving && txt_move.activeInHierarchy)
                txt_move.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            CheckWhatWant();
        }
    }
    private void CheckWhatWant()
    {
        if (activeEnum == GenericActivateEnum.ativaShoot && gun != null)
            StartCoroutine(TutorialController.Activate(txt_shoot));

        else if (activeEnum == GenericActivateEnum.ativaJump) { }

        else if (activeEnum == GenericActivateEnum.ativaDash)
            StartCoroutine(TutorialController.Activate(txt_dash));
        
        else
            Debug.LogWarning("Valor não esperado no enum: " + activeEnum);
    }
}
public enum GenericActivateEnum
{
    ativaShoot,
    ativaDash,
    ativaJump
}