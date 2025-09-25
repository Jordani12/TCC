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
    private bool alr_activate = false;

    private void Start()
    {
        txt_move.SetActive(true);
        txt_shoot.SetActive(false);
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
        if (other.gameObject.tag == "Player" && !alr_activate) {
            CheckWhatWant();
            alr_activate = true;
        }
    }
    private void CheckWhatWant()
    {
        if (activeEnum == GenericActivateEnum.ativaShoot && gun != null) {
            gun.currentGun.passedTutorial = true;
            gun.currentGun.canShoot = true;
            StartCoroutine(TutorialController.Activate(txt_shoot));
        }
        else if (activeEnum == GenericActivateEnum.ativaJump) { }
        else if (activeEnum == GenericActivateEnum.ativaDash)
            player.canDash = true;
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