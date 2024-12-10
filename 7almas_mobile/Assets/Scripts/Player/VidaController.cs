using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaController : MonoBehaviour
{
    private Animator animator;

    [Header("Vida")]
    [SerializeField] private float vida;
    [SerializeField] private float vidaMaxima = 100f;
    private BarraDeVida barraDeVida;

    public PanelMuerte panelMuerte;

    private void Start()
    {
        // Obtener la referencia de la BarraDeVida del canvas de la escena actual
        barraDeVida = FindObjectOfType<BarraDeVida>();
        panelMuerte = FindObjectOfType<PanelMuerte>();

        if (barraDeVida != null)
        {
            vida = vidaMaxima;
            animator = GetComponent<Animator>();
            barraDeVida.InicializarBarraDeVida(vida);
            Debug.Log("Vida:" + vida);
        }
        else
        {
            Debug.LogError("No se encontró BarraDeVida en la escena. Asegúrate de que esté presente en el canvas.");
        }
    }

    public void TomarDanio(float danio)
    {
        vida -= danio;
        if (barraDeVida != null)
        {
            barraDeVida.CambiarVidaActual(vida);
        }

        if (vida <= 0)
        {
            animator.SetTrigger("Muerte");
            //Destroy(gameObject);
        }
    }

    public void Morir()
    {
        panelMuerte.MostrarPanelMuerte();
        gameObject.SetActive(false);
    }
}
