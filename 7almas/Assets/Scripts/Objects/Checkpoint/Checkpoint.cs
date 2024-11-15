using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    // La tecla que debe presionar el jugador para interactuar
    public KeyCode interactionKey = KeyCode.E;

    public PanelFinalNivel nivelFinalPanel;

    // Verificamos si el jugador está dentro del área de interacción
    private bool isPlayerInRange = false;

    // Referencia al jugador
    private Transform jugador;
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuscarJugador(5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (jugador == null) return;
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            NextLevel();
        }
    }
    private void NextLevel()
    {
        nivelFinalPanel.MostrarPanelFinal();
        //SceneManager.LoadScene("NivelMina");
        Debug.Log("Siguiente Nivel");
    }

    // Detecta si el jugador entra en el área de interacción
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // Detecta si el jugador sale del área de interacción
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            //player = null;
        }
    }
}
