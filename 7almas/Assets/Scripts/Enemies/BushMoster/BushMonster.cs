using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushMonster : MonoBehaviour, IDanio
{
    // * Propiedades del enemigo
    private Rigidbody2D rb2D;
    private Transform jugador;
    private Animator animator;
    private Renderer renderer;
    private Vector2 movement;
    private bool enMovimiento;

    [Header("Vida")]
    [SerializeField] private float vida;

    [Header("Control Ataque")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private float velocidad;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform ControladorAtaque;
    [SerializeField] private float dimensionesAtaque;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float danioAtaque;
    private float attackTimer = 0f;
    private bool atacando = false;

    [Header("Patrullaje")]
    [SerializeField] private Transform detectorSuelo;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float longitudRaycastSuelo = 2f;

    [SerializeField] private Transform detectorPared;
    [SerializeField] private float longitudRaycastPared = 0.7f;

    [Header("Control Daño")]
    [SerializeField] private Color colorDaño = new Color(0.3098f, 0.0039f, 0f, 1f);
    [SerializeField] private float tiempoRestablecerColor = 0.2f;

    [Header("Puntos Enemigo")]
    [SerializeField] private float cantidadPuntos;
    private PuntajeController puntaje;

    [Header("Monedas Enemigo")]
    [SerializeField] private float cantidadMonedas;
    private DineroController dinero;
    private bool estaMuerto = false;

    private IEnumerator BuscarJugador(float tiempoMaximo)
    {
        float tiempoTranscurrido = 0f;

        while (jugador == null && tiempoTranscurrido < tiempoMaximo)
        {
            GameObject jugadorObject = GameObject.FindGameObjectWithTag("Player");
            if (jugadorObject != null)
            {
                jugador = jugadorObject.transform;
                yield break;
            }

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        if (jugador == null)
        {
            Debug.LogWarning("Jugador no encontrado después de " + tiempoMaximo + " segundos. Verifica que el jugador tenga la etiqueta 'Player' y que esté en la escena.");
        }
    }

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(BuscarJugador(5f));
        renderer = GetComponent<Renderer>(); // Obtener el Renderer
    }

    void Update()
    {
        // Contar el cooldown del ataque
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (jugador == null) return;
        if (estaMuerto) return;
        float distanciaJugador = Vector2.Distance(transform.position, jugador.position);
        if (distanciaJugador < detectionRadius && distanciaJugador > attackRange)
        {
            if (!atacando)  // No puede moverse mientras está atacando
            {
                Vector2 direction = (jugador.position - transform.position).normalized;

                movement = new Vector2(direction.x, 0);

                MirarJugador();

                enMovimiento = true;
            }
        }
        else if (distanciaJugador <= attackRange)  // Si está en el rango de ataque
        {
            MirarJugador();

            movement = Vector2.zero;  // Detener el movimiento
            enMovimiento = false;

            // Comenzar ataque si no está ya atacando y si el cooldown ha terminado
            if (!atacando && attackTimer <= 0)
            {
                atacando = true;
                attackTimer = attackCooldown;  // Reiniciar el cooldown
                animator.SetBool("Atacando", atacando);
            }
        }
        else
        {
            Patrullar();
        }

        // Solo moverse si no está atacando
        if (!atacando)
        {
            rb2D.MovePosition(rb2D.position + movement * velocidad * Time.deltaTime);
        }

        animator.SetBool("enMovimiento", enMovimiento);
    }

    public void TomarDanio(float danio)
    {
        vida -= danio;

        if (vida <= 0)
        {
            estaMuerto = true;
            animator.SetTrigger("Muerte");

            rb2D.velocity = Vector2.zero;
            rb2D.isKinematic = true;

            CancelInvoke("DesactivarArma");  // Cancelar cualquier ataque en progreso
            atacando = false;
        }
        else
        {
            Debug.Log("Recibi daño");
            CambiarColorDanio();
        }
    }

    private void Patrullar()
    {

        RaycastHit2D sueloDetectado = Physics2D.Raycast(detectorSuelo.position, Vector2.down, longitudRaycastSuelo, capaSuelo);
        // ? Invierto la dirección pq el enemigo está mirando a la izq por defecto
        RaycastHit2D paredDetectada = Physics2D.Raycast(detectorPared.position, Vector2.right * -1 * transform.localScale.x, longitudRaycastPared, capaSuelo);

        // Si no detecta suelo o si detecta una pared, cambiar de dirección
        if (!sueloDetectado || paredDetectada)
        {
            Girar();
        }

        // ? Invierto la dirección pq el enemigo está mirando a la izq por defecto
        movement = new Vector2(-transform.localScale.x, 0);
        enMovimiento = true;
    }


    private void MirarJugador()
    {   // ? Invierto la dirección pq el enemigo está mirando a la izq por defecto
        if (jugador.position.x > transform.position.x && transform.localScale.x > 0)
        {
            Girar();
        }
        else if (jugador.position.x < transform.position.x && transform.localScale.x < 0)
        {
            Girar();
        }
    }

    public void Atacar()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(ControladorAtaque.position, dimensionesAtaque);

        foreach (Collider2D collisionador in objetos)
        {
            if (collisionador.CompareTag("Player"))
            {
                Debug.Log("Collisione");
                collisionador.transform.GetComponent<VidaController>().TomarDanio(danioAtaque);
            }
        }
    }

    private void DesactivarArma()
    {
        atacando = false;
        animator.SetBool("Atacando", atacando);
    }

    private void Girar()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void CambiarColorDanio()
    {
        renderer.material.color = colorDaño;
        StartCoroutine(RestablecerColor());
    }

    private IEnumerator RestablecerColor()
    {
        // Esperar un tiempo y luego restaurar el color original
        yield return new WaitForSeconds(tiempoRestablecerColor);
        renderer.material.color = Color.white; // Cambia a color original
    }

    private void Muerte()
    {
        gameObject.SetActive(false);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(ControladorAtaque.position, dimensionesAtaque);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(detectorSuelo.position, detectorSuelo.position + Vector3.down * longitudRaycastSuelo);

        // ? Invierto la dirección pq el enemigo está mirando a la izq por defecto
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(detectorPared.position, detectorPared.position + Vector3.right * transform.localScale.x * -1 * longitudRaycastPared);
    }
}
