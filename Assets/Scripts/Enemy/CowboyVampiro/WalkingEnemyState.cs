using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingEnemyState : ICowboyState
{
    public SOEnemy enemyData; // Dados configuráveis do inimigo

    // Componentes e referências
    private NavMeshAgent enemyAgent; // Componente de navegação
    private Transform player; // Referência ao jogador

    // Controle de atualização do caminho
    private float pathUpdateDeadLine; // Próximo momento para atualizar o caminho
    private float pathUpdateDelay = 0.5f; // Intervalo entre atualizações de caminho

    // Atributos do inimigo
    public int maxHealth; // Vida máxima
    public bool isItGun; // Se o inimigo é armado (controla comportamento de ataque)

    // Chamado quando entra neste estado
    public void EnterState(CowboyStateManager Enemy)
    {
        // Obtém referências necessárias
        enemyAgent = Enemy.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;

        // Configura propriedades baseadas nos dados do ScriptableObject
        if (enemyData != null)
        {
            maxHealth = enemyData.maxHealth;
            enemyAgent.speed = enemyData.movementSpeed;
            enemyAgent.stoppingDistance = enemyData.attackRange; // Distância para parar e atacar
            isItGun = enemyData.isItGun; // Define o tipo de ataque
        }
    }

    // Chamado a cada frame enquanto neste estado
    public void UpdateState(CowboyStateManager Enemy)
    {
        if (player != null)
        {
            // Verifica se está no alcance de ataque
            bool inRange = Vector3.Distance(Enemy.transform.position, player.position) <= enemyAgent.stoppingDistance;

            if (inRange && isItGun == true)
            {
                // Se está armado e no alcance: mira no jogador e muda para estado de ataque
                LookAtTarget();
                ExitState(Enemy);
            }
            else if (isItGun == false)
            {
                // Se não está armado, ajusta distância de parada e muda para estado de ataque
                enemyAgent.stoppingDistance = 1;
                ExitState(Enemy);
            }
            else if (!inRange)
            {
                // Se não está no alcance, continua atualizando o caminho
                UpdatePath();
            }

            // Verifica se a vida acabou
            if (maxHealth <= 0)
            {
                ExitState(Enemy);
            }
        }
    }

    // Faz o inimigo olhar suavemente para o jogador
    private void LookAtTarget()
    {
        Vector3 lookPos = player.position - enemyAgent.transform.position;
        lookPos.y = 0; // Ignora rotação no eixo Y
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        enemyAgent.transform.rotation = Quaternion.Slerp(enemyAgent.transform.rotation, rotation, 0.21f); // Interpolação suave
    }

    // Atualiza o caminho de navegação com um intervalo controlado
    private void UpdatePath()
    {
        if (Time.time >= pathUpdateDeadLine)
        {
            pathUpdateDeadLine = Time.time + pathUpdateDelay; // Agenda próxima atualização
            enemyAgent.SetDestination(player.position); // Define novo destino
        }
    }

    // Chamado ao sair deste estado
    public void ExitState(CowboyStateManager Enemy)
    {
        // Decide para qual estado transicionar baseado na vida
        if (maxHealth > 0)
        {
            Enemy.SwitchState(Enemy.AttackingState); // Vai para ataque se tem vida
        }
        else if (maxHealth <= 0)
        {
            Enemy.SwitchState(Enemy.DeadingState); // Vai para morte se vida zerou
        }
    }
}
