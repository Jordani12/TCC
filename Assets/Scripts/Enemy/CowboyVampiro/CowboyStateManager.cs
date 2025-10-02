using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyStateManager : MonoBehaviour
{
    [SerializeField] private SOEnemy enemyData; // Scriptable Object que contém os dados do inimigo

    // Interface que representa o estado atual do inimigo
    ICowboyState currentState;

    // Instâncias dos possíveis estados do inimigo:
    public WalkingEnemyState WalkingState = new WalkingEnemyState(); // Estado de caminhada
    public AttackingEnemyState AttackingState = new AttackingEnemyState(); // Estado de ataque
    public DeadingEnemyState DeadingState = new DeadingEnemyState(); // Estado de morte

    void Start()
    {
        // Passa os dados do inimigo para os estados que precisam deles
        WalkingState.enemyData = enemyData;
        AttackingState.enemyData = enemyData;

        // Define o estado inicial como WalkingState e executa sua lógica de entrada
        currentState = WalkingState;
        currentState.EnterState(this);
    }

    void Update()
    {
        // Chama a atualização do estado atual a cada frame
        currentState.UpdateState(this);
    }

    // Método para trocar entre os estados do inimigo
    public void SwitchState(ICowboyState newState)
    {
        currentState = newState; // Atualiza o estado atual
        currentState.EnterState(this); // Executa a lógica de entrada do novo estado
    }
}
