using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguBoss : MonoBehaviour, IDanio
{
    private Animator animator;
    public Rigidbody2D rb2D;
    public Transform jugador;
    public bool mirandoDerecha;
    private Vector2 movement;

    [Header("Vida")]
    [SerializeField] private float vida;
    //Bara de vida

    [Header("AttackPeck")]
    [SerializeField] private Transform peck;
    [SerializeField] private Vector3 dimensionesPeck;
    [SerializeField] private float danioPeck;

    [Header("AttackRay")]
    [SerializeField] private Transform rayo;
    [SerializeField] private Vector3 dimensionesRayo;
    [SerializeField] private float danioRayo;

    [Header("Movimiento")]
    [SerializeField] private float velocidad;

    private bool atacando = false;

    private Renderer renderer;
    public float distanciaJugador;
    private float tiempoEntreAtaques = 2.0f; // tiempo entre ataques
    private float tiempoProximoAtaque = 0f; // controla el tiempo de cada ataque
    private bool enAtaque = false; // controla si el jefe está en medio de un ataque

    // Referencia al objeto con otro Animator
    [Header("AnimatorSpikes")]
    [SerializeField] private GameObject spikes;
    private Animator spikesAnimator;

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
        if (spikes != null)
        {
            spikesAnimator = spikes.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("No se asignó el objeto con otro Animator.");
        }
        
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

        nivelFinalPanel = FindObjectOfType<PanelFinalNivel>();
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
        MirarJugador();
        float[] ataques = { 0.5f, 0.5f };
        float attackIndex = Choose(ataques);
        Debug.Log("Atacar con: " + attackIndex);

        switch (attackIndex)
        {
            case 0.0f:
                animator.SetTrigger("AttackPeck");
                break;
            case 1.0f:
                animator.SetTrigger("AttackRay");
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
    public void AttackRay()
    {
        // Lógica para el ataque especial 1
        Debug.Log("rayoAttack");
        enAtaque = false;
    }

    public void AttackPeck()
    {
        if (spikesAnimator != null)
        {
            spikesAnimator.SetTrigger("Spawn");
        }

        Vector2 size = new Vector2(dimensionesPeck.x, dimensionesPeck.y);

        Collider2D[] objetos = Physics2D.OverlapBoxAll(peck.position, size, 0f);

        foreach (Collider2D collisionador in objetos)
        {
            if (collisionador.CompareTag("Player"))
            {
                Debug.Log("peck Attack");
                collisionador.transform.GetComponent<VidaController>().TomarDanio(danioPeck);
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(peck.position, dimensionesPeck);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(rayo.position, dimensionesRayo);
    }
}
