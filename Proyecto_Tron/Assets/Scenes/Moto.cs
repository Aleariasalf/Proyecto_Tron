using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moto : MonoBehaviour
{
    protected float tiempoEntreMovimientos = 0.5f; // Tiempo entre movimientos
    public float velocidad = 5f;
    protected Vector2 ultimaPosicion; // Última posición de la moto
    public Vector2 direccion = Vector2.right; // Dirección inicial
    public Mapa mapa;
    private Vector2 celdaActual;
    protected float tiempoDesdeUltimoMovimiento = 0f;
    public LineRenderer lineRenderer; 
    private Estela estela;

    protected virtual void Start()
    {
        estela = new Estela(5);
        mapa = FindObjectOfType<Mapa>();
        if (mapa == null)
        {
            Debug.LogError("No se encontró el script Mapa.");
        }
        celdaActual = TransformarAIndice(transform.position);
        tiempoDesdeUltimoMovimiento = Time.time;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo")) 
        {
            Destroy(gameObject); // Destruye la moto del jugador
            Debug.Log("Moto del jugador destruida por colisión.");
        }
    }
    protected virtual void Update()
    {
        if (lineRenderer != null)
        {
            estela.DibujarEstela(lineRenderer);
        }
        if (mapa != null)
        {
            // Actualizar dirección basada en la entrada del teclado
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
            // Actualiza la posición de la moto
            mapa.ActualizarPosicionMoto(TransformarAPosicion(nuevaCelda));
            celdaActual = nuevaCelda;

            // Agrega la posición actual a la estela después de mover
            estela.AgregarPosicion(TransformarAPosicion(celdaActual));
        }
    }


    Vector2 TransformarAIndice(Vector2 posicion)
    {
        // Convierte la posición del mundo a una posición en la matriz
        return new Vector2(Mathf.Round(posicion.x + mapa.filas / 2), Mathf.Round(posicion.y + mapa.columnas / 2));
    }

    Vector2 TransformarAPosicion(Vector2 celda)
    {
        // Convierte la posición de la matriz a una posición en el mundo
        return new Vector2(celda.x - mapa.filas / 2, celda.y - mapa.columnas / 2);
    }

    bool EsCeldaValida(Vector2 celda)
    {
        int x = Mathf.RoundToInt(celda.x);
        int y = Mathf.RoundToInt(celda.y);
        return x >= 0 && x < mapa.filas && y >= 0 && y < mapa.columnas && mapa.grid[x, y] == 0;
    }

    protected bool EsPosicionValida(Vector2 posicion)
    {
        int x = (int)(posicion.x + (mapa.filas / 2));
        int y = (int)(posicion.y + (mapa.columnas / 2));

        // Verificar que esté dentro de los límites de la matriz
        if (x < 0 || x >= mapa.filas || y < 0 || y >= mapa.columnas)
            return false;

        
        return mapa.grid[x, y] == 0; // Solo permitir movimiento a espacios vacíos
    }


}
