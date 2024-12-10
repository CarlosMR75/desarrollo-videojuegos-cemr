using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pengu_Boss_Idle_Behaviour : StateMachineBehaviour
{
    private PenguBoss penguBoss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        penguBoss = animator.GetComponent<PenguBoss>();
        Debug.Log("INICIO IDLE");
        float[] estados = { 0.4f, 0.6f };
        float stateIndex = Choose(estados);
        Debug.Log("Estado seleccionado: " + stateIndex);
        switch (stateIndex)
        {
            case 0.0f:
                animator.SetTrigger("AttackIce");
                penguBoss.MirarJugador();
                break;
            case 1.0f:
                animator.SetBool("isWalking", true);
                penguBoss.MirarJugador();
                break;
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

        float Choose(float[] probs)
    {
        float total = 0;
        foreach (float elem in probs)
            total += elem;

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
                return i;
            else
                randomPoint -= probs[i];
        }
        return probs.Length - 1;
    }
}
