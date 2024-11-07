using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateController : MonoBehaviour
{
    [Header("ControladorAtaque")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float danioGolpe;

    [Header("Animation")]
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Ataque Principal"))
        {
            Golpe();
        }        
    }

    private void Golpe()
    {
        animator.SetTrigger("Golpe");

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D collisionador in objetos)
        {
            IDanio objeto = collisionador.GetComponent<IDanio>();
            if(objeto != null)
            {
                objeto.TomarDanio(danioGolpe);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);    
    }
}
