using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Start()
    {
        // Mantém este objeto ativo entre cenas
        DontDestroyOnLoad(this.gameObject);
        
        // Registra o método para ser chamado sempre que uma nova cena for carregada
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadScene)
    {
        // Verifica se a cena carregada é a tela de "Game Over"
        if (scene.name == "GameOver" || scene.name == "MainMenu")
        {
            // Destroi este objeto na cena de Game Over
            Destroy(this.gameObject);
            
            // Remove o registro do evento para evitar memory leaks
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        else
        {
            // Encontra a posição inicial do jogador na nova cena
            GameObject posicaoInicialObject = GameObject.FindGameObjectWithTag("posicaoInicialJogador");
            Transform posicaoInicialTransform = posicaoInicialObject.transform;
            Vector3 posicaoInicialPlayer = posicaoInicialTransform.position;
            
            // Reposiciona este objeto na posição inicial
            if(posicaoInicialPlayer != null || this.transform != null)
            this.transform.position = posicaoInicialPlayer;
        }
    }
}