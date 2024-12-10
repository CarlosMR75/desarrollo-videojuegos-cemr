using UnityEngine;
using Cinemachine;

public class CameraTargetController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform[] playerCharacters; // Asigna aquí tus prefabs o instancias de personajes

    void Start()
    {
        // Asigna el primer personaje como objetivo al iniciar
        if (playerCharacters.Length > 0)
        {
            virtualCamera.Follow = playerCharacters[0];
        }
    }

    public void ChangeTarget(int index)
    {
        if (index >= 0 && index < playerCharacters.Length)
        {
            virtualCamera.Follow = playerCharacters[index];
        }
    }
}
