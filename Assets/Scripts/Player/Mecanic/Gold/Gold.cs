using UnityEngine;
using System.Collections;

public class Gold : MonoBehaviour
{
    private GunController controller;

    private void Awake()
    {
        controller = FindObjectOfType<GunController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStateManager player = other.GetComponent<PlayerStateManager>();
        if(player != null) { SeparateGuns(); }
    }

    private void SeparateGuns()
    {
        Gun armaAtual = controller.currentGun;
        StartCoroutine(GunImprovement(armaAtual));
    }

    private IEnumerator GunImprovement(Gun armaAtual)
    {
        int upgrade_per_gun = armaAtual.getGun.damage / 2;
        armaAtual.getGun.damage += upgrade_per_gun;

        int upgrade_per_gun2 = armaAtual.getGun.maximumAmmo / 4;
        armaAtual.getGun.maximumAmmo += upgrade_per_gun2;

        //animação
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}
