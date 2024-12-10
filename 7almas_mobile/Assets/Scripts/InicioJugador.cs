using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InicioJugador : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Asigna aquí tu Cinemachine Virtual Camera

    private void Start() {
        // Obtén el índice del jugador desde PlayerPrefs
        int indexJugador = PlayerPrefs.GetInt("JugadorIndex");

        // Instancia el personaje en la posición del objeto InicioJugador
        GameObject jugadorInstanciado = Instantiate(
            GameManagerSeleccionarPesonaje.Instance.personajes[indexJugador].personajeJugable, 
            transform.position, 
            Quaternion.identity
        );

        // Asigna el personaje instanciado como el objetivo de la Cinemachine Virtual Camera
        virtualCamera.Follow = jugadorInstanciado.transform;
    }
}
