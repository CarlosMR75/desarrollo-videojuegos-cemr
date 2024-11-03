using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Boss_Idle_Behaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("INICIO IDLE");
        float[] estados = { 0.2f, 0.25f, 0.55f };
        float stateIndex = Choose(estados);

        switch (stateIndex)
        {
            case 0.0f:
                animator.SetTrigger("SpellHandAttack");
                break;
            case 1.0f:
                animator.SetTrigger("SpellAttack");
                break;
            case 2.0f:
                animator.SetBool("isWalking", true);
                break;
        }
    }

    // Reiniciar la bandera al salir del estado
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

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
