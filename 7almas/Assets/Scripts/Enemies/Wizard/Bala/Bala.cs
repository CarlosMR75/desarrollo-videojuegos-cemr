using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidadMovimiento;
    public float danioBala;

    [Header("Desaparecer")]
    [SerializeField] private float tiempoVida = 3f;

    private int direccion; // Nueva variable para la dirección de la bala

    public void SetDireccion(int direccion)
    {
        this.direccion = direccion;
    }

    private void Update()
    {
        Invoke(nameof(Destruir), tiempoVida);

        // Mover la bala en la dirección correcta
        transform.Translate(Time.deltaTime * velocidadMovimiento * Vector2.right * direccion);
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

    private void Destruir()
    {
        Destroy(gameObject);
    }
}
