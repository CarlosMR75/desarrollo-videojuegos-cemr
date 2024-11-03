using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player01Controller : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private float inputX;
    private float inputY;
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDemovimiento = 0f;
    [Range(0, 0.3f)][SerializeField] private float suavizadoDemovimiento = 0f;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;

    [SerializeField] private Vector3 dimensionesCaja;

    private bool enSuelo;

    private bool salto = false;

    [Header("Animation")]
    private Animator animator;

    [Header("Salto Doble")]
    [SerializeField] private int saltosExtraRestantes;
    [SerializeField] private int saltosExtra;

    private bool saltoDoble = false;

    [Header("SaltoPared")]
    [SerializeField] private Transform controladorPared;

    [SerializeField] private Vector3 dimensionesCajaPared;

    private bool enPared;

    [Header("Deslizar")]
    private bool deslizando;

    [SerializeField] private float velocidadDeslizar;
    [SerializeField] private float fuerzaSaltoParedX;
    [SerializeField] private float fuerzaSaltoParedY;
    [SerializeField] private float tirmpoSaltoPared;

    private bool sePuedeMover = true;

    [Header("Dash")]
    [SerializeField] private float velocidadDash;
    [SerializeField] private float tiempoDash;
    private float gravedadInicial;
    private bool puedeHacerDash;

    [Header("GroundSlam")]
    [SerializeField] private float velocidadGolpeSuelo;
    [SerializeField] private float maxVelocidadGolpeSuelo;
    [SerializeField] private float radioGolpe;
    private bool isSlammed = false;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravedadInicial = rb2D.gravityScale;
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        //Debug.Log("InputY: "+inputY);

        movimientoHorizontal = inputX * velocidadDemovimiento;

        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));

        animator.SetBool("Deslizando", deslizando);

        if (enSuelo)
        {
            saltosExtraRestantes = saltosExtra;
            animator.SetBool("saltoDoble", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            salto = true;
        }

        if (!enSuelo && enPared && inputX != 0)
        {
            deslizando = true;
        }
        else
        {
            deslizando = false;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            StartCoroutine(Dash());
        }

        if (inputY == -1 && !enSuelo)
        {
            EjecutarGroundSlam();
        }
    }

    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        animator.SetBool("enSuelo", enSuelo);

        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, queEsSuelo);

        animator.SetBool("enPared", enPared);

        if (enSuelo && isSlammed)
        {
            isSlammed = false;
            animator.SetBool("isSlammed", isSlammed);
        }

        //Mover      
        Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);

        salto = false;

        if (deslizando)
        {
            //Debug.Log("Estoy en la pared");
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -velocidadDeslizar, float.MaxValue));
        }
    }

    private void Mover(float mover, bool saltar)
    {
        if (sePuedeMover)
        {
            Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDemovimiento);
        }

        if (mover > 0 && !mirandoDerecha)
        {
            //Girar
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        if (saltar)
        {
            if (enSuelo)
            {
                Salto();
            }
            else if (enPared)
            {
                SaltoPared();
            }
            else
            {
                if (saltar && saltosExtraRestantes > 0)
                {
                    Salto();
                    saltosExtraRestantes -= 1;
                    saltoDoble = true;
                    animator.SetBool("saltoDoble", true);
                }
            }
        }

        saltoDoble = false;
    }

    private void Salto()
    {
        //Debug.Log("Trate de saltar");
        //Debug.Log("En Pared: "+ enPared);
        if (!enPared)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, fuerzaSalto);
            enSuelo = false;
            //rb2D.AddForce(new Vector2(0f, fuerzaSalto));
        }
    }

    private void SaltoPared()
    {
        // Saltar hacia el lado opuesto de la dirección actual
        if (mirandoDerecha)
        {
            rb2D.velocity = new Vector2(-fuerzaSaltoParedX, fuerzaSaltoParedY);
            Girar();
        }
        else
        {
            rb2D.velocity = new Vector2(fuerzaSaltoParedX, fuerzaSaltoParedY);
            Girar();
        }

        enPared = false;

        StartCoroutine(CambioSaltoPared());
    }

    IEnumerator CambioSaltoPared()
    {
        sePuedeMover = false;
        yield return new WaitForSeconds(tirmpoSaltoPared);
        sePuedeMover = true;
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private IEnumerator Dash()
    {
        //Debug.Log("Dash ...");
        sePuedeMover = false;
        puedeHacerDash = false;
        animator.SetBool("Dash", true);
        rb2D.gravityScale = 0;
        rb2D.velocity = new Vector2(velocidadDash * transform.localScale.x, 0);
        yield return new WaitForSeconds(tiempoDash);
        sePuedeMover = true;
        puedeHacerDash = true;
        animator.SetBool("Dash", false);
        rb2D.gravityScale = gravedadInicial;
    }

    private void EjecutarGroundSlam()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Max(rb2D.velocity.y - velocidadGolpeSuelo * Time.deltaTime, -maxVelocidadGolpeSuelo));
        isSlammed = true;
        animator.SetBool("isSlammed", isSlammed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
        Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
    }
}
