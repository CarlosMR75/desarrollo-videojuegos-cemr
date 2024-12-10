using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelMuerte : MonoBehaviour
{
    public PuntajeController puntaje; // Referencia al PuntajeController
    public GameObject panelMuerte; // Panel de fin del nivel
    public TextMeshProUGUI textoPuntuacion; // Texto para mostrar la puntuación final
    public TextMeshProUGUI textoTiempo; // Texto para mostrar el tiempo

    private void Start()
    {
        puntaje = FindObjectOfType<PuntajeController>();
        panelMuerte.SetActive(false); // Asegúrate de que el panel esté oculto al inicio
    }

    public void MostrarPanelMuerte()
    {
        // Obtiene los puntos y el tiempo del PuntajeController
        float puntos = puntaje.GetPuntos();
        float tiempo = puntaje.GetTiempo();

        // Muestra los datos en el panel
        textoPuntuacion.text = "Puntuación: " + puntos.ToString("0");
        textoTiempo.text = "Tiempo: " + tiempo.ToString("0.0") + " s";

        panelMuerte.SetActive(true); // Muestra el panel
    }
}
