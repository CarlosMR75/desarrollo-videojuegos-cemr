using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidadMovimiento;
    public float danioBala;

    private void Update()
    {
        transform.Translate(Time.deltaTime * velocidadMovimiento * Vector2.right);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out VidaController vida))
        {
            vida.TomarDanio(danioBala);
            Debug.Log("LE PEGUÉ");
            Destroy(gameObject);
        }
    }
}
