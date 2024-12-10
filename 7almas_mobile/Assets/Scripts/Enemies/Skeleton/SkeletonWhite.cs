using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWhite : MonoBehaviour, IDanio
{
    
    [SerializeField] private float vida;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TomarDanio(float danio)
    {
        Destroy(gameObject);// Con minús se refiere al que pertenece
        Muerte();
    }

    private void Muerte()
    {
        animator.SetTrigger("Muerte");  
    }

    void Update()
    {
        
    }
}
