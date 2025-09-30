using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuPause : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] public GameObject mainCanvas;
    [SerializeField] public GameObject pauseCanvas;

    [Header("Main Options Pause")]
    [SerializeField] private GameObject Main_Options;

    [Header("Menu Options")]
    [SerializeField] private GameObject Options_Menu;

    [Header("Control Options")]
    [SerializeField] private GameObject Control_Menu;

    [Header("Game Controller")]
    [SerializeField] GameObject Game_Controller;

    [Header("Sliders")]
    [SerializeField] private Slider mainSensSlider;
    [SerializeField] private Slider aimSensSlider;

    private GunController gun;
    private PlayerStateManager player;

    //scripts gameController
    private SensibilityControl _sensibility;
    private FovControl _fov;
    private InputControl _input;
    private GetToDeath death;

    //variables
    private bool isPaused = false;
    [HideInInspector] public static bool can_change_canvas;

    private CanvasGroup pauseMenuCanvasGroup;

    void Start()
    {
        _sensibility = Game_Controller.GetComponent<SensibilityControl>();
        _fov = Game_Controller.GetComponent<FovControl>();
        _input = Game_Controller.GetComponent<InputControl>();
        
        player = FindObjectOfType<PlayerStateManager>();
        gun = FindObjectOfType<GunController>();
        death = FindObjectOfType<GetToDeath>();

        StartCoroutine(activate_canvas());
        canvas_group();

        can_change_canvas = true;
    }

    private IEnumerator activate_canvas()
    {
        yield return null;
        if (mainCanvas != null || pauseCanvas != null)
        {
            activateButtons();
            mainCanvas.SetActive(true);
            pauseCanvas.SetActive(false);
        }
    }

    private void canvas_group()
    {
        if (pauseCanvas != null)
        {
            CanvasGroup pauseMenuCanvasGroup = pauseCanvas.GetComponent<CanvasGroup>();
            if (pauseMenuCanvasGroup == null)
            {
                // Adiciona CanvasGroup dinamicamente se não existir
                pauseMenuCanvasGroup = pauseCanvas.AddComponent<CanvasGroup>();
                SetPauseMenuVisibility(false);
            }
        }
    }

    void Update()
    {
        bool press_B = Input.GetKeyDown(KeyCode.B);

        if (press_B && can_change_canvas)
        {
            player.canDash = !player.canDash;
            if(gun.currentGun.passedTutorial)
                gun.currentGun.canShoot = !gun.currentGun.canShoot;
            timeChange();
            SetPauseMenuVisibility(isPaused);
            canva_controller();
            toggle_cursor();
            activateButtons();
        }
    }

    private void canva_controller()
    {
        mainCanvas.SetActive(!mainCanvas.activeInHierarchy);
        pauseCanvas.SetActive(!pauseCanvas.activeInHierarchy);
    }

    private void toggle_cursor()
    {
        if (Cursor.visible)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        Cursor.visible = !Cursor.visible;
    }

    public void SensSlider()
    {
        if (_sensibility == null) return;
        _sensibility.Slider_Controller(mainSensSlider);
    }

    public void AimSensSlider()
    {
        if (_sensibility == null) return;
        _sensibility.Slider_Controller(aimSensSlider);
    }

    private void activateButtons()
    {
        Main_Options.SetActive(true);

        Options_Menu.SetActive(false);

        Control_Menu.SetActive(false);
    }
    private void activateButtonsOptions()
    {
        Main_Options.SetActive(false);

        Options_Menu.SetActive(true);

        Control_Menu.SetActive(false);
    }
    private void SetPauseMenuVisibility(bool isVisible)
    {
        if (pauseMenuCanvasGroup != null)
        {
            pauseMenuCanvasGroup.alpha = isVisible ? 1 : 0; // Transparência (0 = invisível, 1 = visível)
            pauseMenuCanvasGroup.interactable = isVisible; // Permite clicar nos botões
            pauseMenuCanvasGroup.blocksRaycasts = isVisible; // Bloqueia interação com o cenário
        }
    }

    public void Fov()
    {
        if (_fov == null) _fov = FindObjectOfType<FovControl>();
        _fov.Fov_Change();
    }

    public void Resume()
    {
        SetPauseMenuVisibility(false);
        canva_controller();
        timeChange();
        toggle_cursor();/*se no futuro não funcionar, use isto: Cursor.lockState = CursorLockMode.Locked;Cursor.visible = false;*/
        player.canDash = !player.canDash;
        if (gun.currentGun.passedTutorial)
            gun.currentGun.canShoot = true;
    }

    private void timeChange()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // Só funciona em builds
        #endif
    }

    public void OpenControls()
    {
        activateControlOptions();
    }

    private void activateControlOptions()
    {
        Options_Menu.SetActive(false);

        Control_Menu.SetActive(true);
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

    public void BackMenu()
    {
        mainCanvas.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(death.Back_Menu(pauseCanvas, mainCanvas));
    }

    public void Options()
    {
        activateButtonsOptions();
    }

    public void OptionsBack()
    {
        activateButtons();   
    }

    public void VoltarPadrao()
    {
        float valorPadraoFov = 70;
        Camera.main.fieldOfView = valorPadraoFov;

        _fov.Default_Value();
        _sensibility.Voltar_Padrao();
    }
}
