using UnityEngine;

public class FireBreath : MonoBehaviour
{
    [SerializeField] private float danioFireBreath = 30f;
    private Animator animator; // Para verificar la animación del jefe

    private void Start()
    {
        // Obtener el Animator del JefeFinalBossDemon desde el objeto al que está adjunto el FireBreath
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el jefe está en la animación "FireBreathAttack"
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("demon_attack_fire_breath"))
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("El jugador recibe daño por FireBreath");
                other.GetComponent<VidaController>().TomarDanio(danioFireBreath);
            }
        }
    }
}
