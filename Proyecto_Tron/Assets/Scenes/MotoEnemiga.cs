using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoEnemiga : Moto
{
    [SerializeField] private float tiempoEntreMovimientosEnemiga = 1f; 

    

    protected override void Update()
    {
        // Usar el campo tiempoDesdeUltimoMovimiento de la clase base Moto
        tiempoDesdeUltimoMovimiento += Time.deltaTime;

        if (tiempoDesdeUltimoMovimiento >= tiempoEntreMovimientosEnemiga)
        {
            MoverAleatoriamente();
            tiempoDesdeUltimoMovimiento = 0; // Reiniciar el temporizador
        }
    }

    private void MoverAleatoriamente()
    {
        // Lógica de movimiento aleatorio...
        int direccion = Random.Range(0, 4);
        Vector2 nuevaPosicion = transform.position;

        switch (direccion)
        {
            case 0: // Arriba
                nuevaPosicion.y += velocidad * Time.deltaTime;
                break;
            case 1: // Abajo
                nuevaPosicion.y -= velocidad * Time.deltaTime;
                break;
            case 2: // Izquierda
                nuevaPosicion.x -= velocidad * Time.deltaTime;
                break;
            case 3: // Derecha
                nuevaPosicion.x += velocidad * Time.deltaTime;
                break;
        }

        // Verificar si la nueva posición es válida
        if (EsPosicionValida(nuevaPosicion))
        {
            transform.position = nuevaPosicion;
            if (mapa != null)
            {
                mapa.ActualizarPosicionMotoEnemiga(ultimaPosicion, 0); // 0 para espacio vacío
                mapa.ActualizarPosicionMotoEnemiga(nuevaPosicion);
            }
            ultimaPosicion = nuevaPosicion;
        }
    }
}
