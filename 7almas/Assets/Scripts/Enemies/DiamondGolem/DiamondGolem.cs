using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondGolem : MonoBehaviour, IDanio
{
    private Transform jugador;   
    private Animator animator;
    private Vector2 movement;
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    [SerializeField] private float velocidad;
    private bool enMovimiento;

    [Header("Vida")]
    [SerializeField] private float vida;

    [Header("Ataque")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private float danioAtaque;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown = 1.05f;
    private float attackTimer = 0f;
    private bool atacando = false;

    [Header("Golpe")]
    [SerializeField] private Transform golpe;
    [SerializeField] private Vector3 dimensionesGolpe;

    [Header("Patrullaje")]
    [SerializeField] private Transform detectorSuelo;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float longitudRaycastSuelo = 2f;
    [SerializeField] private Transform detectorPared;
    [SerializeField] private float longitudRaycastPared = 0.7f;

    [Header("Control Daño")]
    private Renderer renderer;
    [SerializeField] private Color colorDaño = new Color(0.3098f, 0.0039f, 0f, 1f);
    [SerializeField] private float tiempoRestablecerColor = 0.2f; 

    private bool estaMuerto = false;

    //TODO: Métodos default
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (estaMuerto) return;

        float distanciaJugador = Vector2.Distance(transform.position, jugador.position);
        if (distanciaJugador < detectionRadius && distanciaJugador > attackRange)
        {
            if (!atacando)
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
                Atacar();
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

    //TODO: Gestión del daño
    public void TomarDanio(float danio)
    {
        vida -= danio;

        if (vida <= 0)
        {
            Muerte();
        }
        else
        {
            Debug.Log("Recibi daño");
            CambiarColorDanio();
        }
    }

    private void CambiarColorDanio()
    {
        // Cambiar color al color de daño
        renderer.material.color = colorDaño;
        StartCoroutine(RestablecerColor());
    }

    private IEnumerator RestablecerColor()
    {
        // Esperar un tiempo y luego restaurar el color original
        yield return new WaitForSeconds(tiempoRestablecerColor);
        renderer.material.color = Color.white; // Cambia a color original
    }

    //TODO: Mirada del enemigo
    private void MirarJugador()
    {
        if (jugador.position.x > transform.position.x && transform.localScale.x < 0)
        {
            Girar();
        }
        else if (jugador.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Girar();
        }
    }
    private void Girar()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    //TODO: Control del ataque
    private void Atacar()
    {
        atacando = true;
        attackTimer = 0;  // Reiniciar el cooldown
        animator.SetBool("Atacando", atacando);

        StartCoroutine(WaitAMoment());

        Invoke("DesactivarArma", attackCooldown);
    }

    private IEnumerator WaitAMoment()
    {
        yield return new WaitForSeconds(attackCooldown);

        Vector2 size = new Vector2(dimensionesGolpe.x, dimensionesGolpe.y);

        Collider2D[] objetos = Physics2D.OverlapBoxAll(golpe.position, size, 0f);

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

    //TODO: Control del patrullaje
    private void Patrullar()
    {
        // Hacer raycast hacia abajo para detectar si hay suelo
        RaycastHit2D sueloDetectado = Physics2D.Raycast(detectorSuelo.position, Vector2.down, longitudRaycastSuelo, capaSuelo);
        // Hacer raycast hacia adelante para detectar si hay una pared
        RaycastHit2D paredDetectada = Physics2D.Raycast(detectorPared.position, Vector2.right * transform.localScale.x, longitudRaycastPared, capaSuelo);

        // Si no detecta suelo o si detecta una pared, cambiar de dirección
        if (!sueloDetectado || paredDetectada)
        {
            Girar();
        }

        // Mover en la dirección actual
        movement = new Vector2(transform.localScale.x, 0);
        enMovimiento = true;
    }

    //TODO: Control de la muerte
    private void Muerte()
    {
        estaMuerto = true;
        animator.SetTrigger("Muerte");

        rb2D.velocity = Vector2.zero;
        rb2D.isKinematic = true;

        CancelInvoke("DesactivarArma");  // Cancelar cualquier ataque en progreso
        atacando = false;
    }

    //TODO: Gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(golpe.position, dimensionesGolpe);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(detectorSuelo.position, detectorSuelo.position + Vector3.down * longitudRaycastSuelo);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(detectorPared.position, detectorPared.position + Vector3.right * transform.localScale.x * longitudRaycastPared);
    }
}
