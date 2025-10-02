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

    // Vari�veis privadas
    private Transform _player; // Refer�ncia ao transform do jogador
    private float _nextAttackTime; // Pr�ximo momento que pode atacar
    private CowboyStateManager enemy; // Refer�ncia ao gerenciador de estados
    private SOEnemy enemyData; // Dados do inimigo (ScriptableObject)
    private bool _isAttacking; // Flag para controle de estado de ataque

    private void Awake()
    {
        // Inicializa as refer�ncias necess�rias
        _player = GameObject.Find("Player").transform;
        enemy = GetComponent<CowboyStateManager>();
        enemyData = GetComponent<SOEnemy>();
    }

    // Tenta executar um ataque corpo-a-corpo
    public void TryMeleeAttack()
    {
        // Verifica cooldown e se j� est� atacando
        if (Time.time < _nextAttackTime || _isAttacking)
            return;

        // Verifica se o jogador est� no alcance
        if (Vector3.Distance(transform.position, _player.position) <= _attackRange)
        {
            StartCoroutine(AttackRoutine()); // Inicia a rotina de ataque
        }
        else
        {
            // Se o jogador saiu do alcance, volta para estado de persegui��o
            enemy.SwitchState(enemy.WalkingState);
        }
    }

    // Corotina que controla a sequ�ncia de ataque
    private IEnumerator AttackRoutine()
    {
        _isAttacking = true; // Sinaliza que est� atacando

        yield return new WaitForSeconds(0.3f); // Tempo de antecipa��o antes do dano

        ApplyDamage(); // Aplica o dano

        yield return new WaitForSeconds(0.2f); // Tempo de recupera��o ap�s o ataque

        // Reseta os controles de ataque
        _nextAttackTime = Time.time + _attackCooldown;
        _isAttacking = false;
    }

    // Aplica o dano ao jogador
    private void ApplyDamage()
    {
        // Calcula dire��o e dist�ncia para o jogador
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // Dispara um raycast para verificar se acerta o jogador
        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer,
            out RaycastHit hit, _attackRange, _playerLayer))
        {
            /*
            // C�digo comentado que seria usado para aplicar dano
            if (hit.collider.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(_damage);
                Debug.Log("Dano aplicado ao jogador!");
            }
            */
        }
    }
}
/*Pontos de Aten��o:
O m�todo ApplyDamage() est� com a l�gica de dano comentada

N�o h� tratamento visual/feedback do ataque

A refer�ncia a enemyData (SOEnemy) n�o est� sendo utilizada

Busca por GameObject.Find("Player") pode ser ineficiente

Sugest�es de Melhoria:
Implementar o sistema de dano descomentando/modificando o trecho em ApplyDamage()

Adicionar anima��es ou efeitos visuais durante o ataque

Usar eventos para melhorar a modularidade do c�digo

Substituir GameObject.Find() por refer�ncia no Inspector ou outro m�todo mais eficiente

Adicionar valida��o se o jogador ainda est� vivo antes de atacar

*/