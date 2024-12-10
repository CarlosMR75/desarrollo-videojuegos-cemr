using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pengu_Boss_AttackIce_Behaviour : StateMachineBehaviour
{
    [SerializeField] private GameObject habilidad;
    [SerializeField] private float offsetY; // Desplazamiento en Y
    [SerializeField] private float spacingX = 1f; // Espaciado entre instancias en X
    private PenguBoss penguBoss;
    private Transform jugador;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        penguBoss = animator.GetComponent<PenguBoss>();
        jugador = penguBoss.jugador;

        penguBoss.MirarJugador();

        // Obtener la posición inicial de spawn basada en el jugador
        Vector2 posicionBase = new Vector2(jugador.position.x, jugador.position.y + offsetY);

        // Instanciar 3 objetos en fila
        for (int i = -1; i <= 1; i++)
        {
            Vector2 posicionSpawn = posicionBase + new Vector2(i * spacingX, 0);
            Instantiate(habilidad, posicionSpawn, Quaternion.identity);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {

    // }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
        
    // }
}
