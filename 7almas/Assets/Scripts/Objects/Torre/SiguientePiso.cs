using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiguientePiso : MonoBehaviour
{
    // La posición a la que se teletransportará el personaje
    public Vector3 teleportDestination;

    // La tecla que debe presionar el jugador para interactuar
    public KeyCode interactionKey = KeyCode.E;

    // Verificamos si el jugador está dentro del área de interacción
    private bool isPlayerInRange = false;

    // Referencia al jugador
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            TeleportPlayer();
        }
    }

    // Este método teletransporta al jugador
    private void TeleportPlayer()
    {
        if (player != null)
        {
            teleportDestination.y += 17.141f;
            player.transform.position = teleportDestination;
        }
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
