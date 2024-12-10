using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAttack : MonoBehaviour
{
    [SerializeField] private float danio;
    [SerializeField] private float tiempoDeVida;

    private void Start()
    {
        // Programar la destrucción del objeto junto con su padre si existe.
        Invoke(nameof(DestroySelfAndParent), tiempoDeVida);
    }

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador recibe daño por Ice Cube");
            other.GetComponent<VidaController>().TomarDanio(danio);
        }
        // Verificar si el jefe está en la animación "FireBreathAttack"
        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("demon_attack_fire_breath"))
        // {

        // }
    }

    private void DestroySelfAndParent()
    {
        // Destruir el padre si existe
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        // Destruir este objeto
        Destroy(gameObject);
    }
}
