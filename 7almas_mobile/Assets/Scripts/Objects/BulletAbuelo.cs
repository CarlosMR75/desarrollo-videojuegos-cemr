using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAbuelo : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Desaparecer")]
    [SerializeField] private float tiempoVida = 0.5f;
    private void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

}
