using UnityEngine;
using System.Collections;

public class Gold : MonoBehaviour
{
    //referencias privadas
    private GameObject player;
    private GunController controller;

    private WaitForSeconds tempoAnimacao = new WaitForSeconds(0.5f);

    //calculos privados
    private int upgrade_per_gun;

    private void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<GunController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStateManager player = other.GetComponent<PlayerStateManager>();
        if(player != null){SeparateGuns();}
    }

    private void SeparateGuns()
    {
        Gun armaAtual = controller.currentGun;
        StartCoroutine(GunImprovement(armaAtual));
    }

    private IEnumerator GunImprovement(Gun armaAtual)
    {
        upgrade_per_gun = armaAtual.getGun.damage / 2;
        armaAtual.getGun.damage += upgrade_per_gun;

        upgrade_per_gun = armaAtual.getGun.maximumAmmo / 4;
        armaAtual.getGun.maximumAmmo += upgrade_per_gun;

        //animação
        yield return tempoAnimacao;

        Destroy(gameObject);
    }
}
