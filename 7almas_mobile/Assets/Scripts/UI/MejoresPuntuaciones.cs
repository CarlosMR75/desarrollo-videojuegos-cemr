using UnityEngine;
using TMPro;

public class MejoresPuntuaciones : MonoBehaviour
{
    public TextMeshProUGUI[] mejoresPuntuacionesTextos; // Array de textos en el UI para mostrar cada puntaje
    [SerializeField] private int nivelActual; // Nivel actual configurado en el Inspector

    private void Start()
    {
        MostrarMejoresPuntuaciones();
    }

    public void GuardarNuevaPuntuacion(float nuevaPuntuacion)
    {
        // Carga las puntuaciones actuales para el nivel especificado en el Inspector
        float[] puntuaciones = new float[5];
        for (int i = 0; i < 5; i++)
        {
            puntuaciones[i] = PlayerPrefs.GetFloat("PuntuacionNivel" + nivelActual + "_" + i, 0);
        }

        // Verifica si la nueva puntuación es una de las 5 mejores para el nivel
        for (int i = 0; i < 5; i++)
        {
            if (nuevaPuntuacion > puntuaciones[i])
            {
                // Inserta la nueva puntuación en el lugar correcto y desplaza las demás
                for (int j = 4; j > i; j--)
                {
                    puntuaciones[j] = puntuaciones[j - 1];
                }
                puntuaciones[i] = nuevaPuntuacion;
                break;
            }
        }

        // Guarda las puntuaciones actualizadas en PlayerPrefs para el nivel dado
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetFloat("PuntuacionNivel" + nivelActual + "_" + i, puntuaciones[i]);
        }
        
        PlayerPrefs.Save(); // Guarda los cambios

        // Actualiza el UI para el nivel actual
        MostrarMejoresPuntuaciones();
    }

    public void MostrarMejoresPuntuaciones()
    {
        for (int i = 0; i < 5; i++)
        {
            float puntuacion = PlayerPrefs.GetFloat("PuntuacionNivel" + nivelActual + "_" + i, 0);
            mejoresPuntuacionesTextos[i].text = "" + (i + 1) + ".- " + puntuacion.ToString("0");
        }
    }
}
