using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class GenericPool<T> : IUpdateable
{
    private T objeto;

    private List<T> poolingList= new List<T>();
    public GenericPool(T objeto)
    {
        this.objeto = objeto;

        UpdateManager.Instance.Register(this);
    }

    public void CustomUpdate(float time)
    {

    }
    public void Destroy()
    {
        UpdateManager.Instance.Unregister(this);
    }

    private void OnGet(GameObject pooledObject)
    {
        pooledObject.SetActive(true);
    }

    // Se llama cuando un elemento se devuelve al grupo.
    private void OnRelease(GameObject pooledObject)
    {
        pooledObject.SetActive(false);
    }

 // // Se llama cuando el grupo decide destruir un elemento (por ejemplo, por encima del tamaño máximo).
 //  private void OnDestroyItem(T objeto)
 //  {
 //      Destroy(T group);
 //  }
 // 
 //  private System.Collections.IEnumerator ReturnAfter(GameObject pooledObject, float seconds)
 //  {
 //      yield return new WaitForSeconds(seconds);
 //      // Devuélvelo a la piscina.
 //      pool.Release(pooledObject);
 //  }

}
