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
        Debug.Log("Estado seleccionado: " + stateIndex);

        switch (stateIndex)
        {
            case 0.0f:
                animator.SetTrigger("SpellHandAttack");
//                Debug.Log("Trigger SpellHandAttack activado");
                break;
            case 1.0f:
                animator.SetTrigger("SpellAttack");
                Debug.Log("Trigger SpellAttack activado");
                break;
            case 2.0f:
                animator.SetBool("isWalking", true);
                Debug.Log("isWalking activado");
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

// ? Solución usada por si el jefe se queda atrapado en Idle
// ? Añadir transiciones de Idle a cada ataque Meele como en los de a Rango

// ! Solución por si el jefe se queda atrapado en Idle v2.0
// ! Se añade un temporizador para que no exceda el tiempo
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Demon_Boss_Idle_Behaviour : StateMachineBehaviour
// {
//     public float idleTimeLimit = 0.5f; // Tiempo máximo permitido en idle
//     private float idleTime = 0f; // Contador de tiempo en idle

//     override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         Debug.Log("INICIO IDLE");

//         // Reiniciar el contador al entrar en el estado idle
//         idleTime = 0f;

//         // Elegir una acción aleatoria
//         float[] estados = { 0.2f, 0.25f, 0.55f };
//         float stateIndex = Choose(estados);

//         switch (stateIndex)
//         {
//             case 0.0f:
//                 animator.SetTrigger("SpellHandAttack");
//                 break;
//             case 1.0f:
//                 animator.SetTrigger("SpellAttack");
//                 break;
//             case 2.0f:
//                 animator.SetBool("isWalking", true);
//                 break;
//         }
//     }

//     override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         // Aumentar el contador de tiempo en idle
//         idleTime += Time.deltaTime;

//         // Verificar si el tiempo en idle ha excedido el límite
//         if (idleTime >= idleTimeLimit)
//         {
//             Debug.Log("El jefe está atrapado en idle, reiniciando estado.");

            
//             animator.SetTrigger("SpellAttack");
//         }
//     }

//     override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         idleTime = 0f;
//     }

//     float Choose(float[] probs)
//     {
//         float total = 0;
//         foreach (float elem in probs)
//             total += elem;

//         float randomPoint = Random.value * total;

//         for (int i = 0; i < probs.Length; i++)
//         {
//             if (randomPoint < probs[i])
//                 return i;
//             else
//                 randomPoint -= probs[i];
//         }
//         return probs.Length - 1;
//     }
// }