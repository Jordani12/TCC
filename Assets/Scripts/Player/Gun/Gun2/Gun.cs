using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Referências
    private PlayerStateManager player; 
    private GunController gun;

    // Dados da arma (ScriptableObject)
    public GunCreator getGun;          

    [Header("Text")]
    public TextMeshProUGUI textAmmunition;        
    public TextMeshProUGUI textAmmunitionToReload; 
    public TextMeshProUGUI textBar;               

    [Header("Settings")]
    public GameObject model;          
    public Transform firePoint;       
    public GameObject muzzeEfect;     
    public AudioSource fireSource;    
    public AudioSource reloadSource;  
    public AudioSource noAmmoSource;

    [Header("Animation")]
    public Animator animator;

    public bool isTimeToShoot { get; set; } = false;
    public bool passedTutorial { get; set; } = false;  // Progressão no tutorial
    public bool canEquipShotgun { get; set; } = false; // Flag especial para desbloqueio de shotgun
    public bool canShoot { get; set; } = false;
    
    private void Awake() {
        // Inicializa referências
        gun = GameObject.FindObjectOfType<GunController>();
        passedTutorial = false;
    }

    private void OnEnable() {
        if (passedTutorial) {
            isTimeToShoot = true;
            canShoot = true;
        }
    }

    private void Start() {
        if (model == null) { model = this.gameObject; }

        // Configuração inicial
        passedTutorial = false;
        isTimeToShoot = false;
        canShoot = false;//desativar se tiver tutorial
        isTimeToShoot = true;
        
        getGun.ammunition = getGun.maximumAmmo;

        player = GameObject.FindObjectOfType<PlayerStateManager>();
    }

    private void Update() {
        CheckShoot(gun);
        elements_UI();
    }

    private void elements_UI() {
        // Atualiza elementos UI
        textAmmunition.text = getGun.ammunition.ToString("00");
        textAmmunitionToReload.text = getGun.ammunitionToReload.ToString("00");
    }

    private void CheckShoot(GunController gun){
        bool pressBottom = Input.GetMouseButtonDown(0); 
        
        if (!gun.reloading && pressBottom && canShoot && !player.isDashing) {
            if (isTimeToShoot) {
                if (getGun.ammunition != 0 || getGun.gunName == "Weapon")
                    StartCoroutine(ShootingReload());
                Shoot(gun);
            }
        }
    }

    private void Shoot(GunController gun){
        gun.Shooting();  
    }

    public void StopReload(){
        gun.reloading = false;
    }

    private IEnumerator ShootingReload(){
        isTimeToShoot = false; 
        yield return new WaitForSeconds(getGun.shootingTime);
        isTimeToShoot = true;  
    }
}