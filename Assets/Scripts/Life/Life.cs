using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
public class Life : MonoBehaviour
{
    //float life
    public int actualLife;
    public int maxLife;

    //UI 
    [SerializeField] private Slider BarraDeVida;
    [SerializeField] private TextMeshProUGUI vidaTxt;

    [SerializeField] private GameObject low_life_txt;

    private Main_Animation anim;
    private GetToDeath death;

    void Start(){//atualiza a vida atual para a max
        actualLife = maxLife;
        BarraDeVida.maxValue = maxLife;
        BarraDeVida.value = actualLife;

        vidaTxt.text = maxLife.ToString();
        low_life_txt.SetActive(false);

        anim = FindObjectOfType<Main_Animation>();
        death = FindObjectOfType<GetToDeath>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) {
            TakeDamage(30);
        }
        update_life_UI();
    }

    public void low_life_text(bool ligado){
        if(ligado) low_life_txt.SetActive(true);
        else low_life_txt.SetActive(false);
    }

    public void TakeDamage(int value){
        actualLife -= value;
        if (actualLife <= 0){
            actualLife = 0;
            if (gameObject.transform.parent != null){//caso o componente life n�o esteja no objeto que queira destruir em si
                Transform parent = gameObject.transform.parent;
                Die(parent.gameObject);
            }
            else
                Die(gameObject);
        }
        if(actualLife <= maxLife / 4) low_life_text(true);
        else low_life_text(false);
    }

    private void update_life_UI()
    {
        BarraDeVida.value = actualLife;
        vidaTxt.text = actualLife.ToString();
    }

    private void Die(GameObject objeto){
        anim.DeadMoveCam();
        death.can_change = true;
        Destroy(objeto);
    }
}
