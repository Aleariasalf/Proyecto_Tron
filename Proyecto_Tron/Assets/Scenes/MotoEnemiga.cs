using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoEnemiga : Moto
{
    [SerializeField] private float tiempoEntreMovimientos = 1f;
    private float tiempoDesdeUltimoMovimiento = 0f;
    private int movimientosRestantes = 0;
    private Vector2 direccionActual;

    protected override void Update()
    {
        tiempoDesdeUltimoMovimiento += Time.deltaTime;

        if (tiempoDesdeUltimoMovimiento >= tiempoEntreMovimientos)
        {
            if (movimientosRestantes <= 0) // Si ya terminó los 3 movimientos en una dirección
            {
                SeleccionarNuevaDireccionAleatoria();
                movimientosRestantes = 3; // Cambia para moverse 3 veces en la nueva dirección
            }

            MoverEnDireccionActual();
            movimientosRestantes--;
            tiempoDesdeUltimoMovimiento = 0; // Reiniciar el temporizador
        }
    }

    private void SeleccionarNuevaDireccionAleatoria()
    {
        int direccion = Random.Range(0, 4);
        switch (direccion)
        {
            case 0: direccionActual = Vector2.up; break;
            case 1: direccionActual = Vector2.down; break;
            case 2: direccionActual = Vector2.left; break;
            case 3: direccionActual = Vector2.right; break;
        }
    }

    private void MoverEnDireccionActual()
    {
        Vector2 nuevaPosicion = transform.position + (Vector3)(direccionActual * velocidad * Time.deltaTime);

        if (EsPosicionValida(nuevaPosicion))
        {
            transform.position = nuevaPosicion;
            

            // Actualizar la matriz en el Mapa
            if (mapa != null)
            {
                mapa.ActualizarPosicionMotoEnemiga(ultimaPosicion, 0); // Limpiar la posición anterior en el mapa
                mapa.ActualizarPosicionMotoEnemiga(nuevaPosicion, 2); // Actualizar con la nueva posición
            }

            ultimaPosicion = nuevaPosicion;
        }
    }
}

