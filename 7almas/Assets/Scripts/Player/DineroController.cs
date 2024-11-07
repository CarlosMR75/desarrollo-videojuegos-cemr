using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DineroController : MonoBehaviour
{
    private float monedas = 0f;
    private TextMeshProUGUI textMesh;

    private void Start() {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = monedas.ToString("0");
    }

    private void Update() {
        // monedas += Time.deltaTime;
        // textMesh.text = monedas.ToString("0");
    }

    public void SumarMonedas(float monedasEntrada)
    {
        monedas += monedasEntrada;
        textMesh.text = monedas.ToString("0");
    }
}
