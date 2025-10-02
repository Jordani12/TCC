    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Requer os componentes EnemyStateManager e SOEnemy no mesmo GameObject
[RequireComponent(typeof(CowboyStateManager))]
[RequireComponent(typeof(SOEnemy))]
public class AtkNear : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float _attackRange = 1.5f; // Alcance do ataque corpo-a-corpo
    [SerializeField] private float _attackCooldown = 1f; // Tempo entre ataques
    [SerializeField] private int _damage; // Quantidade de dano causado
    [SerializeField] private LayerMask _playerLayer; // LayerMask para detectar o jogador

    // Variáveis privadas
    private Transform _player; // Referência ao transform do jogador
    private float _nextAttackTime; // Próximo momento que pode atacar
    private CowboyStateManager enemy; // Referência ao gerenciador de estados
    private SOEnemy enemyData; // Dados do inimigo (ScriptableObject)
    private bool _isAttacking; // Flag para controle de estado de ataque

    private void Awake()
    {
        // Inicializa as referências necessárias
        _player = GameObject.Find("Player").transform;
        enemy = GetComponent<CowboyStateManager>();
        enemyData = GetComponent<SOEnemy>();
    }

    // Tenta executar um ataque corpo-a-corpo
    public void TryMeleeAttack()
    {
        // Verifica cooldown e se já está atacando
        if (Time.time < _nextAttackTime || _isAttacking)
            return;

        // Verifica se o jogador está no alcance
        if (Vector3.Distance(transform.position, _player.position) <= _attackRange)
        {
            StartCoroutine(AttackRoutine()); // Inicia a rotina de ataque
        }
        else
        {
            // Se o jogador saiu do alcance, volta para estado de perseguição
            enemy.SwitchState(enemy.WalkingState);
        }
    }

    // Corotina que controla a sequência de ataque
    private IEnumerator AttackRoutine()
    {
        _isAttacking = true; // Sinaliza que está atacando

        yield return new WaitForSeconds(0.3f); // Tempo de antecipação antes do dano

        ApplyDamage(); // Aplica o dano

        yield return new WaitForSeconds(0.2f); // Tempo de recuperação após o ataque

        // Reseta os controles de ataque
        _nextAttackTime = Time.time + _attackCooldown;
        _isAttacking = false;
    }

    // Aplica o dano ao jogador
    private void ApplyDamage()
    {
        // Calcula direção e distância para o jogador
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // Dispara um raycast para verificar se acerta o jogador
        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer,
            out RaycastHit hit, _attackRange, _playerLayer))
        {
            /*
            // Código comentado que seria usado para aplicar dano
            if (hit.collider.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(_damage);
                Debug.Log("Dano aplicado ao jogador!");
            }
            */
        }
    }
}
/*Pontos de Atenção:
O método ApplyDamage() está com a lógica de dano comentada

Não há tratamento visual/feedback do ataque

A referência a enemyData (SOEnemy) não está sendo utilizada

Busca por GameObject.Find("Player") pode ser ineficiente

Sugestões de Melhoria:
Implementar o sistema de dano descomentando/modificando o trecho em ApplyDamage()

Adicionar animações ou efeitos visuais durante o ataque

Usar eventos para melhorar a modularidade do código

Substituir GameObject.Find() por referência no Inspector ou outro método mais eficiente

Adicionar validação se o jogador ainda está vivo antes de atacar

*/