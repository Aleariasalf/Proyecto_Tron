using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotoEnemiga : Moto
{
    private float tiempoEntreMovimientos = 1f; // Tiempo entre cambios de direcci�n
    private float tiempoDesdeUltimoMovimiento;

    private Vector2 ultimaPosicion;

    protected override void Update()
    {
        tiempoDesdeUltimoMovimiento += Time.deltaTime;

        if (tiempoDesdeUltimoMovimiento >= tiempoEntreMovimientos)
        {
            MoverAleatoriamente();
            tiempoDesdeUltimoMovimiento = 0; // Reiniciar el temporizador
        }
    }

    private void MoverAleatoriamente()
    {
        // Elige una direcci�n aleatoria
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

        // Actualizar la posici�n en Unity
        transform.position = nuevaPosicion;

        // Actualizar la matriz en el Mapa
        if (mapa != null)
        {
            // Actualizar la posici�n anterior a espacio vac�o
            mapa.ActualizarPosicionMotoEnemiga(ultimaPosicion, 0); // 0 para espacio vac�o
            mapa.ActualizarPosicionMotoEnemiga(nuevaPosicion);
        }

        // Guardar la nueva posici�n como �ltima
        ultimaPosicion = nuevaPosicion;
    }

}
