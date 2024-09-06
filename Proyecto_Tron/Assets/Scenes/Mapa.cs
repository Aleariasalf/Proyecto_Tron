using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{
    public int filas = 1000;
    public int columnas = 1000;
    public GameObject Muro;
    public GameObject Moto;
    public int cantidadInicialPool = 1000;
    private Queue<GameObject> muroPool = new Queue<GameObject>();
    public int[,] grid;
    private GameObject moto;

    void Start()
    {
        InicializarPool();
        GenerarMatriz();
        GenerarMapa();
        CrearMoto();
    }
    

    void InicializarPool()
    {
        for (int i = 0; i < cantidadInicialPool; i++)
        {
            GameObject muro = Instantiate(Muro);
            muro.SetActive(false);
            muroPool.Enqueue(muro);
        }
    }

    GameObject ObtenerMuro()
    {
        if (muroPool.Count > 0)
        {
            GameObject muro = muroPool.Dequeue();
            muro.SetActive(true);
            return muro;
        }
        else
        {
            GameObject muro = Instantiate(Muro);
            return muro;
        }
    }

    void DevolverMuro(GameObject muro)
    {
        muro.SetActive(false);
        muroPool.Enqueue(muro);
    }

    void GenerarMatriz()
    {
        grid = new int[filas, columnas];

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (i == 0 || i == filas - 1 || j == 0 || j == columnas - 1)
                {
                    grid[i, j] = 1;
                }
                else
                {
                    grid[i, j] = 0;
                }
            }
        }
    }

    void GenerarMapa()
    {
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (grid[i, j] == 1)
                {
                    Vector2 posicion = new Vector2(i - filas / 2, j - columnas / 2);
                    GameObject muro = ObtenerMuro();
                    muro.transform.position = posicion;
                }
            }
        }
    }

    void CrearMoto()
    {
        if (moto == null)
        {
            if (Moto != null) // Verifica que MotoPrefab no sea null
            {
                moto = Instantiate(Moto);
                Vector2 posicion = new Vector2(filas / 2 - filas / 2, columnas / 2 - columnas / 2); // Posición inicial
                moto.transform.position = posicion;
                grid[filas / 2, columnas / 2] = 2; // Marca la posición de la moto en la matriz
            }
            else
            {
                Debug.LogError("El prefab Moto no ha sido asignado en el script Mapa.");
            }
        }
    }


    public void ActualizarPosicionMoto(Vector2 nuevaPosicion)
    {
        Vector2 antiguaPosicion = moto.transform.position;
        Vector2 antiguaCelda = TransformarAIndice(antiguaPosicion);
        Vector2 nuevaCelda = TransformarAIndice(nuevaPosicion);

        int xAntigua = Mathf.RoundToInt(antiguaCelda.x);
        int yAntigua = Mathf.RoundToInt(antiguaCelda.y);

        if (xAntigua >= 0 && xAntigua < filas && yAntigua >= 0 && yAntigua < columnas)
        {
            grid[xAntigua, yAntigua] = 0; // Limpia la posición antigua
        }

        int xNueva = Mathf.RoundToInt(nuevaCelda.x);
        int yNueva = Mathf.RoundToInt(nuevaCelda.y);

        if (xNueva >= 0 && xNueva < filas && yNueva >= 0 && yNueva < columnas)
        {
            grid[xNueva, yNueva] = 2; // Marca la nueva posición
            moto.transform.position = nuevaPosicion;
        }
    }

    Vector2 TransformarAIndice(Vector2 posicion)
    {
        return new Vector2(Mathf.Round(posicion.x + filas / 2), Mathf.Round(posicion.y + columnas / 2));
    }



}
