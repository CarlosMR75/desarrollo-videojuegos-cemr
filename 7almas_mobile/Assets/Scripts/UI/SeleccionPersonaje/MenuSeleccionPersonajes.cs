using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuSeleccionPersonajes : MonoBehaviour
{
    private int index;
    [SerializeField] private Image imagen;
    [SerializeField] private TextMeshProUGUI nombre;
    private GameManagerSeleccionarPesonaje gameManagerSeleccionarPesonaje;

    private void Start() {
        gameManagerSeleccionarPesonaje = GameManagerSeleccionarPesonaje.Instance;

        index = PlayerPrefs.GetInt("JugadorIndex");

        if(index > gameManagerSeleccionarPesonaje.personajes.Count - 1)
        {
            index = 0;
        }

        CambiarPantalla();
    }

    private void CambiarPantalla()
    {
        PlayerPrefs.SetInt("JugadorIndex", index);
        imagen.sprite = gameManagerSeleccionarPesonaje.personajes[index].imagen;
        nombre.text = gameManagerSeleccionarPesonaje.personajes[index].nombre;
    }

    public void SiguientePersonaje()
    {
        if(index == gameManagerSeleccionarPesonaje.personajes.Count - 1)
        {
            index = 0;
        }
        else
        {
            index += 1;
        }

        CambiarPantalla();
    }

    public void AnteriorPersonaje()
    {
        if(index == 0)
        {
            index = gameManagerSeleccionarPesonaje.personajes.Count - 1;
        }
        else
        {
            index -= 1;
        }

        CambiarPantalla();
    }

    public void IniciarJuego()
    {
        SceneManager.LoadScene("NivelPrototipo");
    }
}
