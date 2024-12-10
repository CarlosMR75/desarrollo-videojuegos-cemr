using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LogicaOpciones : MonoBehaviour
{
    public ControladorOpciones panelOpciones;
    private bool juegoPausado = false;

    private Entradas entradas; // Refiere a las entradas del Nuevo Input System
    private bool accionEnProceso = false; // Variable para evitar cambios rápidos

    void Start()
    {
        panelOpciones = GameObject.FindGameObjectWithTag("Opciones").GetComponent<ControladorOpciones>();
        entradas.Gameplay.Pausar.performed += context => AlternarPausa(); // Mapeo de la acción de pausar
    }

    private void Awake()
    {
        entradas = new Entradas();
    }


    private void OnEnable()
    {
        entradas.Enable(); // Activamos las entradas
    }

    private void OnDisable()
    {
        if (entradas != null) // Verificar que 'entradas' esté inicializado
        {
            entradas.Disable();
        }
    }

    private void Update()
    {
        juegoPausado = panelOpciones.pantallaOpciones.activeSelf;
    }

    private void AlternarPausa()
    {
        if (!accionEnProceso) // Solo si no hay una acción en proceso
        {
            accionEnProceso = true; // Marcar que la acción está en proceso

            if (!juegoPausado)
            {
                MostrarOpciones();
            }
            else
            {
                Reanudar();
            }

            // Desactivar la acción temporalmente y volver a activarla después de un breve delay
            StartCoroutine(EsperarParaReactivarAccion());
        }
    }

    private IEnumerator EsperarParaReactivarAccion()
    {
        yield return new WaitForSeconds(0.2f); // Esperar 0.2 segundos antes de permitir otra acción
        accionEnProceso = false; // Reactivar la capacidad de alternar el estado de pausa
    }

    public void MostrarOpciones()
    {
        Time.timeScale = 0f; // Pausa el juego
        panelOpciones.pantallaOpciones.SetActive(true); // Muestra el panel de opciones
    }

    public void Reanudar()
    {
        panelOpciones.pantallaOpciones.SetActive(false); // Oculta el panel de opciones
        Time.timeScale = 1f; // Reanuda el juego
    }
}
