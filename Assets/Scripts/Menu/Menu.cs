using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    private MenuAnimation anim;
    private EventSystem eventSystem;

    // 👇 Elementos da UI (arrastados no Inspector)
    [Header("Async Objects")]
    [SerializeField] private GameObject menuOptions;
    [SerializeField] private Slider progressSlider;        // Barra de progresso de carregamento
    [SerializeField] private TextMeshProUGUI messageText; // Texto de status
    [Header("Canvas")]
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject optionsCanvas;
    [Header("GameObjects")]
    [SerializeField] private GameObject pressText;
    [SerializeField] private GameObject play_button;
    [SerializeField] private GameObject options_button;
    [Header("Opt GO")]
    [SerializeField] private GameObject main_options;
    [SerializeField] private GameObject _control;


    private bool pressedAnyButton;
    [SerializeField] private Slider mainSensSlider;
    [SerializeField] private Slider aimSensSlider;
    [SerializeField] private Slider fovSlider;

    private InputControl _input;
    private SensibilityControl _sensibility;
    private FovControl _fov;

    private AsyncOperation async;  // Controle da operação assíncrona de carregamento

    private void Awake()
    {
        destroy_events_system();
    }

    private void destroy_events_system()
    {
        EventSystem myEventSystem = GetComponent<EventSystem>();

        EventSystem[] allEventSystems = FindObjectsOfType<EventSystem>();

        foreach (EventSystem es in allEventSystems)
        {
            if (es != myEventSystem)
            {
                Destroy(es.gameObject);
            }
        }
    }

    private void Start()
    {
        _fov = FindObjectOfType<FovControl>();
        _sensibility = FindObjectOfType<SensibilityControl>();
        _input = FindObjectOfType<InputControl>();
        anim = FindObjectOfType<MenuAnimation>();
        eventSystem = GetComponent<EventSystem>();
        inicial_settings();
    }

    private void inicial_settings()
    {
        this.optionsCanvas.SetActive(false);
        this.mainCanvas.SetActive(true);
        this.menuOptions.SetActive(false);
        this.pressText.SetActive(true);
        this.progressSlider.gameObject.SetActive(false);
        this._control.SetActive(false);
    }

    public void Jogar()
    {
        this.menuOptions.SetActive(!menuOptions.activeInHierarchy);
        this.progressSlider.gameObject.SetActive(true);
        
        StartCoroutine(CarregarCena());
    }

    private void Update()
    {
        if(!pressedAnyButton && Input.anyKeyDown) {
            open_menu_options();
            anim.FadeInMenuOptions();
        }
    }

    public void VoltarPadrao()
    {
        float valorPadraoFov = 70;
        Camera.main.fieldOfView = valorPadraoFov;

        _fov.Default_Value();
        _sensibility.Voltar_Padrao();
    }

    public void SensSlider()
    {
        if (_sensibility == null) return;

        _sensibility.Slider_Controller(mainSensSlider);
    }//

    public void AimSensSlider()
    {
        if (_sensibility == null) return;

        _sensibility.Slider_Controller(aimSensSlider);
    }

    public void Fov()
    {
        _fov.Fov_Change();
    }

    public void AlternateButtons()
    {
        if (_input == null) return;
        _input.Input_Alternate();
    }

    public void textToUpper()
    {
        if (_input == null) return;
        _input.Text_Upper();
    }

    public void SetInputField()
    {
        if (_input == null) return;
        _input.SetInputField();
    }

    private void open_menu_options()
    {
        eventSystem.firstSelectedGameObject = play_button;
        this.pressedAnyButton = true;
        this.menuOptions.SetActive(true);
        this.pressText.SetActive(false);
    }

    public void Options()
    {
        eventSystem.firstSelectedGameObject = options_button;
        this.mainCanvas.SetActive(false);
        this.optionsCanvas.SetActive(true);
        activateButtonsOptions();
    }

    public void Comeback()
    {
        eventSystem.firstSelectedGameObject = play_button;
        this.mainCanvas.SetActive(true);
        this.optionsCanvas.SetActive(false);
    }
    public void OptionsBack()
    {
        activateButtonsOptions();
    }
    private void activateButtonsOptions()
    {
        main_options.SetActive(true);

        _control.SetActive(false);
    }

    private void activateControl()
    {
        main_options.SetActive(false);
        _control.SetActive(true);
    }

    public void control()
    {
        activateControl();
    }

    private IEnumerator CarregarCena()
    {
        async = SceneManager.LoadSceneAsync("Parte1");
        
        async.allowSceneActivation = true;

        this.pressedAnyButton = false;

        while (!async.isDone)
        {
            this.messageText.text = "Carregando... " + (async.progress * 100f).ToString("F0") + "%";
            
            this.progressSlider.value = async.progress;
            
            yield return null;
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // Só funciona em builds
        #endif
    }
}