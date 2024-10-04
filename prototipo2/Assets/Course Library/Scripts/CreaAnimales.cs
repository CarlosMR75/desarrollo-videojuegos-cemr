using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaAnimales : MonoBehaviour
{
    public GameObject[] animales;

    void Start()
    {
        InvokeRepeating("CreateAnimal", 1, 1);
    }

    void CreateAnimal()
    {
        int index = Random.Range(0, 3);
        int posX = Random.Range(-20, 20);

        animales[index].transform.position = new Vector3(posX,
            animales[index].transform.position.y,
            animales[index].transform.position.z);

        Instantiate(animales[index], animales[index].transform.position,
            animales[index].transform.rotation);
    }

}
