using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlCalidad : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int calidad;

    private void Start()
    {
        calidad = PlayerPrefs.GetInt("numeroDeCalidad", 5);
        dropdown.value = calidad;
        AjustarCalidad();
    }

    private void Update()
    {

    }

    public void AjustarCalidad()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("numeroDeCalidad", dropdown.value);
        calidad = dropdown.value;
    }
}