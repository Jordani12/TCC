using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingEnemyState : ICowboyState
{
    public SOEnemy enemyData; // Dados configur�veis do inimigo

    // Componentes e refer�ncias
    private NavMeshAgent enemyAgent; // Componente de navega��o
    private Transform player; // Refer�ncia ao jogador

    // Controle de atualiza��o do caminho
    private float pathUpdateDeadLine; // Pr�ximo momento para atualizar o caminho
    private float pathUpdateDelay = 0.5f; // Intervalo entre atualiza��es de caminho

    // Atributos do inimigo
    public int maxHealth; // Vida m�xima
    public bool isItGun; // Se o inimigo � armado (controla comportamento de ataque)

    // Chamado quando entra neste estado
    public void EnterState(CowboyStateManager Enemy)
    {
        // Obt�m refer�ncias necess�rias
        enemyAgent = Enemy.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;

        // Configura propriedades baseadas nos dados do ScriptableObject
        if (enemyData != null)
        {
            maxHealth = enemyData.maxHealth;
            enemyAgent.speed = enemyData.movementSpeed;
            enemyAgent.stoppingDistance = enemyData.attackRange; // Dist�ncia para parar e atacar
            isItGun = enemyData.isItGun; // Define o tipo de ataque
        }
    }

    // Chamado a cada frame enquanto neste estado
    public void UpdateState(CowboyStateManager Enemy)
    {
        if (player != null)
        {
            // Verifica se est� no alcance de ataque
            bool inRange = Vector3.Distance(Enemy.transform.position, player.position) <= enemyAgent.stoppingDistance;

            if (inRange && isItGun == true)
            {
                // Se est� armado e no alcance: mira no jogador e muda para estado de ataque
                LookAtTarget();
                ExitState(Enemy);
            }
            else if (isItGun == false)
            {
                // Se n�o est� armado, ajusta dist�ncia de parada e muda para estado de ataque
                enemyAgent.stoppingDistance = 1;
                ExitState(Enemy);
            }
            else if (!inRange)
            {
                // Se n�o est� no alcance, continua atualizando o caminho
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
        lookPos.y = 0; // Ignora rota��o no eixo Y
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        enemyAgent.transform.rotation = Quaternion.Slerp(enemyAgent.transform.rotation, rotation, 0.21f); // Interpola��o suave
    }

    // Atualiza o caminho de navega��o com um intervalo controlado
    private void UpdatePath()
    {
        if (Time.time >= pathUpdateDeadLine)
        {
            pathUpdateDeadLine = Time.time + pathUpdateDelay; // Agenda pr�xima atualiza��o
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
