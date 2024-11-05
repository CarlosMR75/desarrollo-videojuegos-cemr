using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaController : MonoBehaviour
{
    private Animator animator;

    [Header("Vida")]
    [SerializeField] private float vida;
    [SerializeField] private float vidaMaxima = 100f;
    [SerializeField] private BarraDeVida barraDeVida;

    private void Start()
    {
        //vida = vidaMaxima;
        animator = GetComponent<Animator>();
        barraDeVida.InicializarBarraDeVida(vida);
        Debug.Log("Vida:" + vida);
    }

    public void TomarDanio(float danio)
    {
        vida -= danio;
        barraDeVida.CambiarVidaActual(vida);
        if(vida <= 0)
        {
            animator.SetTrigger("Muerte");
            //Destroy(gameObject);
        }

    }
}
