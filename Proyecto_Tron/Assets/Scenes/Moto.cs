using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moto : MonoBehaviour
{
    public float tiempoEntreMovimientos = 0.2f; // Tiempo entre movimientos
    public Vector2 direccion = Vector2.right; // Direcci�n inicial
    public Mapa mapa;
    private Vector2 celdaActual;
    private float tiempoDesdeUltimoMovimiento;

    void Start()
    {
        mapa = FindObjectOfType<Mapa>();
        if (mapa == null)
        {
            Debug.LogError("No se encontr� el script Mapa.");
        }
        celdaActual = TransformarAIndice(transform.position);
        tiempoDesdeUltimoMovimiento = Time.time;
    }

    void Update()
    {
        if (mapa != null)
        {
            // Actualizar direcci�n basada en la entrada del teclado
            if (Input.GetKey(KeyCode.UpArrow))
            {
                direccion = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                direccion = Vector2.down;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                direccion = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                direccion = Vector2.right;
            }

            // Mover la moto a intervalos regulares
            if (Time.time - tiempoDesdeUltimoMovimiento >= tiempoEntreMovimientos)
            {
                MoverMoto();
                tiempoDesdeUltimoMovimiento = Time.time;
            }
        }
    }

    void MoverMoto()
    {
        Vector2 nuevaCelda = celdaActual + direccion;
        if (EsCeldaValida(nuevaCelda))
        {
            mapa.ActualizarPosicionMoto(TransformarAPosicion(nuevaCelda));
            celdaActual = nuevaCelda;
        }
    }

    Vector2 TransformarAIndice(Vector2 posicion)
    {
        // Convierte la posici�n del mundo a una posici�n en la matriz
        return new Vector2(Mathf.Round(posicion.x + mapa.filas / 2), Mathf.Round(posicion.y + mapa.columnas / 2));
    }

    Vector2 TransformarAPosicion(Vector2 celda)
    {
        // Convierte la posici�n de la matriz a una posici�n en el mundo
        return new Vector2(celda.x - mapa.filas / 2, celda.y - mapa.columnas / 2);
    }

    bool EsCeldaValida(Vector2 celda)
    {
        int x = Mathf.RoundToInt(celda.x);
        int y = Mathf.RoundToInt(celda.y);
        return x >= 0 && x < mapa.filas && y >= 0 && y < mapa.columnas && mapa.grid[x, y] == 0;
    }
}
