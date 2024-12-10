using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombateController : MonoBehaviour
{
    [Header("ControladorAtaque")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float danioGolpe;

    [Header("Animation")]
    private Animator animator;

    private Entradas entradas;

    private void Awake()
    {
        entradas = new Entradas();
    }

    private void OnEnable()
    {
        entradas.Enable();
    }

    private void OnDisable()
    {
        entradas.Disable();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        entradas.Gameplay.AtaquePrincipal.performed += context => Atacar(context);
    }

    private void Update()
    {
        // if(Input.GetButtonDown("Ataque Principal"))
        // {
        //     Golpe();
        // }        
    }

    private void Atacar(InputAction.CallbackContext context)
    {
        Golpe();
    }

    private void Golpe()
    {
        animator.SetTrigger("Golpe");

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D collisionador in objetos)
        {
            IDanio objeto = collisionador.GetComponent<IDanio>();
            if(objeto != null)
            {
                objeto.TomarDanio(danioGolpe);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);    
    }
}
