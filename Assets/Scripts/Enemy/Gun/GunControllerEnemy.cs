using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(CowboyStateManager))]
public class GunControllerEnemy : MonoBehaviour
{
    public SOEnemy enemyData;
    // Configura��es de tiro
    public GameObject projectilePrefab; // Prefab do proj�til
    public Transform gunPoint; // Ponto de origem do disparo

    public float fireRate = 0.5f; // Intervalo entre tiros (em segundos)
    public Vector3 spread = new Vector3(0.06f, 0.06f, 0.06f); // Dispers�o dos tiros

    // Sistema de pool de objetos para otimiza��o
    private IObjectPool<GameObject> _pool;
    private float _nextFireTime; // Pr�ximo momento que pode atirar
    private Transform _player; // Refer�ncia ao jogador

    private void Awake()
    {
        // Inicializa��o das refer�ncias
        _player = GameObject.Find("Player").transform;
        projectilePrefab = GameObject.Find("Bala"); // Nota: Busca por nome pode ser ineficiente

        // Configura��o do Object Pool para proj�teis
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(projectilePrefab), // Como criar novos proj�teis
            actionOnGet: projectile => projectile.SetActive(true), // Ao pegar da pool
            actionOnRelease: projectile => projectile.SetActive(false), // Ao devolver � pool
            actionOnDestroy: projectile => Destroy(projectile), // Ao destruir excedentes
            collectionCheck: false, // Verificar se objeto j� est� na pool
            defaultCapacity: 10, // Capacidade inicial
            maxSize: 20 // Capacidade m�xima
        );
    }

    // Tenta realizar um disparo se condi��es forem atendidas
    public void AttemptShot()
    {
        // Verifica cooldown
        if (Time.time < _nextFireTime) return;

        // Calcula dire��o do disparo
        Vector3 direction = _GetDirection();

        // Verifica se h� linha de vis�o (raycast)
        if (Physics.Raycast(gunPoint.position, direction, out var hit))
        {
            if (direction != null) Shoot(direction); // Executa o disparo
            else Debug.LogWarning("Direction � null");
            _nextFireTime = Time.time + fireRate; // Atualiza cooldown
        }
    }

    // Calcula dire��o do disparo com dispers�o aleat�ria
    private Vector3 _GetDirection()
    {
        // Dire��o base (apontando para o jogador)
        Vector3 baseDirection = (_player.position - gunPoint.position).normalized;

        // Adiciona dispers�o aleat�ria
        baseDirection += new Vector3(
            Random.Range(-spread.x, spread.x),
            Random.Range(-spread.y, spread.y),
            Random.Range(-spread.z, spread.z)
        );

        // Normaliza para manter como vetor de dire��o
        baseDirection.Normalize();
        return baseDirection;
    }

    // Executa o disparo de fato
    public void Shoot(Vector3 direction)
    {
        // Obt�m proj�til da pool
        GameObject projectile = _pool.Get();

        // Posiciona e rotaciona o proj�til
        projectile.transform.SetPositionAndRotation(
            gunPoint.position,
            Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0) // Ajuste de rota��o
        );

        // Inicializa o proj�til com dire��o e refer�ncia � pool
        projectile.GetComponent<Projectile>().Initialize(direction, _pool, enemyData);
    }
}
/*
 * Poss�veis Melhorias:
Busca por GameObject.Find("Bala") pode ser substitu�da por refer�ncia direta no Inspector

Adicionar verifica��o se o hit do raycast � realmente o jogador

Implementar diferentes padr�es de disparo

Adicionar efeitos sonoros/visuais ao disparar
*/