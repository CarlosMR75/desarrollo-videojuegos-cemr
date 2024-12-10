using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombateAbueloController : MonoBehaviour
{
    [Header("Controlador Ataque Principal")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private Vector2 tamanioCajaGolpe;
    [SerializeField] private float danioGolpe;

    [Header("Ataque Secundario")]
    [SerializeField] private Transform controladorGolpeSecundario;
    [SerializeField] private float rango;
    [SerializeField] private GameObject efectoImpacto;
    [SerializeField] private float danioGolpeSecundario;
    [SerializeField] private float offsetDisparoX;
    [SerializeField] private GameObject efectoDisparo;

    [Header("Cooldown")]
    [SerializeField] private float cooldownTiempo = 0.35f;
    private float tiempoUltimoDisparo = 0f;

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
        entradas.Gameplay.AtaquePrincipal.performed += context => AtaquePrincipal();
        entradas.Gameplay.AtaqueSecundario.performed += context => AtaqueSecundario();
    }

    private void AtaquePrincipal()
    {
        Golpe();
    }

    private void AtaqueSecundario()
    {
        if (Time.time >= tiempoUltimoDisparo + cooldownTiempo)
        {
            GolpeSecundario();
            tiempoUltimoDisparo = Time.time;
        }
    }

    private void Golpe()
    {
        animator.SetTrigger("Golpe");

        Collider2D[] objetos = Physics2D.OverlapBoxAll(controladorGolpe.position, tamanioCajaGolpe, 0);

        foreach (Collider2D collisionador in objetos)
        {
            IDanio objeto = collisionador.GetComponent<IDanio>();
            if (objeto != null)
            {
                objeto.TomarDanio(danioGolpe);
            }
        }
    }

    private void GolpeSecundario()
    {
        animator.SetTrigger("GolpeSecundario");

        if (efectoDisparo != null)
        {
            Vector3 direccionDisparo = controladorGolpeSecundario.right * Mathf.Sign(transform.localScale.x);
            Vector3 posicionDisparo = controladorGolpeSecundario.position + direccionDisparo * offsetDisparoX;

            GameObject efectoInstanciado = Instantiate(efectoDisparo, posicionDisparo, Quaternion.identity);

            if (transform.localScale.x < 0)
            {
                efectoInstanciado.transform.Rotate(0, 180, 0);
            }
        }

        RaycastHit2D raycastHit2D = Physics2D.Raycast(controladorGolpeSecundario.position, controladorGolpeSecundario.right * transform.localScale.x, rango);

        if (raycastHit2D)
        {
            if (raycastHit2D.transform.CompareTag("Enemigo"))
            {
                raycastHit2D.transform.GetComponent<IDanio>().TomarDanio(danioGolpeSecundario);
                GameObject impacto = Instantiate(efectoImpacto, raycastHit2D.point, Quaternion.identity);

                if (transform.localScale.x < 0)
                {
                    impacto.transform.Rotate(0, 180, 0);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorGolpe.position, tamanioCajaGolpe);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(controladorGolpeSecundario.position, controladorGolpeSecundario.position + Vector3.right * transform.localScale.x * rango);
    }
}
