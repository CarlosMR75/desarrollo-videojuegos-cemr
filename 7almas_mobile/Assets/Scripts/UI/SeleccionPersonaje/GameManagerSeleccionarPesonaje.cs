using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSeleccionarPesonaje : MonoBehaviour
{
    public static GameManagerSeleccionarPesonaje Instance;

    public List<Personajes> personajes;

    private void Awake() {
        if(GameManagerSeleccionarPesonaje.Instance == null)
        {
            GameManagerSeleccionarPesonaje.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
