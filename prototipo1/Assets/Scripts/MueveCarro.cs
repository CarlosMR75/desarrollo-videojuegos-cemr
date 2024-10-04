using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MueveCarro : MonoBehaviour
{
    public float moveSpeed = 5f;  // Velocidad de movimiento
    public float turnSpeed = 100f; // Velocidad de rotaci�n
    public float nitroDuration = 2f; // Duraci�n del nitro en segundos
    private bool nitroActive = false; // Controla si el nitro est� activo
    private float nitroTimeRemaining; // Tiempo restante de nitro

    void Start()
    {
        nitroTimeRemaining = nitroDuration; // Inicializa el tiempo del nitro
    }

    void Update()
    {
        // Movimiento hacia adelante y atr�s
        float moveDirection = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) && !nitroActive)
        {
            nitroActive = true;
            nitroTimeRemaining = nitroDuration;
        }

        if (nitroActive)
        {
            moveSpeed = 30f; // Incrementa temporalmente la velocidad

            nitroTimeRemaining -= Time.deltaTime; // Decrementa el tiempo restante

            if (nitroTimeRemaining <= 0)
            {
                nitroActive = false; // Desactiva el nitro
                moveSpeed = 5f; // Restablece la velocidad normal
            }
        }

        // Mueve el carro
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * moveDirection);

        // Rotaci�n c
        float turnDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * turnDirection);
    }
}
