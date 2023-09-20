using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelota : MonoBehaviour
{
    private Jugador jugador; // Referencia al objeto del jugador
    private bool movimientoHabilitado = true; // Controla si la pelota se mueve o no
    private Vector3 posicionInicial; // Almacena la posición inicial de la pelota
    private Rigidbody2D rb; // Referencia al componente Rigidbody2D

    private void Start()
    {
        jugador = FindObjectOfType<Jugador>(); // Encuentra y almacena la referencia al jugador
        posicionInicial = transform.position; // Almacena la posición inicial de la pelota
        rb = GetComponent<Rigidbody2D>(); // Obtener el componente Rigidbody2D
    }

    private void Update()
    {
    }

    public void IniciarMovimiento()
    {
        movimientoHabilitado = true;
        // Reinicia la posición de la pelota a la inicial
        transform.position = posicionInicial;
        // Reinicia cualquier velocidad o fuerza que apliques a la pelota
        rb.velocity = Vector2.zero;

        // Restaura la gravedad
        rb.gravityScale = 1.0f;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            // Destruye la pelota cuando colisiona con el jugador
            Destroy(gameObject);
        }
    }
}
