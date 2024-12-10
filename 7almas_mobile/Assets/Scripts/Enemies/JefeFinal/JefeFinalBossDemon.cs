using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeFinalBossDemon : MonoBehaviour, IDanio
{
    private Animator animator;
    public Rigidbody2D rb2D;
    public Transform jugador;
    public bool mirandoDerecha;
    private Vector2 movement;

    [Header("Vida")]
    [SerializeField] private float vida;
    //Bara de vida

    [Header("Ataque")]
    [SerializeField] private Transform controladorArma;
    [SerializeField] private float radioArma;
    [SerializeField] private float danioArma;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float danioAtaque;
    [SerializeField] private float attackRange;

    [Header("FireBall")]
    public GameObject fireBall;
    public Transform controladorDisparo;
    public LayerMask capaJugador;
    public int numeroDisparos = 5;
    public float radioDisparo = 1.5f; // Radio de la "caja" en la que girarán los disparos
    public float tiempoGiro = 1.5f; // Tiempo que durarán girando antes de salir disparados

    [Header("Smash")]
    [SerializeField] private Transform smash;
    [SerializeField] private Vector3 dimensionesSmash;
    [SerializeField] private float danioSmash;

    [Header("FireBreath")]
    [SerializeField] private Transform fireBreath;
    [SerializeField] private Vector3 dimensionesFireBreath;
    [SerializeField] private float danioFireBreath;

    [Header("Movimiento")]
    [SerializeField] private float velocidad;

    private bool atacando = false;

    private Renderer renderer;
    public float distanciaJugador;
    private float tiempoEntreAtaques = 2.0f; // tiempo entre ataques
    private float tiempoProximoAtaque = 0f; // controla el tiempo de cada ataque
    private bool enAtaque = false; // controla si el jefe está en medio de un ataque

    public PanelFinalNivel nivelFinalPanel;

    [Header("Puntos Enemigo")]
    [SerializeField] private float cantidadPuntos;
    private PuntajeController puntaje;

    [Header("Monedas Enemigo")]
    [SerializeField] private float cantidadMonedas;
    private DineroController dinero;
    
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
        renderer = GetComponent<Renderer>();
        nivelFinalPanel = FindObjectOfType<PanelFinalNivel>();

        puntaje = FindObjectOfType<PuntajeController>();
        dinero = FindObjectOfType<DineroController>();

        if (puntaje == null)
        {
            Debug.LogError("No se encontró PuntajeController en la escena.");
        }

        if (dinero == null)
        {
            Debug.LogError("No se encontró DineroController en la escena.");
        }
    }

    private void FixedUpdate()
    {
        if (jugador == null) return;
        if (animator.GetBool("isWalking"))
        {
            float distanciaJugador = Vector2.Distance(transform.position, jugador.position);
            //Debug.Log("Distanciajugador: " + distanciaJugador);

            Vector2 direction = (jugador.position - transform.position).normalized;

            movement = new Vector2(direction.x, 0);

            if (distanciaJugador <= 6.5)
            {
                MirarJugador();

                movement = Vector2.zero;  // Detener el movimiento

                Atacar();
            }

            rb2D.MovePosition(rb2D.position + movement * velocidad * Time.deltaTime);
        }
    }

    public void Atacar()
    {
        atacando = true;
        animator.SetBool("isWalking", false);
        float[] ataques = { 0.33f, 0.33f, 0.33f };
        float attackIndex = Choose(ataques);
        Debug.Log("Atacar con: "+attackIndex);

        switch (attackIndex)
        {
            case 0.0f:
                animator.SetTrigger("KnifeAttack");
                break;
            case 1.0f:
                animator.SetTrigger("SmashAttack");
                break;
            case 2.0f:
                animator.SetTrigger("FireBreathAttack");
                break;
        }
    }

    public void DesactivarAtaque()
    {
        atacando = false;
    }

    public void TomarDanio(float danio)
    {
        vida -= danio;

        if (vida <= 0)
        {
            animator.SetTrigger("Muerte");
        }
    }

    private void Muerte()
    {
        puntaje.SumarPuntos(cantidadPuntos);
        dinero.SumarMonedas(cantidadMonedas);
        Destroy(gameObject);
        nivelFinalPanel.MostrarPanelFinal();
    }

    public void MirarJugador()
    {
        if ((jugador.position.x > transform.position.x && !mirandoDerecha) || (jugador.position.x < transform.position.x && mirandoDerecha))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }

    // Métodos para ejecutar los ataques después de la animación
    public void KnifeAttack()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorArma.position, radioArma);

        foreach (Collider2D collision in objetos)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<VidaController>().TomarDanio(danioArma);
            }
        }
        enAtaque = false; // permite otro ataque
    }

    public void FireBreathAttack()
    {
        // Lógica para el ataque especial 1
        Debug.Log("FireBreathAttack");
        enAtaque = false;
    }

    public void SpellAttack()
    {
        Debug.Log("SpellAttack");
        for (int i = 0; i < numeroDisparos; i++)
        {
            // Calcular una posición alrededor del controladorDisparo
            float angle = i * (360f / numeroDisparos); // Distribuir los disparos en un círculo
            Vector3 posicionInicial = controladorDisparo.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * radioDisparo;

            // Calcular una rotación adicional para cada FireBall
            Quaternion rotacionDisparo = controladorDisparo.rotation * Quaternion.Euler(0, 0, angle);

            // Instanciar la FireBall en la posición y rotación calculadas
            GameObject nuevaFireBall = Instantiate(fireBall, posicionInicial, rotacionDisparo);

            // Agregar una pequeña variación aleatoria en la velocidad o dirección para cada FireBall
            FireBall fireBallScript = nuevaFireBall.GetComponent<FireBall>();
            if (fireBallScript != null)
            {
                fireBallScript.velocidadMovimiento += Random.Range(-1f, 1f); // Variación en la velocidad
                fireBallScript.anguloInicial += Random.Range(-5f, 5f); // Variación en el ángulo inicial
            }
        }
        enAtaque = false;
    }
    public void SmashAttack()
    {
        Vector2 size = new Vector2(dimensionesSmash.x, dimensionesSmash.y);

        Collider2D[] objetos = Physics2D.OverlapBoxAll(smash.position, size, 0f);

        foreach (Collider2D collisionador in objetos)
        {
            if (collisionador.CompareTag("Player"))
            {
                Debug.Log("Smash Attack");
                collisionador.transform.GetComponent<VidaController>().TomarDanio(danioSmash);
            }
        }
        enAtaque = false; // permite otro ataque
    }

    float Choose(float[] probs)
    {
        float total = 0;
        foreach (float elem in probs)
            total += elem;

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
                return i;
            else
                randomPoint -= probs[i];
        }
        return probs.Length - 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorArma.position, radioArma);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(smash.position, dimensionesSmash);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(controladorDisparo.position, radioDisparo);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(fireBreath.position, dimensionesFireBreath);
    }
}