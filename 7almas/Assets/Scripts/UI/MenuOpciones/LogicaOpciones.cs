using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaOpciones : MonoBehaviour
{
    public ControladorOpciones panelOpciones;
    private bool juegoPausado = false;

    void Start()
    {
        panelOpciones = GameObject.FindGameObjectWithTag("Opciones").GetComponent<ControladorOpciones>();
    }

    private void Update()
    {
        juegoPausado = panelOpciones.pantallaOpciones.activeSelf;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("juegoPausado: " + juegoPausado);
            if (!juegoPausado)
            {
                MostrarOpciones();
            }
            else
            {
                Reanudar();
            }
        }
    }
    public void MostrarOpciones()
    {
        Time.timeScale = 0f;
        panelOpciones.pantallaOpciones.SetActive(true);
    }

    public void Reanudar()
    {
        panelOpciones.pantallaOpciones.SetActive(false);
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }
}
