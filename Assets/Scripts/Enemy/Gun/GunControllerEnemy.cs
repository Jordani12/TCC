using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(CowboyStateManager))]
public class GunControllerEnemy : MonoBehaviour
{
    public SOEnemy enemyData;
    // Configurações de tiro
    public GameObject projectilePrefab; // Prefab do projétil
    public Transform gunPoint; // Ponto de origem do disparo

    public float fireRate = 0.5f; // Intervalo entre tiros (em segundos)
    public Vector3 spread = new Vector3(0.06f, 0.06f, 0.06f); // Dispersão dos tiros

    // Sistema de pool de objetos para otimização
    private IObjectPool<GameObject> _pool;
    private float _nextFireTime; // Próximo momento que pode atirar
    private Transform _player; // Referência ao jogador

    private void Awake()
    {
        // Inicialização das referências
        _player = GameObject.Find("Player").transform;
        projectilePrefab = GameObject.Find("Bala"); // Nota: Busca por nome pode ser ineficiente

        // Configuração do Object Pool para projéteis
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(projectilePrefab), // Como criar novos projéteis
            actionOnGet: projectile => projectile.SetActive(true), // Ao pegar da pool
            actionOnRelease: projectile => projectile.SetActive(false), // Ao devolver à pool
            actionOnDestroy: projectile => Destroy(projectile), // Ao destruir excedentes
            collectionCheck: false, // Verificar se objeto já está na pool
            defaultCapacity: 10, // Capacidade inicial
            maxSize: 20 // Capacidade máxima
        );
    }

    // Tenta realizar um disparo se condições forem atendidas
    public void AttemptShot()
    {
        // Verifica cooldown
        if (Time.time < _nextFireTime) return;

        // Calcula direção do disparo
        Vector3 direction = _GetDirection();

        // Verifica se há linha de visão (raycast)
        if (Physics.Raycast(gunPoint.position, direction, out var hit))
        {
            if (direction != null) Shoot(direction); // Executa o disparo
            else Debug.LogWarning("Direction é null");
            _nextFireTime = Time.time + fireRate; // Atualiza cooldown
        }
    }

    // Calcula direção do disparo com dispersão aleatória
    private Vector3 _GetDirection()
    {
        // Direção base (apontando para o jogador)
        Vector3 baseDirection = (_player.position - gunPoint.position).normalized;

        // Adiciona dispersão aleatória
        baseDirection += new Vector3(
            Random.Range(-spread.x, spread.x),
            Random.Range(-spread.y, spread.y),
            Random.Range(-spread.z, spread.z)
        );

        // Normaliza para manter como vetor de direção
        baseDirection.Normalize();
        return baseDirection;
    }

    // Executa o disparo de fato
    public void Shoot(Vector3 direction)
    {
        // Obtém projétil da pool
        GameObject projectile = _pool.Get();

        // Posiciona e rotaciona o projétil
        projectile.transform.SetPositionAndRotation(
            gunPoint.position,
            Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0) // Ajuste de rotação
        );

        // Inicializa o projétil com direção e referência à pool
        projectile.GetComponent<Projectile>().Initialize(direction, _pool, enemyData);
    }
}
/*
 * Possíveis Melhorias:
Busca por GameObject.Find("Bala") pode ser substituída por referência direta no Inspector

Adicionar verificação se o hit do raycast é realmente o jogador

Implementar diferentes padrões de disparo

Adicionar efeitos sonoros/visuais ao disparar
*/