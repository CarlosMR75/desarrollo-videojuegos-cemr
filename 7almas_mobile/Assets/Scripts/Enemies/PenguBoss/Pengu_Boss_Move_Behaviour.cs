using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pengu_Boss_Move_Behaviour : StateMachineBehaviour
{
    private PenguBoss penguBoss;
    private Rigidbody2D rb2D;
    public int selectedAttackIndex;
    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 5f;
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        penguBoss = animator.GetComponent<PenguBoss>();
        rb2D = penguBoss.rb2D;

        penguBoss.MirarJugador();
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2D.velocity = new Vector2(velocidadMovimiento, rb2D.velocity.y) * -animator.transform.right;
        penguBoss.MirarJugador();
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
    }

    float Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
