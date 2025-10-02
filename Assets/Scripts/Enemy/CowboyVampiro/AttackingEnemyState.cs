using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackingEnemyState : ICowboyState
{
    public SOEnemy enemyData; // ScriptableObject com dados do inimigo
    private GunControllerEnemy _gunController; // Controlador de armas para inimigos armados

    private NavMeshAgent enemyAgent; // Componente de navega��o
    private Transform player; // Refer�ncia ao jogador

    public int maxHealth; // Vida m�xima do inimigo
    public bool isItGun; // Flag que indica se � um inimigo armado

    // Chamado ao entrar neste estado
    public void EnterState(CowboyStateManager Enemy)
    {
        // Obt�m refer�ncias necess�rias
        enemyAgent = Enemy.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;

        // Obt�m o controlador de arma se existir
        _gunController = Enemy.GetComponent<GunControllerEnemy>();

        // Configura propriedades baseadas nos dados do ScriptableObject
        if (enemyData != null)
        {
            maxHealth = enemyData.maxHealth;
            enemyAgent.stoppingDistance = enemyData.attackRange; // Dist�ncia de parada para ataque
            isItGun = enemyData.isItGun; // Define se � um inimigo armado
        }
    }

    // Chamado a cada frame enquanto neste estado
    public void UpdateState(CowboyStateManager Enemy)
    {
        // Verifica se est� no alcance de ataque (com margem de +2 unidades)
        bool inRange = Vector3.Distance(Enemy.transform.position, player.position) <= (enemyAgent.stoppingDistance + 2);

        if (inRange && isItGun == true)
        {
            // Se est� armado e no alcance: tenta atirar
            _gunController.AttemptShot();
        }
        else if (isItGun == false)
        {
            // Se n�o est� armado, ajusta dist�ncia de parada para ataque corpo-a-corpo
            enemyAgent.stoppingDistance = 1;
            // Nota: Falta implementa��o do ataque corpo-a-corpo aqui
        }
        else if (!inRange)
        {
            // Se saiu do alcance, volta para estado de persegui��o
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
 * Observa��o importante: H� uma poss�vel melhoria n�o implementada - o estado n�o verifica a vida do inimigo (como faz o WalkingState) para transicionar para o estado de morte. Tamb�m falta a implementa��o completa do ataque corpo-a-corpo para inimigos desarmados.

*/
