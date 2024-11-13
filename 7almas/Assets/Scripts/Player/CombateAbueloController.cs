using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateAbueloController : MonoBehaviour
{
    [Header("Controlador Ataque Principal")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private Vector2 tamanioCajaGolpe;
    [SerializeField] private float danioGolpe;

    [Header("Controlador Ataque Secundario")]
    [SerializeField] private Transform controladorGolpeSecundario;
    [SerializeField] private float rango;
    [SerializeField] private GameObject efectoImpacto;
    [SerializeField] private float danioGolpeSecundario;
    [SerializeField] private float offsetDisparoX;
    [SerializeField] private GameObject efectoDisparo;  // Efecto de disparo desde el cañón

    [Header("Cantidad Balas")]
    [SerializeField] private int disparosRestantes = 5;

    [Header("Cooldown")]
    [SerializeField] private float cooldownTiempo = 0.35f;
    private float tiempoUltimoDisparo = 0f;

    [Header("Animation")]
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Solo se permite el ataque secundario si ha pasado el cooldown
        if (Input.GetButtonDown("Ataque Secundario") && Time.time >= tiempoUltimoDisparo + cooldownTiempo)
        {
            if (disparosRestantes > 0)
            {
                GolpeSecundario();
                disparosRestantes--;
                tiempoUltimoDisparo = Time.time;
            }
            else
            {
                animator.SetTrigger("Recargar");
                //Recargar();
            }
        }

        if (Input.GetButtonDown("Ataque Principal"))
        {
            Golpe();
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

    public void Recargar()
    {
        Debug.Log("Recargando...");
        disparosRestantes = 5;
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

        // Gizmos.color = Color.green;
        // Gizmos.DrawWireCube(controladorGolpeSecundario.position, tamanioCajaGolpeSecundario);
    }
}
