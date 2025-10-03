using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class GunController : MonoBehaviour
{
    public SO_SaveInputs inputs_SO;

    [Header("Tags and transforms")]
    public string wallTag, floorTag, sandTag, barrilTag, destructibleDoor;
    public GameObject wallHole;
    public GameObject wallImpact, sandImpact, enemyImpact;
    public Transform holes;
    [Header("Gun")]
    public Gun currentGun;
    public Gun[] inventory;
    [Header("Hud")]
    [SerializeField] private GameObject textNoAmmo;
    [SerializeField] private GameObject textLowAmmo;

    [Header("Test")]
    public GameObject _revolver;
    public GameObject _shotgun;
    public GameObject _melee;

    private Gun beforeThisGun;

    //scripts includeds
    private PlayerStateManager player;
    private PlayerCamera cam;
    private BarrelExplosion barril;
    private DoorDestructible doorDestructible;
    private Recoil recoil;
    private Main_Animation main_anim;

    //tempo de animações
    private float timeReloadAnimPistol = 1.3f;
    private float timeReloadAnimShotgun = 2f;
    private WaitForSeconds timeAnimationGunExitScreen = new WaitForSeconds(0.3f);

    private float innacurate_distance = 0.5f;
    private int times_shotgun_dispair = 0;

    //booleans
    public static bool isAiming = false;
    [HideInInspector]public bool reloading = false;

    private int[] damage_increase_per_bodypart = new int[2] {1, 2};

    private Aim _aim;
    private ReplacementGun _replace;

    public KeyCode recharging => inputs_SO.recharging;
    public MouseButton aim => inputs_SO.aim;
    public KeyCode change1 => inputs_SO.change1;
    public KeyCode change2 => inputs_SO.change2;
    public KeyCode change3 => inputs_SO.change3;

    private void Start(){ 
        foreach (var gun in inventory) 
            gun.model.SetActive(false);

        currentGun.model.SetActive(true);
        textLowAmmo.SetActive(false);
        textNoAmmo.SetActive(false);

        beforeThisGun = null;

        _revolver.SetActive(true);
        _melee.SetActive(false);
        _shotgun.SetActive(false);

        get_references();
    }
    private void get_references()
    {
        barril = GameObject.FindObjectOfType<BarrelExplosion>();
        doorDestructible = GameObject.FindObjectOfType<DoorDestructible>();
        player = GetComponent<PlayerStateManager>();
        cam = GameObject.FindObjectOfType<PlayerCamera>();
        recoil = GameObject.FindObjectOfType<Recoil>();
        main_anim = FindObjectOfType<Main_Animation>();
        _aim = GetComponent<Aim>();
        _replace = GetComponent<ReplacementGun>();
    }
    private void Update(){
        CheckBottom();
    }
    
    private bool isTrading_gun;
    private void CheckBottom(){
        bool _isAiming = Input.GetMouseButton((int)aim);

        cam.isAiming = _isAiming;

        if(currentGun.getGun.gunName != "Weapon") {//se não for uma arma branca(ela não consegue recarregar e mirar)
            //mira suave
            if (Input.GetMouseButtonDown((int)aim)) 
                Aim.canChangeMove = true;
            if (Input.GetMouseButton((int)aim) && !isAiming)
                _aim.start_aim_increase();
            else if (Input.GetMouseButtonUp((int)aim) && isAiming)
                _aim.releasing_aim();
            // movimentacao suave da arma (independente do zoom)
            if (isAiming && currentGun.getGun.gunName != "Shotgun")
                 _aim.aim_moving_weapon(player);
            else if(!isAiming && currentGun.getGun.gunName != "Shotgun")
                 _aim.aim_release_weapon(player);
        }
        if (!reloading){
            if (!isAiming && currentGun.isTimeToShoot)
            {
                //troca de arma
                if (Input.GetKeyDown(change1) && inventory.Length > 0 && currentGun.getGun.gunName != "Pistol" && !isTrading_gun)
                    _replace.replacement_pistol();
                else if (Input.GetKeyDown(change2) && inventory.Length > 1 && currentGun.getGun.gunName != "Shotgun" && !isTrading_gun)
                    _replace.replacement_shotgun();
                else if (Input.GetKeyDown(change3) && inventory.Length > 2 && currentGun.getGun.gunName != "Weapon" && !isTrading_gun)
                    _replace.replacement_melee();
                //recarregar municao
                else if (Input.GetKeyDown(recharging) && currentGun.getGun.gunName != "Weapon" && currentGun.isTimeToShoot)
                    StartCoroutine(Reload());
                    
            }
        }
    }

    public static IEnumerator aim_Increase(bool zoomIn, float targetFOV, float defaultFOV, float zoomSpeed)
    {
        float target = zoomIn ? targetFOV : defaultFOV;
        while (Mathf.Abs(Camera.main.fieldOfView - target) > 0.1f) {//verifica o valor para subtrair
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, target, zoomSpeed * Time.deltaTime);
            yield return null;
        }
        Camera.main.fieldOfView = target;//garante o valor exato
    }
    
    public IEnumerator Change(int index) {//troca de arma
        isTrading_gun = true;
        activateCanShoot();
        beforeThisGun = currentGun;
        yield return timeAnimationGunExitScreen;

        main_anim.Change_Gun_Anim();
        currentGun.model.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        currentGun = this.inventory[index];
        currentGun.model.SetActive(true);

        switch(currentGun.getGun.gunName){
            case"Weapon":
                _revolver.SetActive(false);
                _melee.SetActive(true);
                _shotgun.SetActive(false);
            break;
            case"Shotgun":
                _revolver.SetActive(false);
                _melee.SetActive(false);
                _shotgun.SetActive(true);
            break;
            case"Pistol":
                _revolver.SetActive(true);
                _melee.SetActive(false);
                _shotgun.SetActive(false);
            break;
            default:
                break;
        }

        AmmoText.check_low_ammo(currentGun, textLowAmmo);
        AmmoText.check_no_ammo(currentGun, textLowAmmo, textNoAmmo);
        melee_UI();
        //rodar a anima��o da troca pra nova arma

        Animation.Anim(currentGun.animator, currentGun.getGun.Idle);
        isTrading_gun = false;
    }

    
    private void activateCanShoot() {
        var gunVerify = GameObject.FindObjectOfType<Gun>();

        if (gunVerify != null && currentGun != null && currentGun.passedTutorial) {
            foreach (var guns in inventory) {
                guns.canShoot = true;
            }
        }
    }

    private void melee_UI(){
        if (currentGun.getGun.gunName == "Weapon")
        {
            currentGun.textAmmunition.gameObject.SetActive(false); currentGun.textBar.gameObject.SetActive(false); currentGun.textAmmunitionToReload.gameObject.SetActive(false);
        }
        else if (beforeThisGun.getGun.gunName == "Weapon")
        {
            currentGun.textAmmunition.gameObject.SetActive(true); currentGun.textBar.gameObject.SetActive(true); currentGun.textAmmunitionToReload.gameObject.SetActive(true);
        }
    }

    private IEnumerator Reload()
    {
        if (currentGun.getGun.ammunition == currentGun.getGun.maximumAmmo) yield break;

        else if (currentGun.getGun.ammunitionToReload == 0 && currentGun.getGun.gunName != "Weapon"){currentGun.noAmmoSource.Play(); yield break; }

        currentGun.reloadSource.Play();

        Animation.Anim(currentGun.animator, currentGun.getGun.Reload);

        reloading = true;

        if(currentGun.getGun.gunName == "Weapon") yield break;
        if (currentGun.getGun.gunName == "Pistol")
            yield return new WaitForSeconds(timeReloadAnimPistol);
        else if (currentGun.getGun.gunName == "Shotgun")
            yield return new WaitForSeconds(timeReloadAnimShotgun);

        increasesBullets();
        AmmoText.check_low_ammo(currentGun, textLowAmmo);
        AmmoText.check_no_ammo(currentGun, textLowAmmo, textNoAmmo);

        currentGun.StopReload();
    }

    

    private void increasesBullets(){
        var difBullets = currentGun.getGun.maximumAmmo - currentGun.getGun.ammunition;

        if (difBullets > currentGun.getGun.ammunitionToReload)
            currentGun.getGun.ammunition += currentGun.getGun.ammunitionToReload;
        else
            currentGun.getGun.ammunition += difBullets;

        currentGun.getGun.ammunitionToReload = Math.Clamp(currentGun.getGun.ammunitionToReload - difBullets, 0, currentGun.getGun.ammunitionToReload);
    }
    //esse de baixo � pra quando tiver animacao, ai o bool reloading vai servir com mais efetividade, pois vai acabar a animacao e vai direto pro StopReloading
    /*private void Reload(){
        if (currentGun.getGun.ammunition == currentGun.getGun.maximumAmmo){
            return;}
        if(currentGun.getGun.ammunitionToReload == 0){
            //sound sem disparo
            return;
        }
        reloading = true;
        //starta a anima��o
        var difBullets = currentGun.getGun.maximumAmmo - currentGun.getGun.ammunition;

        if (difBullets > currentGun.getGun.ammunitionToReload)
            currentGun.getGun.maximumAmmo += currentGun.getGun.ammunitionToReload;
        else
            currentGun.getGun.ammunition += difBullets;

        currentGun.getGun.ammunitionToReload = Math.Clamp(currentGun.getGun.ammunitionToReload -= difBullets, 0, currentGun.getGun.ammunitionToReload);
    }*/

    private void decreasesBullets(){
        currentGun.getGun.ammunition = Mathf.Clamp(currentGun.getGun.ammunition -= 1, 0, currentGun.getGun.maximumAmmo);
    }

    private void playEffects(){
        currentGun.fireSource.Play();//da play no a�dio do tiro
        if(currentGun.getGun.gunName != "Weapon")
            Instantiate(currentGun.muzzeEfect, currentGun.firePoint);//aparece o brilho do tiro
    }

    private float quant_Raycasts_Shotgun = 9;
    public void Shooting(){
        if(currentGun.getGun.ammunition == 0 && currentGun.getGun.gunName != "Weapon" && currentGun.isTimeToShoot){StartCoroutine(Reload()); return;}

        if (currentGun.getGun.gunName != "Weapon")
            decreasesBullets();

        if(recoil != null)
            recoil.RecoilFire();

        Animation.Anim(currentGun.animator, currentGun.getGun.Shoot);
        AmmoText.check_low_ammo(currentGun, textLowAmmo);
        AmmoText.check_no_ammo(currentGun, textLowAmmo, textNoAmmo);

        playEffects();

        RaycastHit target;
        GameObject impact = null;

        if (currentGun.getGun.gunName == "Shotgun") {
            for (int i = 2; i <= quant_Raycasts_Shotgun; i++) {
                times_shotgun_dispair = i;
                if (Physics.Raycast(Camera.main.transform.position, disperse_shoot(), out target, i)) {
                    give_damage(target, impact);
                    if (i >= quant_Raycasts_Shotgun)//colocar os buracos somente nos utilmos raycasts
                        wall_Floor_Holes(target, impact);
                }
                if (Physics.Raycast(Camera.main.transform.position, disperse_shoot(), out target, i)) {
                    give_damage(target, impact);
                    if (i >= quant_Raycasts_Shotgun)
                        wall_Floor_Holes(target, impact);
                }
            }
            times_shotgun_dispair = 0;
        }
        else {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target, currentGun.getGun.distance)) {
                give_damage(target, impact);
                wall_Floor_Holes(target, impact);
                //anima��o e tempo de anima��o
            }
        }
    }
   
    
    private void give_damage(RaycastHit target, GameObject impact)
    {
        enemyLife enLife = target.transform.GetComponentInParent<enemyLife>(); // Busca em todos os pais
        if (enLife == null) return;

        if (currentGun.getGun.gunName == "Weapon")
            StartCoroutine(desacelerate_time());
        impact = Instantiate(enemyImpact, target.point, Quaternion.LookRotation(target.normal));
        StartCoroutine(DestroyImpact(impact));

        string partName = target.transform.name.ToLower();
        
        if (partName.Contains("head"))
            enLife.TakeDamage(currentGun.getGun.damage * damage_increase_per_bodypart[1]);
        else if (partName.Contains("body")) 
            enLife.TakeDamage(currentGun.getGun.damage * damage_increase_per_bodypart[0]);
        else if (partName.Contains("leg")) 
            enLife.TakeDamage(currentGun.getGun.damage / 2);
    }

    private void wall_Floor_Holes(RaycastHit target, GameObject impact)
    {
        if (target.transform.tag == wallTag)
        {
            impact = Instantiate(wallImpact, target.point, Quaternion.LookRotation(target.normal));/*da para usar wallImpact.transform.rotation*/
            Instantiate(wallHole, target.point, Quaternion.LookRotation(target.normal), holes); StartCoroutine(DestroyImpact(impact));
        }
        else if (target.transform.tag == sandTag)
        {
            impact = Instantiate(sandImpact, target.point, sandImpact.transform.rotation); StartCoroutine(DestroyImpact(impact));
        }
        else if (target.transform.tag == barrilTag)
            barril.StartCoroutine("Explosion");
        else if (target.transform.tag == destructibleDoor)
            if (doorDestructible != null)
                doorDestructible.CountDamage();
    }

    private Vector3 disperse_shoot() {
        Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * times_shotgun_dispair;

        targetPos = new Vector3(
            targetPos.x + UnityEngine.Random.Range(-innacurate_distance, innacurate_distance),
            targetPos.y + UnityEngine.Random.Range(-innacurate_distance, innacurate_distance),
            targetPos.z + UnityEngine.Random.Range(-innacurate_distance, innacurate_distance)
            );

        Vector3 dir = targetPos - Camera.main.transform.position;
        return dir.normalized;
    }

    private IEnumerator DestroyImpact(GameObject impact){
        yield return new WaitForSeconds(2);
        Destroy(impact);
    }

    private IEnumerator desacelerate_time()
    {
        Time.timeScale = 0.5f;
        if(MenuPause.can_change_canvas)
            MenuPause.can_change_canvas = false;
        yield return new WaitForSecondsRealtime(1);
        MenuPause.can_change_canvas = true;
        Time.timeScale = 1f;
    }
}
/*
Melhorias: diminuir o TimeScale quando atacar com a faca.
*/