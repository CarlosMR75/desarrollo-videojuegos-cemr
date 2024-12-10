using System.Collections;
using UnityEngine;
using TMPro;

public class PuntajeController : MonoBehaviour
{
    private float puntos;
    private float tiempoTranscurrido;
    private TextMeshProUGUI textMesh;
    private float multiplicador;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        tiempoTranscurrido = 0;
        multiplicador = 10f;
    }

    private void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        // Lógica para reducir el multiplicador conforme pasa el tiempo (minimo 1)
        multiplicador = Mathf.Max(1f, 10f - (tiempoTranscurrido / 10f)); // LLega a 1 a los 90 segs
    }

    public void SumarPuntos(float puntosEntrada)
    {
        float puntosConMultiplicador = puntosEntrada * multiplicador; // Aplicar multiplicador
        puntos += puntosConMultiplicador;
        textMesh.text = puntos.ToString("0");
    }

    public float GetPuntos()
    {
        return puntos;
    }

    public float GetTiempo()
    {
        return tiempoTranscurrido;
    }
}
