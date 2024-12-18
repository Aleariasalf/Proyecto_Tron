using UnityEngine;

public class Nodo
{
    public Vector2 posicion;
    public Nodo siguiente;

    public Nodo(Vector2 posicion)
    {
        this.posicion = posicion;
        this.siguiente = null;
    }
}
