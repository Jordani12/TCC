using System.Collections;
using UnityEngine;
public class FinalizateState : MonoBehaviour, IPlayerState
{
    private float maxDistance = 7;
    //booleans
    [HideInInspector]public bool isFinalizating;
    //coroutine variables
    private WaitForSeconds tempoAnimacao = new WaitForSeconds(2);
    public void EnterState(PlayerStateManager player) {
        GunController gun = FindObjectOfType<GunController>();

        GameObject enemy = GetEnemy(gun);
        if (enemy == null || enemy.gameObject == null)return;

        if (gun.currentGun.getGun.gunName != "Weapon") return;

        StartCoroutine(Finalization(enemy));
        if (gun != null && gun.currentGun.getGun.gunName == "Weapon")
            Animation.Anim(gun.currentGun.animator, gun.currentGun.getGun.Reload);
    }
    public void UpdateState(PlayerStateManager player) { }
    public void FixedUpdateState(PlayerStateManager player) { }
    public void ExitState(PlayerStateManager player) { }
    private IEnumerator Finalization(GameObject enemyDestroy) {
        isFinalizating = true;
        yield return tempoAnimacao;
        Destroy(enemyDestroy);
        isFinalizating = false;
    }
    private GameObject GetEnemy(GunController gun) {
        if (Camera.main == null)return null;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance)) {
            enemyLife enLife = hit.transform.parent.gameObject.GetComponent<enemyLife>();
            if (enLife == null || enLife.actualLife >= enLife.maxLife / 4)return null;
            start_effect(gun, hit);
            return hit.transform.parent.gameObject;
        }
        return null;
    }

    private void start_effect(GunController gun, RaycastHit hit) {
        RaycastHit target = hit; GameObject impact = null;
        impact = Instantiate(gun.enemyImpact, target.point, Quaternion.LookRotation(target.normal));
        StartCoroutine(DestroyImpact(impact));
    }

    private IEnumerator DestroyImpact(GameObject impact) {
        yield return new WaitForSecondsRealtime(2);
        Destroy(impact);
    }
}