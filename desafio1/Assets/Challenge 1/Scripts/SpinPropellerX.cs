using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
 * Carlos Eduardo Mata Rojas
 * Fecha: 04/10/2024
 * Descripci�n Hace girar la helice
 */
public class SpinPropellerX : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward * 1500 * Time.deltaTime);
    }
}
