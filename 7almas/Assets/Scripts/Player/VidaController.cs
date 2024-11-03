using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();       
    }

    [SerializeField] private float vida;

    public void TomarDanio(float danio)
    {
        vida -= danio;

        if(vida <= 0)
        {
            animator.SetTrigger("Muerte");
            //Destroy(gameObject);
        }

    }
}
