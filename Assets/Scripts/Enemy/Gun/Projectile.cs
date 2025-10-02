using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Pool;

// Classe que controla o comportamento dos proj�teis
public class Projectile : MonoBehaviour
{
    // Refer�ncias (nota: estas vari�veis n�o parecem ser utilizadas no c�digo atual)
    AttackingEnemyState AttackingState;
    SOEnemy enemyData;

    // Configura��es do proj�til
    public float speed = 25f;       // Velocidade de movimento
    public float lifetime = 2f;     // Tempo m�ximo de vida antes de auto-destruir

    // Vari�veis privadas de controle
    private Vector3 _direction;     // Dire��o do movimento
    private IObjectPool<GameObject> _pool;  // Refer�ncia ao pool de objetos
    private float _lifeTimer;       // Contador de tempo de vida
    private bool _released = false; // Flag para evitar libera��o m�ltipla


    // M�todo de inicializa��o chamado quando o proj�til � ativado
    public void Initialize(Vector3 direction, IObjectPool<GameObject> pool, SOEnemy data)
    {
        _direction = direction;    // Define a dire��o do movimento
        _pool = pool;              // Armazena a refer�ncia ao pool
        _lifeTimer = lifetime;     // Reseta o timer de vida
        enemyData = data;
        _released = false;

        // Ativa o objeto se n�o estiver ativo
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    // Atualiza��o a cada frame
    private void Update()
    {
        if (_released) return;
        // Movimenta o proj�til na dire��o definida
        transform.position += _direction * (speed * Time.deltaTime);

        // Contagem regressiva do tempo de vida
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            ReleaseToPool();  // Devolve ao pool quando o tempo acaba
        }
    }

    // Detecta colis�es com outros objetos
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

    // M�todo para devolver o proj�til ao pool
    private void ReleaseToPool()
    {
        if (_released || !gameObject.activeSelf) return;

        _released = true;

        // Verifica se o objeto est� ativo antes de devolver ao pool
        if (gameObject.activeSelf)
        {
            _pool.Release(gameObject);
        }
    }

    // Reset do proj�til quando desativado
    private void OnDisable()
    {
        _lifeTimer = lifetime;  // Prepara para pr�xima utiliza��o
    }
}
/*Poss�veis Melhorias:
Implementar l�gica de dano ao colidir com jogador

Adicionar efeitos visuais/sonoros ao colidir

Distinguir entre diferentes tipos de colis�o

Remover vari�veis n�o utilizadas (AttackingState, data)

Adicionar sistema de penetra��o em certos materiais
*/
