using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelFinalNivel : MonoBehaviour
{
    public PuntajeController puntaje; // Referencia al PuntajeController
    public GameObject panelFinal; // Panel de fin del nivel
    public TextMeshProUGUI textoPuntuacion; // Texto para mostrar la puntuación final
    public TextMeshProUGUI textoTiempo; // Texto para mostrar el tiempo
    public MejoresPuntuaciones mejoresPuntuaciones;

    private void Start()
    {
        puntaje = FindObjectOfType<PuntajeController>();
        mejoresPuntuaciones = FindObjectOfType<MejoresPuntuaciones>();
        panelFinal.SetActive(false); // Asegúrate de que el panel esté oculto al inicio
    }

    public void MostrarPanelFinal()
    {
        // Obtiene los puntos y el tiempo del PuntajeController
        float puntos = puntaje.GetPuntos();
        float tiempo = puntaje.GetTiempo();

        // Muestra los datos en el panel
        textoPuntuacion.text = "Puntuación: " + puntos.ToString("0");
        textoTiempo.text = "Tiempo: " + tiempo.ToString("0.0") + " s";

        mejoresPuntuaciones.GuardarNuevaPuntuacion(puntos);

        panelFinal.SetActive(true); // Muestra el panel
    }
}
