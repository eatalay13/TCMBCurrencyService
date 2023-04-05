using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TCMB.CurrencyService.Exporters
{
    public static class ExporterManager
    {
        public static IEnumerable<Type> GetAllExporters()
        {
            return from x in Assembly.GetAssembly(typeof(IExporter<>)).GetTypes()
                   from z in x.GetInterfaces()
                   let y = x.BaseType
                   where
                   (y != null && y.IsGenericType &&
                   typeof(IExporter<>).IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                   (z.IsGenericType &&
                   typeof(IExporter<>).IsAssignableFrom(z.GetGenericTypeDefinition()))
                   select x;
        }

        public static IEnumerable<string> GetAllExporterNames()
        {
            var allExporter = GetAllExporters();

            if (!allExporter.Any())
                throw new Exception("Exporter could not be found.");

            return allExporter.Select(e => e.Name.Replace("Exporter", "").Replace("`1", ""));
        }

        public static IExporter<T> GetExporterByName<T>(string name)
        {
            var exporter = GetAllExporters().FirstOrDefault(e => e.Name.ToLower().Contains(name.ToLower())) ?? throw new Exception("Exporter could not be found.");

            return Activator.CreateInstance(exporter.MakeGenericType(typeof(T))) as IExporter<T>;
        }
    }
}
