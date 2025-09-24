using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Linq;

public class InputControl : MonoBehaviour
{
    public SO_SaveInputs inputs_saved;

    //color inputs controller
    private ColorBlock corInvalid_input;
    private ColorBlock corNeutro_input;

    private Color corNeutroText_input = Color.black;
    private Color corInvalidText_input = Color.white;

    [Header("Inputs TMP")]
    [SerializeField] private TMP_InputField[] inputs;
    [SerializeField] private TMP_InputField currentInput;

    //string to prevent null value
    private string input_value_before;

    //references scripts
    private PlayerStateManager player;
    private InspectorCollectible _inspectorCollectible;
    public SO_SaveInputs inputs_SO;

    private void Awake()
    {
        atribuate_colors();
        atribuate_text();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerStateManager>();
        _inspectorCollectible = FindObjectOfType<InspectorCollectible>();
    }

    private void atribuate_text()
    {

        foreach (TMP_InputField inputField in inputs)
        {
                switch (inputField.transform.parent.name)
                {
                    case "mvForward":
                        inputField.text = inputs_saved.forward_in.ToString();
                        break;
                    case "mvBackward":
                        inputField.text = inputs_saved.backward_in.ToString();
                        break;
                    case "mvLeft":
                        inputField.text = inputs_saved.left_in.ToString();
                        break;
                    case "mvRight":
                        inputField.text = inputs_saved.right_in.ToString();
                        break;
                    case "interact":
                        inputField.text = inputs_saved.interact_in.ToString();
                        break;
                    case "dash":
                        inputField.text = inputs_saved.dash_in.ToString();
                        break;
                    case "finalizate":
                        inputField.text = inputs_saved.finalization_in.ToString();
                        break;
                    default:
                        return;
                }
            detect_input_repeat();
        }
        
    }

    private void atribuate_colors()
    {
        corInvalid_input = new ColorBlock()
        {
            normalColor = Color.red,
            highlightedColor = new Color(1f, 0.6f, 0.6f),
            pressedColor = new Color(0.8f, 0.4f, 0.4f),
            selectedColor = Color.red,
            disabledColor = new Color(0.8f, 0.8f, 0.8f, 0.5f),
            colorMultiplier = 1f,
            fadeDuration = 0.1f
        };
        corNeutro_input = new ColorBlock()
        {
            normalColor = new Color(0.95f, 0.95f, 0.95f), // Cinza muito claro
            highlightedColor = new Color(0.85f, 0.85f, 0.85f), // Cinza um pouco mais escuro
            pressedColor = new Color(0.75f, 0.75f, 0.75f),     // Cinza médio
            selectedColor = new Color(0.95f, 0.95f, 0.95f),    // Mesmo que normal
            disabledColor = new Color(0.8f, 0.8f, 0.8f, 0.5f), // Cinza semi-transparente
            colorMultiplier = 1f,
            fadeDuration = 0.1f
        };
    }

    public void Input_Alternate()
    {
        if (currentInput == null) return;

        string inputText = currentInput.text;

        if (inputText != "" && inputText != input_value_before)
        {

            KeyCode keyFor;
            try
            {
                keyFor = (KeyCode)Enum.Parse(typeof(KeyCode), inputText, true);
            }
            catch
            {
                currentInput.text = input_value_before;
                return;
            }

            detect_input_repeat();

                switch (currentInput.transform.parent.name)
                {
                    case "mvForward":
                        inputs_SO.forward_in = keyFor;
                        break;
                    case "mvBackward":
                        inputs_SO.backward_in = keyFor;
                        break;
                    case "mvLeft":
                        inputs_SO.left_in = keyFor;
                        break;
                    case "mvRight":
                        inputs_SO.right_in = keyFor;
                        break;
                    case "interact":
                    if(_inspectorCollectible != null) _inspectorCollectible.interact_key = keyFor;
                        inputs_SO.interact_in = keyFor;
                        break;
                    case "dash":
                        inputs_SO.dash_in = keyFor;
                        break;
                    case "finalizate":
                        inputs_SO.finalization_in = keyFor;
                        break;
                    default:
                        currentInput.text = input_value_before;
                        return;
            } 

            // Atualiza o valor anterior para o novo valor válido
            input_value_before = inputText;
        }
        else currentInput.text = input_value_before;

        if (_inspectorCollectible!= null)
            _inspectorCollectible.AlternateBottom();
    }

    private void detect_input_repeat()
    {
        if(currentInput != null) input_value_before = currentInput.text;

        List<string> repeated_inputs1 = new List<string>();
        HashSet<string> uniqueInputs = new HashSet<string>();
        List<string> duplicatedInputs = new List<string>();

        repeated_inputs1.Clear();
        uniqueInputs.Clear();
        duplicatedInputs.Clear();

        foreach (TMP_InputField inputField in inputs)
        {
            string currentText = inputField.text.Trim();

            // Se estiver vazio, pula
            if (string.IsNullOrEmpty(currentText))
                continue;

            // Se já existe no HashSet, é duplicado
            if (!uniqueInputs.Add(currentText))
            {
                duplicatedInputs.Add(currentText);
            }
        }

        // Adiciona os duplicados à lista final
        repeated_inputs1.AddRange(duplicatedInputs.Distinct());

        foreach (TMP_InputField inputs in inputs)
        {
            if (repeated_inputs1.Contains(inputs.text))
            {
                inputs.colors = corInvalid_input;
                inputs.textComponent.color = corInvalidText_input;
            }
            else
            {
                inputs.colors = corNeutro_input;
                inputs.textComponent.color = corNeutroText_input;
            }
        }
    }

    public void SetInputField()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            TMP_InputField selectedInput = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
            if (selectedInput != null)
            {
                currentInput = selectedInput;
            }
        }
        if (currentInput != null)
            input_value_before = currentInput.text;
        currentInput.text = "";
    }

    public void Text_Upper()
    {
        foreach (TMP_InputField input in inputs)
        {
            input.text = input.text.ToUpper();
        }
        currentInput.DeactivateInputField();
    }
}
