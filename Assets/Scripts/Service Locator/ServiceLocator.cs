using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        Type type = typeof(T);

        if (services.ContainsKey(type))
        {
            services[type] = service;
            return;
        }
        services.Add(type, service);
    }

    public static T Get<T>()
    {
        Type type = typeof(T);

        if (services.TryGetValue(type, out object service))
        {
            return (T)service;
        }
        throw new Exception("Servicio de tipo " + type + " no registrado");
    }

    public static bool TryGet<T>(out T service)
    {
        Type type = typeof(T);

        if(services.TryGetValue(type, out object serviceF))
        {
            service = (T)serviceF;
            return true;
        }
        service = default;
        return false;
    }

    public static void Unregister<T>()
    {
        Type type = typeof(T);

        if (services.ContainsKey(type))
        {
            services.Remove(type);
        }
    }
}
