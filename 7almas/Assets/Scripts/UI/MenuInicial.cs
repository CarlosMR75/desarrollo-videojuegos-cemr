using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void Jugar() {
        SceneManager.LoadScene("SampleScene");//SceneManager.GetActiveScene().buildIndex + 1 -- Para la siguiente escena
    }

    public void Opciones()
    {
        //Debug.Log("Opciones ...");
    }

    public void Salir()
    {
        //Debug.Log("Salir ...");
        Application.Quit();
    }
}
