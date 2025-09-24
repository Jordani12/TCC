using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FovControl : MonoBehaviour
{
    private float valorPadraoFov = 70;

    [SerializeField] private Slider fovSlider;
    public SO_SaveInputs inputs;

    private void Awake()
    {
        atribuate_values();
    }
    
    public void Fov_Change()
    {
        float change = valorPadraoFov;
        change += fovSlider.value;
        inputs.fov = change;
        PlayerCamera playerCam = FindObjectOfType<PlayerCamera>();
        if(playerCam != null) Camera.main.fieldOfView = change;
    }

    public void Default_Value()
    {
        fovSlider.value = 0f;
    }

    private void atribuate_values()
    {
        try {
            if (fovSlider == null) return;

            fovSlider.minValue = -10f;
            fovSlider.maxValue = 20f;
            fovSlider.value = inputs.fov - valorPadraoFov;
        }
        catch (System.Exception e) {
            Debug.LogError("Erro: " + e.Message);
        }
    }
}
