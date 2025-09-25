using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InspectorCollectible : MonoBehaviour
{
    // Variáveis de controle
    private bool onInspector;
    private GunController gunController;
    private PlayerCamera cam; 
    private PlayerStateManager player; 

    // Elementos de UI
    public TextMeshProUGUI textCollectMessage; //("[E] to interact")
    public TextMeshProUGUI textObjectName;
    public TextMeshProUGUI textObjectDescription;

    public Transform inspector;

    public List <string> alreadyCollectList;

    public SO_SaveInputs inputs_SO;

    //bottom
    public KeyCode interact_key;


    private void Start()
    {
        interact_key = inputs_SO.interact_in;
        // Inicializa as referências necessárias
        cam = Camera.main.gameObject.GetComponent<PlayerCamera>();
        player = GetComponent<PlayerStateManager>();
        gunController = GameObject.FindObjectOfType<GunController>();
    }

    public void AlternateBottom()
    {
        textObjectName.text = ""; // Limpa o nome
        textCollectMessage.text = $"[{interact_key}] to interact"; // Restaura a mensagem padrão
    }

    private bool ContainsWeapon(Collectible c)
    {
        if (alreadyCollectList.Contains(c.weaponName))
            return true;
        else{
            alreadyCollectList.Add(c.weaponName);
            return false;
        }
    }

    private void Update()
    {
        move_block();
        shot_block();

        var closestObject = SetColect();

        if (closestObject != null)
        {
            if (onInspector && Input.GetKeyDown(KeyCode.F))
            {
                CloseInspector(closestObject);
            }
        }
    }

    private void move_block()
    {
        cam.canMoveCam = !onInspector;
        player.canMove = !onInspector;
    }

    private void shot_block()
    {
        if (gunController.currentGun.canShoot && onInspector)
        {
            gunController.currentGun.canShoot = false;
        }
    }

    public GameObject SetColect()
    {
        var objects = GameObject.FindGameObjectsWithTag("collectibleObjects");

        if (objects.Length > 0)
        {
            var closestObject = Generic.GetClosestObjects(transform, objects);

            var distance = Vector3.Distance(transform.position, closestObject.transform.position);

            textCollectMessage.gameObject.SetActive(distance <= 2);

            //se o jogador estiver perto e pressionar E, abre a inspeção
            if (!onInspector && Input.GetKeyDown(interact_key) && distance <= 2)
            {
                var proprities = closestObject.GetComponent<Collectible>();
                bool alreadyCollected = ContainsWeapon(proprities);
                if (alreadyCollected){
                    CollectItem(closestObject);
                    Destroy(closestObject);
                }
                else{
                    textCollectMessage.text = "[F] to return";
                    OpenInspector(closestObject);
                }
            }
            return closestObject;
        }
        else
        {
            textCollectMessage.gameObject.SetActive(false);
        }
        return null;
    }

    public void CollectItem(GameObject obj)
    {
        var item = obj.GetComponent<Collectible>();

        if (item.type == Enums.ItemType.Ammo)
        {
            //se for munição, recarrega a arma correspondente
            ammo_logic(item);
        }
        else if (item.type == Enums.ItemType.Gun)
        {
            //se for uma arma, libera o uso (ex: shotgun)
            allow_equip_shotgun();
        }
        else if (item.type == Enums.ItemType.MedKit)
        {
            medkit_logic(item);
        }
    }

    private void allow_equip_shotgun()
    {
        gunController.currentGun.canEquipShotgun = true;
    }

    private void ammo_logic(Collectible item)
    {
        foreach (var gun in gunController.inventory)
        {
            if (item.weaponName == gun.getGun.gunName)
            {
                gun.getGun.ammunitionToReload += item.value;
                return;
            }
        }
    }

    private void medkit_logic(Collectible item)
    {
        Life life = gameObject.GetComponent<Life>();
        if (life != null)
        {
            var difLife = life.maxLife - life.actualLife;

            if (difLife > item.value)
                life.actualLife += item.value;
            else
                life.actualLife += difLife;
        }
        if(life.actualLife <= life.maxLife / 4) life.low_life_text(true);
        else life.low_life_text(false);
    }

    public void OpenInspector(GameObject obj)
    {
        onInspector = true; // Ativa o modo de inspeção

        DestroyObjectsOfInspector(); // Limpa objetos anteriores

        // Pega as propriedades do objeto (nome e descrição)
        var proprities = obj.GetComponent<Collectible>();
        textObjectName.text = proprities.objectName;
        textObjectDescription.text = proprities.objectDescription;

        // Instancia o objeto na posição de inspeção
        var collectible = Instantiate(obj, inspector.position, inspector.rotation);
        collectible.tag = "collectible"; // Marca para ser removido depois
        collectible.AddComponent<CollectibleController>(); // Adiciona controle (se necessário)
    }

    public void CloseInspector(GameObject closestObject)
    {
        CollectItem(closestObject); // Coleta o item
        DestroyObjectsOfInspector(); // Limpa objetos da inspeção
        textObjectDescription.text = ""; // Limpa a descrição
        textObjectName.text = ""; // Limpa o nome
        textCollectMessage.text = $"[{interact_key}] to interact"; // Restaura a mensagem padrão
        onInspector = false; // Desativa o modo de inspeção
        gunController.currentGun.canShoot = true; // Permite atirar novamente

        Destroy(closestObject); // Remove o objeto do cenário
    }

    /// <summary>
    /// Destroi todos os objetos marcados como "collectible" (usado para limpar a inspeção).
    /// </summary>
    private void DestroyObjectsOfInspector()
    {
        var objectsInScene = GameObject.FindGameObjectsWithTag("collectible");

        if (objectsInScene.Length > 0)
        {
            foreach (var obj in objectsInScene)
            {
                Destroy(obj); // Remove cada objeto instanciado
            }
        }
    }
}