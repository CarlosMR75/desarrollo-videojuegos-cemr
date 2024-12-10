using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAttack : MonoBehaviour
{
    [SerializeField] private float danio;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("El jugador recibe daño por Rayo");
            collision.gameObject.GetComponent<VidaController>().TomarDanio(danio);
        }
    }
}
