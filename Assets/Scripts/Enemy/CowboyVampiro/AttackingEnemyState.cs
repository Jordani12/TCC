using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackingEnemyState : ICowboyState
{
    public SOEnemy enemyData; // ScriptableObject com dados do inimigo
    private GunControllerEnemy _gunController; // Controlador de armas para inimigos armados

    private NavMeshAgent enemyAgent; // Componente de navegação
    private Transform player; // Referência ao jogador

    public int maxHealth; // Vida máxima do inimigo
    public bool isItGun; // Flag que indica se é um inimigo armado

    // Chamado ao entrar neste estado
    public void EnterState(CowboyStateManager Enemy)
    {
        // Obtém referências necessárias
        enemyAgent = Enemy.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;

        // Obtém o controlador de arma se existir
        _gunController = Enemy.GetComponent<GunControllerEnemy>();

        // Configura propriedades baseadas nos dados do ScriptableObject
        if (enemyData != null)
        {
            maxHealth = enemyData.maxHealth;
            enemyAgent.stoppingDistance = enemyData.attackRange; // Distância de parada para ataque
            isItGun = enemyData.isItGun; // Define se é um inimigo armado
        }
    }

    // Chamado a cada frame enquanto neste estado
    public void UpdateState(CowboyStateManager Enemy)
    {
        // Verifica se está no alcance de ataque (com margem de +2 unidades)
        bool inRange = Vector3.Distance(Enemy.transform.position, player.position) <= (enemyAgent.stoppingDistance + 2);

        if (inRange && isItGun == true)
        {
            // Se está armado e no alcance: tenta atirar
            _gunController.AttemptShot();
        }
        else if (isItGun == false)
        {
            // Se não está armado, ajusta distância de parada para ataque corpo-a-corpo
            enemyAgent.stoppingDistance = 1;
            // Nota: Falta implementação do ataque corpo-a-corpo aqui
        }
        else if (!inRange)
        {
            // Se saiu do alcance, volta para estado de perseguição
            ExitState(Enemy);
        }
    }

    // Chamado ao sair deste estado
    public void ExitState(CowboyStateManager Enemy)
    {
        // Volta sempre para o estado de caminhada
        Enemy.SwitchState(Enemy.WalkingState);
    }

}
/*
 * Observação importante: Há uma possível melhoria não implementada - o estado não verifica a vida do inimigo (como faz o WalkingState) para transicionar para o estado de morte. Também falta a implementação completa do ataque corpo-a-corpo para inimigos desarmados.

*/
