using System.Collections.Generic;
using UnityEngine;

public class Estela
{
    private Nodo cabeza;
    private int largoEstela;
    private int longitudActual;

    public Estela(int largoEstela)
    {
        this.largoEstela = largoEstela;
        this.cabeza = null;
        this.longitudActual = 0;
    }

    public void AgregarPosicion(Vector2 nuevaPosicion)
    {
        Nodo nuevoNodo = new Nodo(nuevaPosicion);

        // Si la estela est� vac�a, a�adimos el primer nodo
        if (cabeza == null)
        {
            cabeza = nuevoNodo;
        }
        else
        {
            // Agregamos el nuevo nodo al principio
            nuevoNodo.siguiente = cabeza;
            cabeza = nuevoNodo;
        }

        longitudActual++;

        // Si la longitud actual excede el largo de la estela, eliminamos el �ltimo nodo
        if (longitudActual > largoEstela)
        {
            EliminarUltimoNodo();
        }
    }

    private void EliminarUltimoNodo()
    {
        if (cabeza == null) return;

        if (cabeza.siguiente == null)
        {
            cabeza = null; // Solo hab�a un nodo
        }
        else
        {
            Nodo actual = cabeza;
            while (actual.siguiente != null && actual.siguiente.siguiente != null)
            {
                actual = actual.siguiente;
            }
            actual.siguiente = null; // Eliminamos el �ltimo nodo
        }

        longitudActual--;
    }

    public void DibujarEstela(LineRenderer lineRenderer)
    {
        List<Vector3> posiciones = new List<Vector3>();
        Nodo actual = cabeza;

        while (actual != null)
        {
            posiciones.Add(actual.posicion); // Agregamos la posici�n a la lista
            actual = actual.siguiente;
        }

        lineRenderer.positionCount = posiciones.Count;
        lineRenderer.SetPositions(posiciones.ToArray()); // Actualizamos la l�nea del LineRenderer
    }
}
