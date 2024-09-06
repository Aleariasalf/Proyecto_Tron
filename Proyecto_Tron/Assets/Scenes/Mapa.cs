using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{
    public int filas = 1000;
    public int columnas = 1000;
    public GameObject Muro;
    public int cantidadInicialPool = 1000;
    private Queue<GameObject> muroPool = new Queue<GameObject>();
    private int[,] grid;

    void Start()
    {
        InicializarPool();
        GenerarMatriz();
        GenerarMapa();
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
}
