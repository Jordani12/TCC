/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraplingHook : MonoBehaviour
{
    private Rigidbody rb;
    public Transform player;
    private PlayerStateManager playerState;

    private bool _canGrab;

    public float MaxGrappleDistance = 50f;
    public LayerMask GrappleLayer; // Defina no Inspector quais layers podem ser "agarradas"

    private Vector3 grapplePoint;
    public bool isGrappling;

    public float grapplePullForce = 10f;
    public float upwardBoost = 2f; // Adiciona um pouco de altura ao movimento

    public bool mantem;

    public LineRenderer grappleLine;
    private void Start()
    {
        playerState = player.GetComponent<PlayerStateManager>();
        rb = player.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (_canGrab)
        {
            FindGameObject();
            Line();

            if (isGrappling && Vector3.Distance(transform.position, grapplePoint) < 2f)
            {
                isGrappling = false;
                rb.velocity = Vector3.zero; // Para o jogador ao chegar perto
            }
        }
    }
    public void CheckCanGrab(bool canGrab)
    {
        _canGrab = canGrab;
    }
    private void FindGameObject()
    {
        if (mantem && playerState.onGrapling){
            isGrappling = true;
            return;
        }
            
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Centro da tela
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MaxGrappleDistance, GrappleLayer))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            mantem = true;
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            _canGrab = false;
            isGrappling = false;
        }
    }

    private void Line()
    {
        if (isGrappling)
        {
            grappleLine.enabled = true;
            grappleLine.SetPosition(0, transform.position); // Ponto inicial (jogador)
            grappleLine.SetPosition(1, grapplePoint);       // Ponto final (gancho)
        }
        else
        {
            grappleLine.enabled = false;
        }
    }
    private void FixedUpdate()
    {
        if (isGrappling)
        {
            Vector3 directionToGrapple = (grapplePoint - transform.position).normalized;

            // Adiciona força na direção do gancho
            rb.AddForce(directionToGrapple * grapplePullForce, ForceMode.Force);

            // Adiciona um pouco de força para cima para evitar colisões com o chão
            rb.AddForce(Vector3.up * upwardBoost, ForceMode.Force);
        }
    }
}
*/