using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateMeeleController : MonoBehaviour
{
    [Header("Controlador Ataque Principal")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float danioGolpe;

    [Header("Controlador Ataque Secundario")]
    [SerializeField] private Transform controladorGolpeSecundario;
    [SerializeField] private Vector2 tamanioCajaGolpeSecundario;
    [SerializeField] private float danioGolpeSecundario;
    [SerializeField] private float distanciaEmpujeSecundario; // Distancia del empuje en el golpe secundario
    [SerializeField] private float duracionEmpuje; // Tiempo que tomará el empuje

    [Header("Animation")]
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Ataque Principal"))
        {
            Golpe();
        }
        if (Input.GetButtonDown("Ataque Secundario"))
        {
            GolpeSecundario();
        }
    }

    private void Golpe()
    {
        animator.SetTrigger("Golpe");

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

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

        Collider2D[] objetos = Physics2D.OverlapBoxAll(controladorGolpeSecundario.position, tamanioCajaGolpeSecundario, 0);

        foreach (Collider2D collisionador in objetos)
        {
            IDanio objeto = collisionador.GetComponent<IDanio>();
            if (objeto != null)
            {
                objeto.TomarDanio(danioGolpeSecundario);

                Vector2 direccionEmpuje = (collisionador.transform.position - controladorGolpeSecundario.position).normalized;
                Vector2 posicionFinal = collisionador.transform.position + (Vector3)(direccionEmpuje * distanciaEmpujeSecundario);

                // Solo usar el eje X
                posicionFinal.y = collisionador.transform.position.y;

                StartCoroutine(EmpujarConLerp(collisionador.transform, posicionFinal, duracionEmpuje));
            }
        }
    }

    private IEnumerator EmpujarConLerp(Transform enemigo, Vector2 posicionFinal, float duracion)
    {
        Vector2 posicionInicial = enemigo.position;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            // Lerp solo en el eje X
            Vector2 nuevaPosicion = new Vector2(
                Mathf.Lerp(posicionInicial.x, posicionFinal.x, tiempoTranscurrido / duracion),
                enemigo.position.y // Mantener la posición Y igual
            );

            enemigo.position = nuevaPosicion;
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        enemigo.position = new Vector2(posicionFinal.x, enemigo.position.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(controladorGolpeSecundario.position, tamanioCajaGolpeSecundario);
    }
}
