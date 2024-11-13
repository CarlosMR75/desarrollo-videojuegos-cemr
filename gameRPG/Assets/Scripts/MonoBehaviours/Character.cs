using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Clase genérica para todo tipo de personaje en el Juego.
*/
public abstract class Character : MonoBehaviour
{
    public HitPoints hitPoints; //Puntos de vida actuales
    public int maxHitPoints; //Puntos de vida máximos
}