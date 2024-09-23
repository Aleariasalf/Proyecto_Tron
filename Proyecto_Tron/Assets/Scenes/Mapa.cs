using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{
    public int filas = 1000;
    public int columnas = 1000;
    public GameObject Muro;
    public GameObject Moto;
    public GameObject MotoEnemigaPrefab;  // Prefab de la Moto Enemiga
    public int cantidadInicialPool = 1000;
    public int cantidadEnemigos = 4;  // Cantidad de enemigos
    private Queue<GameObject> muroPool = new Queue<GameObject>();
    public int[,] grid;
    private GameObject moto;

    void Start()
    {
        InicializarPool();
        GenerarMatriz();
        GenerarMapa();
        CrearMoto();
        CrearEnemigos();  // Llamamos al método para crear enemigos
    }

    // Método para generar enemigos en posiciones aleatorias
    void CrearEnemigos()
    {
        for (int i = 0; i < cantidadEnemigos; i++)
        {
            Vector2 posicionAleatoria = ObtenerPosicionAleatoria();

            if (MotoEnemigaPrefab != null) // Verificamos que el prefab esté asignado
            {
                GameObject enemigo = Instantiate(MotoEnemigaPrefab);
                enemigo.transform.position = posicionAleatoria;

                // Marcamos la posición en la matriz
                Vector2 celdaEnemiga = TransformarAIndice(posicionAleatoria);
                int x = Mathf.RoundToInt(celdaEnemiga.x);
                int y = Mathf.RoundToInt(celdaEnemiga.y);
                if (x >= 0 && x < filas && y >= 0 && y < columnas)
                {
                    grid[x, y] = 3;  // Usamos el valor 3 para marcar la posición del enemigo
                }
            }
            else
            {
                Debug.LogError("El prefab MotoEnemiga no ha sido asignado en el script Mapa.");
            }
        }
    }

    // Método para obtener una posición aleatoria en la matriz
    Vector2 ObtenerPosicionAleatoria()
    {
        int x, y;
        do
        {
            x = Random.Range(1, filas - 1);  // Evitamos las paredes
            y = Random.Range(1, columnas - 1);
        } while (grid[x, y] != 0);  // Aseguramos que la celda esté vacía (valor 0)

        return new Vector2(x - filas / 2, y - columnas / 2);  // Convertimos a coordenadas del mundo
    }
    public void ActualizarPosicionMotoEnemiga(Vector2 nuevaPosicion, int valor = 2)
    {
        // Convertir la nueva posición a índices de la matriz
        int x = (int)(nuevaPosicion.x + (filas / 2));
        int y = (int)(nuevaPosicion.y + (columnas / 2));

        // Asegurarse de que los índices estén dentro del rango de la matriz
        if (x >= 0 && x < filas && y >= 0 && y < columnas)
        {
            // Actualizar la matriz con el valor correspondiente
            grid[x, y] = valor;
        }
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
