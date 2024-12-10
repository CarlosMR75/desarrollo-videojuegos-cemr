using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Animator animator;
    public Transform transformJugador;
    [Header("Movimiento")]
    public float velocidadMovimiento;
    public float anguloInicial;
    public float velocidadRotacion;
    [SerializeField] private float danioAtaque;

    [Header("Desaparecer")]
    [SerializeField] private float tiempoVida = 3f;
    private bool haColisionado = false;

    private void Start()
    {
        Invoke(nameof(Explotar), tiempoVida);

        animator = GetComponent<Animator>();
        GameObject jugadorGameObject = GameObject.FindGameObjectWithTag("Player");

        if (jugadorGameObject == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transformJugador = jugadorGameObject.transform;
        }
    }

    private void Update()
    {
        if (haColisionado) return;

        transform.Translate(velocidadMovimiento * Time.deltaTime * Vector2.right, Space.Self);

        if (transformJugador == null) return;

        float anguloRadianes = Mathf.Atan2(transformJugador.position.y - transform.position.y, transformJugador.position.x - transform.position.x);

        float anguloGrados = 180 / Mathf.PI * anguloRadianes - anguloInicial;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, anguloGrados), Time.deltaTime * velocidadRotacion);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out VidaController vida))
        {
            vida.TomarDanio(danioAtaque);
            Debug.Log("LE PEGUÉ");
            Explotar();
            haColisionado = true;
        }
    }

    private void Explotar()
    {
        animator.SetTrigger("Explode");
    }

    private void Destruir()
    {
        Destroy(gameObject);
    }
}
