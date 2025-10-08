using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string nameScene;

    [Header("Fade Animator")]
    [SerializeField] private Animator fade_animator;    

    private void Start()
    {
        StartCoroutine(black_blind_fade(false));
    }

    private void OnTriggerEnter(Collider other){//check se esta perto da porta
        if(other.gameObject.tag == "Player")
        {
            Logic();
        }
    }

    void Logic()
    {
        if (this.nameScene != null) { StartCoroutine(black_blind_fade(true)); }
    }

    private IEnumerator black_blind_fade(bool blink)
    {
        if (blink)
            fade_animator.Play("fade_painel_onLevelPass", 0, 0f);
        else
            fade_animator.Play("fade_out_painel_onLevelEntry", 0, 0f);
        
        yield return new WaitForSeconds(1.49f);

        if(blink)
            SceneManager.LoadScene(this.nameScene);
    }
}
