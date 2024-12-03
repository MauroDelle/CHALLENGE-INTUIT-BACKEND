using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABMClientes.Models;
using ABMClientes.DataBase;


namespace ABMClientes.DataBase
{
    public interface IDataBase<T> where T : class
    {
        static string DataBaseConnection()
        {
            return @"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;";
        }

        #region GENERICS
        void Conectar();
        void Desconectar();
        T ObtenerPorId(int id);
        List<T> ObtenerTodos();

        void Crear(T objeto);
        void Modificar(T objeto);
        void Borrar(T objeto);
        #endregion

    }

        public class DatabaseConnectionException : Exception
        {
            public DatabaseConnectionException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }

}
