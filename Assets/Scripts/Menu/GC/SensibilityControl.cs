using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensibilityControl : MonoBehaviour
{
    private PlayerCamera cam;

    private float[] valorPadraoSens = new float[3];

    [Header("Sliders")]
    [SerializeField] private Slider mainSensSlider;
    [SerializeField] private Slider aimSensSlider;
    [Header("SCR Obj")]
    [SerializeField] private SO_SaveInputs inputs;

    private void Awake()
    {
        default_value();
        atribuate_values();
    }
    private void Start()
    {
        cam = GameObject.FindObjectOfType<PlayerCamera>();
    }

    private void default_value()
    {
        float[] value = { 400, 300, 100, 50 }; valorPadraoSens = value;
    }

    public void Voltar_Padrao()
    {
        standart_sens(cam, aimSensSlider, mainSensSlider);
    }

    private void atribuate_values()
    {
        try
        {
            if (mainSensSlider == null) return;

            mainSensSlider.minValue = 0.1f;
            mainSensSlider.maxValue = 3f;
            mainSensSlider.value = inputs.sensitivityX / 400;

            aimSensSlider.minValue = 0.1f;
            aimSensSlider.maxValue = 3f;
            aimSensSlider.value = inputs.aim_sensitivityX / 100;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erro: " + e.Message);
        }      
    }

    private void standart_sens(PlayerCamera cam, Slider aimSlider, Slider defaultSlider)
    {
        if(cam != null) cam.sensX = valorPadraoSens[0]; 
        inputs.sensitivityX = valorPadraoSens[0];
        if (cam != null) cam.sensY = valorPadraoSens[1]; 
        inputs.sensitivityY = valorPadraoSens[1];
        if (cam != null) cam.sensOnAimY = valorPadraoSens[2]; 
        inputs.aim_sensitivityY = valorPadraoSens[2];
        if (cam != null) cam.sensOnAimX = valorPadraoSens[3]; 
        inputs.aim_sensitivityX = valorPadraoSens[3];

        defaultSlider.value = 1f;
        aimSlider.value = 1f;
    }//

    public void Slider_Controller(Slider slider)
    {
        if (slider == null) return;

        if (slider.transform.name == "mainSensSlider")
            default_slider(slider);
        else
            aim_slider(slider);
    }

    private void aim_slider(Slider slider)
    {
        float sensX = valorPadraoSens[2];
        float sensY = valorPadraoSens[3];//valor padrão
        sensY *= slider.value;
        sensX *= slider.value;
        if (cam != null) {
            cam.sensOnAimX = sensX;
            cam.sensOnAimY = sensY;
        }
        inputs.aim_sensitivityX = sensX; 
        inputs.aim_sensitivityY = sensY;
    }

    private void default_slider(Slider slider)
    {
        float sensX = valorPadraoSens[0]; //valor padrão
        float sensY = valorPadraoSens[1];
        sensX *= slider.value;
        sensY *= slider.value;
        if (cam != null) {
            cam.sensX = sensX;
            cam.sensY = sensY;
        }
        inputs.sensitivityX = sensX;
        inputs.sensitivityY = sensY;
    }
}
