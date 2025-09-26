using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    public bool ShakeDir = false;
    public float shakeSpeed = 3f;


    public float sensX; //variaveis pra sensibilidade
    public float sensY;
    public float sensOnAimX;
    public float sensOnAimY;
    [SerializeField] Transform player;
    private float xRotation;
    private float yRotation;
    [HideInInspector] public bool canMoveCam;
    [HideInInspector] public bool isAiming = false;
    private Recoil recoil;
    private PlayerStateManager playerState;
    private bool isWalking;
    private bool onGround;

    public float minHeight = 0f;
    public float maxHeight = 0.1f;
    public bool toward = true;

    [Header("Inputs")]
    public SO_SaveInputs inputs;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked; //travo o cursor no centro da tela
        Cursor.visible = false; //visibilidade do cursor é desligada
        canMoveCam = true;
        recoil = GameObject.FindObjectOfType<Recoil>();
        playerState = GameObject.FindObjectOfType<PlayerStateManager>();

        if (inputs != null) atribuate_values();
    }

    private void atribuate_values()
    {
        sensX = inputs.sensitivityX;
        sensY = inputs.sensitivityY;
        sensOnAimX = inputs.aim_sensitivityX;
        sensOnAimY = inputs.aim_sensitivityY;
        Camera camera = Camera.main;
        camera.fieldOfView = inputs.fov;
    }

    public IEnumerator ApplyDashEffect(float dash_duration)
    {
        //aumenta a "distancia" da camera
        for (int i = 0; i <= 3; i++)
        {
            Camera.main.fieldOfView += 2;
            yield return new WaitForSeconds(dash_duration / 20);
        }
        yield return new WaitForSeconds(dash_duration / 2);
        //diminui a "distancia" da camera
        for (int i = 0; i <= 3; i++)
        {
            Camera.main.fieldOfView -= 2;
            yield return new WaitForSeconds(dash_duration / 20);
        }
    }

    void Update(){

        if (transform.position.y >= maxHeight)
            toward = false;
        else if (transform.position.y <= minHeight)
            toward = true;

        if (canMoveCam)
            LogicCam(isAiming);

        if (playerState != null) { isWalking = playerState.isMoving; onGround = playerState.isGrounded; }

        if (!isWalking || !onGround){ ShakingShakira(); return;}

        ShakeUpdate();
    }

    private void LogicCam(bool isAiming){
        if(player != null)
        {
            float mouseX, mouseY;
            if (isAiming)
            {
                mouseX = Input.GetAxisRaw("Mouse X") * sensOnAimX * Time.deltaTime;
                mouseY = Input.GetAxisRaw("Mouse Y") * sensOnAimY * Time.deltaTime;
            }
            else
            {
                mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
                mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;
            }
            rotations(mouseX, mouseY);
        }
    }

    public void Head_Movement(Vector3 pos)
    {
        transform.position = pos;
    }

    private void rotations(float mouseX, float mouseY)
    {
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90); //limito as rotações em 90 graus
        
        // Rotação base do mouse
        Quaternion baseRotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Obter rotação do recoil (se existir)
        Quaternion recoilRotation = Quaternion.identity;
        if (recoil != null)
        {
            recoilRotation = recoil.GetCurrentRecoilRotation();
        }

        // Combinar as rotações - RECOIL PRIMEIRO, depois base
        transform.rotation = baseRotation * recoilRotation;
        player.rotation = Quaternion.Euler(0, yRotation, 0); //aplico a rotação em eixo Y no jogador (para afetar a movimentação)

    }

    private void ShakeUpdate()
    {
        float a = 0;
        if (ShakeDir)
        {
            a = minHeight;
        }
        else
        {
            a = maxHeight;
        }
        ShakeDir = ShakingShakira(a);
    }

    bool ShakingShakira(float height = 0)
    {
        Vector3 pos = transform.position;
        Vector3 parent = transform.parent.position;
        Vector3 newPos = new Vector3(parent.x, Mathf.Lerp(pos.y, parent.y + height, Time.deltaTime * shakeSpeed), parent.z);
        transform.position = newPos;

        float distance = Mathf.Abs(Mathf.Abs(newPos.y) - Mathf.Abs(parent.y + height));
        if (distance <= (0.1f))
        {
            transform.position = new Vector3(parent.x, parent.y + height, parent.z);
            return !ShakeDir;
        }
        return ShakeDir;
    }
}
