using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour, IDanio
{
    private Transform jugador;
    private Rigidbody2D rb2D;
    public Transform controladorDisparo;
    public float distanciaLinea = 5f; // Ajusta la distancia de detección
    public LayerMask capaJugador;
    public bool jugadorEnRango;

    private Animator animator;

    [Header("Vida")]
    [SerializeField] private float vida;

    [Header("Tiempos")]
    public float tiempoEntreDisparos = 2f;
    public float tiempoUltimoDisparo;
    public float tiempoEsperaDisparo = 0.5f;

    [Header("Deteccion")]
    [SerializeField] private float detectionRadius;

    [Header("Patrullaje")]
    [SerializeField] private Transform detectorSuelo;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float longitudRaycastSuelo = 2f;
    [SerializeField] private Transform detectorPared;
    [SerializeField] private float longitudRaycastPared = 0.7f;

    [Header("Movimiento")]
    [SerializeField] private float velocidad;
    private bool enMovimiento;
    private Vector2 movement;
    private bool estaDisparando = false; // Nueva variable para controlar el estado de disparo

    [Header("Bala")]
    public GameObject balaWizard;

    [Header("Control Daño")]
    private Renderer renderer;
    [SerializeField] private Color colorDaño = new Color(0.3098f, 0.0039f, 0f, 1f);
    [SerializeField] private float tiempoRestablecerColor = 0.2f;

    private bool estaMuerto = false;

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
        renderer = GetComponent<Renderer>();
        StartCoroutine(BuscarJugador(5f));
    }

    private void FixedUpdate()
    {
        if (jugador == null) return;
        if (estaMuerto || estaDisparando) return; // No moverse si está muerto o disparando

        float distanciaJugador = Vector2.Distance(transform.position, jugador.position);

        // Raycast para detectar al jugador en el rango de disparo
        RaycastHit2D hit = Physics2D.Raycast(controladorDisparo.position, transform.right * transform.localScale.x, distanciaLinea, capaJugador);
        jugadorEnRango = hit.collider != null;

        if (distanciaJugador < detectionRadius && !jugadorEnRango)
        {
            Vector2 direction = (jugador.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0);
            MirarJugador();
            enMovimiento = true;
            rb2D.MovePosition(rb2D.position + movement * velocidad * Time.deltaTime);
        }
        else if (jugadorEnRango)
        {
            enMovimiento = false;
            MirarJugador();
            if (Time.time > tiempoEntreDisparos + tiempoUltimoDisparo)
            {
                tiempoUltimoDisparo = Time.time;
                estaDisparando = true; // Inicia el disparo
                animator.SetTrigger("Disparar");
                Invoke(nameof(Disparar), tiempoEsperaDisparo);
            }
        }
        else
        {
            Patrullar();
            rb2D.MovePosition(rb2D.position + movement * velocidad * Time.deltaTime);
        }

        animator.SetBool("enMovimiento", enMovimiento);
    }

    private void Disparar()
    {
        // Instanciar la bala y asignarle la dirección correcta
        GameObject nuevaBala = Instantiate(balaWizard, controladorDisparo.position, controladorDisparo.rotation);
        Bala balaScript = nuevaBala.GetComponent<Bala>();

        // Configurar la dirección de la bala según la dirección en la que mira el mago
        int direccion = transform.localScale.x > 0 ? 1 : -1;
        balaScript.SetDireccion(direccion);

        // Finalizar el estado de disparo después de un breve retraso para sincronizar con la animación
        Invoke(nameof(FinalizarDisparo), tiempoEsperaDisparo);
    }

    private void FinalizarDisparo()
    {
        estaDisparando = false; // Retoma el movimiento después de disparar
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

    private void Muerte()
    {
        gameObject.SetActive(false);
    }

    //TODO: Control del patrullaje
    private void Patrullar()
    {
        if (estaDisparando) return; // Evitar patrullaje si está disparando

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorDisparo.position, controladorDisparo.position + transform.right * transform.localScale.x * distanciaLinea);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(detectorSuelo.position, detectorSuelo.position + Vector3.down * longitudRaycastSuelo);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(detectorPared.position, detectorPared.position + Vector3.right * transform.localScale.x * longitudRaycastPared);
    }
}
