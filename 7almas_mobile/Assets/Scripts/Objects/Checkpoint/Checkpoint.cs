using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [Header("Configuración")]
    public PanelFinalNivel nivelFinalPanel;

    private bool isPlayerInRange = false;
    private Transform jugador;

    // Referencia al esquema de entradas
    private Entradas entradas;

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

    private void Awake()
    {
        // Inicializamos el esquema de entradas
        entradas = new Entradas();
    }

    private void OnEnable()
    {
        // Habilitamos el esquema y registramos el evento de interacción
        entradas.Enable();
        entradas.Gameplay.Interactuar.performed += OnInteract;
    }

    private void OnDisable()
    {
        // Deshabilitamos el esquema y removemos el evento
        entradas.Gameplay.Interactuar.performed -= OnInteract;
        entradas.Disable();
    }

    private void Start()
    {
        StartCoroutine(BuscarJugador(5f));
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerInRange)
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        nivelFinalPanel.MostrarPanelFinal();
        Debug.Log("Siguiente Nivel");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
