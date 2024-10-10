using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
 * Carlos Eduardo Mata Rojas
 * Fecha: 04/10/2024
 * Descripción: Sigue al jugador
 */
public class FollowPlayerX : MonoBehaviour
{
    public GameObject plane;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(30,0,10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = plane.transform.position + offset;
    }
}
