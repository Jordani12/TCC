using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Pool;

// Classe que controla o comportamento dos projéteis
public class Projectile : MonoBehaviour
{
    // Referências (nota: estas variáveis não parecem ser utilizadas no código atual)
    AttackingEnemyState AttackingState;
    SOEnemy enemyData;

    // Configurações do projétil
    public float speed = 25f;       // Velocidade de movimento
    public float lifetime = 2f;     // Tempo máximo de vida antes de auto-destruir

    // Variáveis privadas de controle
    private Vector3 _direction;     // Direção do movimento
    private IObjectPool<GameObject> _pool;  // Referência ao pool de objetos
    private float _lifeTimer;       // Contador de tempo de vida
    private bool _released = false; // Flag para evitar liberação múltipla


    // Método de inicialização chamado quando o projétil é ativado
    public void Initialize(Vector3 direction, IObjectPool<GameObject> pool, SOEnemy data)
    {
        _direction = direction;    // Define a direção do movimento
        _pool = pool;              // Armazena a referência ao pool
        _lifeTimer = lifetime;     // Reseta o timer de vida
        enemyData = data;
        _released = false;

        // Ativa o objeto se não estiver ativo
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    // Atualização a cada frame
    private void Update()
    {
        if (_released) return;
        // Movimenta o projétil na direção definida
        transform.position += _direction * (speed * Time.deltaTime);

        // Contagem regressiva do tempo de vida
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            ReleaseToPool();  // Devolve ao pool quando o tempo acaba
        }
    }

    // Detecta colisões com outros objetos
    private void OnCollisionEnter(Collision collision)
    {
        if (_released) return;

        if (collision.gameObject.tag == "Player")
        {
            var life = collision.gameObject.GetComponent<Life>();
            if (life != null && enemyData != null) life.TakeDamage(enemyData.damage);
        }
        ReleaseToPool();  // Devolve ao pool ao colidir com qualquer coisa
    }

    // Método para devolver o projétil ao pool
    private void ReleaseToPool()
    {
        if (_released || !gameObject.activeSelf) return;

        _released = true;

        // Verifica se o objeto está ativo antes de devolver ao pool
        if (gameObject.activeSelf)
        {
            _pool.Release(gameObject);
        }
    }

    // Reset do projétil quando desativado
    private void OnDisable()
    {
        _lifeTimer = lifetime;  // Prepara para próxima utilização
    }
}
/*Possíveis Melhorias:
Implementar lógica de dano ao colidir com jogador

Adicionar efeitos visuais/sonoros ao colidir

Distinguir entre diferentes tipos de colisão

Remover variáveis não utilizadas (AttackingState, data)

Adicionar sistema de penetração em certos materiais
*/
