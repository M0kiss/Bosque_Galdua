using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsObjectInside : MonoBehaviour
{
    public Door door;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Object"))
        {
            Debug.Log("Objeto colidiu com bot�o");
            door.Open();
        }
    }
}
