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
    private InspectorCollectible _inspectorCollectible;
    public SO_SaveInputs inputs_SO;

    private void Awake()
    {
        atribuate_colors();
        atribuate_text();
    }

    private void Start()
    {
        _inspectorCollectible = FindObjectOfType<InspectorCollectible>();
    }

    private void atribuate_text()
    {

        foreach (TMP_InputField inputField in inputs)
        {
            string name = inputField.name;

                switch (inputField.transform.parent.name)
                {
                    case "mvForward":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.forward_in);
                        break;
                    case "mvBackward":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.backward_in);
                        break;
                    case "mvLeft":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.left_in);
                        break;
                    case "mvRight":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.right_in);
                        break;
                    case "interact":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.interact_in);
                        break;
                    case "dash":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.dash_in);
                        break;
                    case "finalizate":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.finalization_in);
                        break;
                    case "changeShotgun":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.change2);
                        break;
                    case "recharge":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.recharging);
                        break;
                    case "changeMelee":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.change3);
                        break;
                    case "changePistol":
                        inputField.text = KeyCodeToDisplayString(inputs_saved.change1);
                        break;
                    default:
                        return;
                }
            detect_input_repeat();
        }
        
    }

    private string KeyCodeToDisplayString(KeyCode keyCode)
    {
        // Converte KeyCode para string amigável
        switch (keyCode)
        {
            case KeyCode.Alpha0: return "0";
            case KeyCode.Alpha1: return "1";
            case KeyCode.Alpha2: return "2";
            case KeyCode.Alpha3: return "3";
            case KeyCode.Alpha4: return "4";
            case KeyCode.Alpha5: return "5";
            case KeyCode.Alpha6: return "6";
            case KeyCode.Alpha7: return "7";
            case KeyCode.Alpha8: return "8";
            case KeyCode.Alpha9: return "9";

            case KeyCode.Keypad0: return "Num 0";
            case KeyCode.Keypad1: return "Num 1";
            case KeyCode.Keypad2: return "Num 2";
            case KeyCode.Keypad3: return "Num 3";
            case KeyCode.Keypad4: return "Num 4";
            case KeyCode.Keypad5: return "Num 5";
            case KeyCode.Keypad6: return "Num 6";
            case KeyCode.Keypad7: return "Num 7";
            case KeyCode.Keypad8: return "Num 8";
            case KeyCode.Keypad9: return "Num 9";

            // Outras teclas comuns que podem ter nomes muito longos
            case KeyCode.LeftShift: return "Shift";
            case KeyCode.RightShift: return "R Shift";
            case KeyCode.LeftControl: return "Ctrl";
            case KeyCode.RightControl: return "R Ctrl";
            case KeyCode.LeftAlt: return "Alt";
            case KeyCode.RightAlt: return "R Alt";
            case KeyCode.Return: return "Enter";
            case KeyCode.KeypadEnter: return "Num Enter";
            case KeyCode.Escape: return "Esc";
            case KeyCode.Space: return "Space";
            case KeyCode.Backspace: return "Backspace";
            case KeyCode.Tab: return "Tab";
            case KeyCode.CapsLock: return "Caps Lock";

            // Para outras teclas, usa o nome padrão mas remove prefixos desnecessários
            default:
                string keyString = keyCode.ToString();

                // Remove "Keypad" prefix
                if (keyString.StartsWith("Keypad"))
                    return "Num " + keyString.Substring(6);

                // Remove "Alpha" prefix
                if (keyString.StartsWith("Alpha"))
                    return keyString.Substring(5);

                return keyString;
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

            if(IsNumericKey(inputText, out keyFor)) { }

            else
            {
                try
                {
                    keyFor = (KeyCode)Enum.Parse(typeof(KeyCode), inputText, true);
                }
                catch
                {
                    currentInput.text = input_value_before;
                    return;
                }
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
                        inputs_SO.interact_in = keyFor;
                        break;
                    case "dash":
                        inputs_SO.dash_in = keyFor;
                        break;
                    case "finalizate":
                        inputs_SO.finalization_in = keyFor;
                        break;
                    case "changeShotgun":
                        inputs_SO.change2 = keyFor;
                        break;
                    case "recharge":
                        inputs_SO.recharging = keyFor;
                        break;
                    case "changeMelee":
                        inputs_SO.change3 = keyFor;
                        break;
                    case "changePistol":
                        inputs_SO.change1 = keyFor;
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


    private bool IsNumericKey(string input, out KeyCode keyCode)
    {
        keyCode = KeyCode.None;

        var alphaMap = new Dictionary<string, KeyCode>
    {
        {"1", KeyCode.Alpha1}, {"2", KeyCode.Alpha2}, {"3", KeyCode.Alpha3},
        {"4", KeyCode.Alpha4}, {"5", KeyCode.Alpha5}, {"6", KeyCode.Alpha6},
        {"7", KeyCode.Alpha7}, {"8", KeyCode.Alpha8}, {"9", KeyCode.Alpha9},
        {"0", KeyCode.Alpha0}
    };

        var keypadMap = new Dictionary<string, KeyCode>
    {
        {"Keypad1", KeyCode.Keypad1}, {"Keypad2", KeyCode.Keypad2}, {"Keypad3", KeyCode.Keypad3},
        {"Keypad4", KeyCode.Keypad4}, {"Keypad5", KeyCode.Keypad5}, {"Keypad6", KeyCode.Keypad6},
        {"Keypad7", KeyCode.Keypad7}, {"Keypad8", KeyCode.Keypad8}, {"Keypad9", KeyCode.Keypad9},
        {"Keypad0", KeyCode.Keypad0}
    };

        if (alphaMap.ContainsKey(input))
        {
            keyCode = alphaMap[input];
            return true;
        }

        return false;
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
