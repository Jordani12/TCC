using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashCooldown : MonoBehaviour
{
    private float timeSlider = 0;
    [SerializeField] private float _dashCooldown = 5;

    //booleans
    private bool canCheck = true;
    public bool onCooldown { get; set; } = false;

    //references
    private PlayerStateManager player;
    private Slider slider => player.dashSlider;

    private void Start()
    {
        player = GetComponent<PlayerStateManager>();
        if(slider == null) { return; }
        slider.maxValue = _dashCooldown;
        slider.value = slider.maxValue;
    }

    private void Update()
    {
        if (onCooldown && canCheck)
            StartCoroutine(dashCooldown());
        if(timeSlider >= _dashCooldown){
            timeSlider = 0;
            onCooldown = false;
            player.canDash = true;
        }
    }
    private IEnumerator dashCooldown()
    {
        canCheck = false;
        timeSlider += Time.deltaTime;
        slider.value = timeSlider;
        yield return new WaitForEndOfFrame();
        canCheck = true;
    }
}