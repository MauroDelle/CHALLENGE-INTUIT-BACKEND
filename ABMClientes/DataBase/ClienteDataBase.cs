using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ABMClientes.Models;
using ABMClientes.DataBase;
using Microsoft.SqlServer;
using System.Data.SqlClient;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text.RegularExpressions;

namespace ABMClientes.DataBase
{
    public class ClienteDataBase : IDataBase<Cliente>
    {
        private SqlConnection? _connection;


        public void Conectar()
        {
            string connectionString = @"Server=localhost\SQLEXPRESS;Database=ABMClientes;Trusted_Connection=True;TrustServerCertificate=True;";


            _connection = new SqlConnection(connectionString);


            try
            {
                _connection.Open();

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
                _connection = null;
            }

        }

        public void Desconectar()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public void Crear(Cliente obj)
        {
            // Validar los datos obligatorios
            if (string.IsNullOrWhiteSpace(obj.Nombre) ||
                string.IsNullOrWhiteSpace(obj.Apellido) ||
                string.IsNullOrWhiteSpace(obj.CUIT) ||
                string.IsNullOrWhiteSpace(obj.TelefonoCelular) ||
                string.IsNullOrWhiteSpace(obj.Email))
            {
                Console.WriteLine("Error: Todos los campos obligatorios deben ser completados.");
                return; // Salir del método si los datos obligatorios faltan
            }

            // Validar fecha de nacimiento (debe ser una fecha válida y estar en el pasado)
            if (obj.FechaNacimiento == null || obj.FechaNacimiento > DateTime.Now)
            {
                Console.WriteLine("Error: La fecha de nacimiento no es válida.");
                return;
            }

            // Validar formato del email
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!emailRegex.IsMatch(obj.Email))
            {
                Console.WriteLine("Error: El email no tiene un formato válido.");
                return;
            }

            // Validar formato del CUIT (Ejemplo: 20-XXXXXXXX-X)
            var cuitRegex = new Regex(@"^(20|27)\d{8}-\d{1}$");
            if (!cuitRegex.IsMatch(obj.CUIT))
            {
                Console.WriteLine("Error: El CUIT no tiene un formato válido.");
                return;
            }

            // Verificar si el CUIT ya existe en la base de datos (Unicidad)
            if (!ValidarCUITUnico(obj.CUIT))
            {
                Console.WriteLine("Error: El CUIT ya está registrado.");
                return;
            }

            Conectar();

            try
            {
                // Consulta SQL para insertar los datos del cliente (sin incluir el Id)
                string query = "INSERT INTO Clientes (nombres, apellidos, FechaNacimiento, cuit, domicilio, TelefonoCelular, email) " +
                               "VALUES (@Nombres, @Apellidos, @FechaNacimiento, @CUIT, @Domicilio, @TelefonoCelular, @Email)";

                // Crear un comando SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(query, _connection);

                // Agregar los parámetros a la consulta
                command.Parameters.AddWithValue("@Nombres", obj.Nombre);
                command.Parameters.AddWithValue("@Apellidos", obj.Apellido);
                command.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento);
                command.Parameters.AddWithValue("@CUIT", obj.CUIT);
                command.Parameters.AddWithValue("@Domicilio", obj.Domicilio);
                command.Parameters.AddWithValue("@TelefonoCelular", obj.TelefonoCelular);
                command.Parameters.AddWithValue("@Email", obj.Email);

                // Ejecutar el comando SQL
                command.ExecuteNonQuery();
                Console.WriteLine("Cliente creado exitosamente.");
            }
            catch (SqlException ex)
            {
                // Capturar cualquier error durante la ejecución
                Console.WriteLine($"Error al crear cliente: {ex.Message}");
            }
            finally
            {
                // Desconectar de la base de datos
                Desconectar();
            }
        }

        public void Modificar(Cliente obj) { }

        public void Borrar(Cliente obj) { }

        public Cliente ObtenerPorId(int id)
        {
            Conectar();

            Cliente cliente = null;


            string query = "SELECT * FROM Clientes WHERE ID = @ID";

            SqlCommand command = new SqlCommand(@query, _connection);

            command.Parameters.AddWithValue("@ID", id);


            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cliente = new Cliente
                    {
                        Id = (int)reader["ID"],
                        Nombre = reader["Nombres"].ToString(),
                        Apellido = reader["Apellidos"].ToString(),
                        FechaNacimiento = (DateTime)reader["FechaNacimiento"],
                        CUIT = reader["CUIT"].ToString(),
                        Domicilio = reader["Domicilio"].ToString(),
                        TelefonoCelular = reader["TelefonoCelular"].ToString(),
                        Email = reader["Email"].ToString()
                    };
                }
            }

            Desconectar();

            return cliente;
        }

        public List<Cliente> ObtenerTodos()
        {
            Conectar();
            List<Cliente> clientes = new List<Cliente>();

            string query = "SELECT * FROM Clientes";
            SqlCommand command = new SqlCommand(query, _connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id = (int)reader["ID"],
                        Nombre = reader["Nombres"].ToString(),
                        Apellido = reader["Apellidos"].ToString(),
                        FechaNacimiento = (DateTime)reader["FechaNacimiento"],
                        CUIT = reader["CUIT"].ToString(),
                        Domicilio = reader["Domicilio"].ToString(),
                        TelefonoCelular = reader["TelefonoCelular"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }

            Desconectar();
            return clientes;
        }

        public List<Cliente> Search(string nombre)
        {
            Conectar();
            List<Cliente> clientes = new List<Cliente>();

            string query = "SELECT * FROM Clientes WHERE Nombres LIKE @Nombre";

            SqlCommand command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Nombre", "%" + nombre + "%"); // % en ambos lados para búsqueda parcial

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id = (int)reader["ID"],
                        Nombre = reader["Nombres"].ToString(),
                        Apellido = reader["Apellidos"].ToString(),
                        FechaNacimiento = (DateTime)reader["FechaNacimiento"],
                        CUIT = reader["CUIT"].ToString(),
                        Domicilio = reader["Domicilio"].ToString(),
                        TelefonoCelular = reader["TelefonoCelular"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }

            Desconectar();
            return clientes;
        }




        /// <summary> DOCUMENTACION
        /// Inserta un nuevo cliente en la base de datos después de realizar todas las validaciones necesarias.
        /// </summary>
        /// <param name="cliente">
        /// Objeto de tipo <see cref="Cliente"/> que contiene los datos del cliente a insertar.
        /// </param>
        /// <remarks>
        /// Validaciones realizadas:
        /// - Todos los campos obligatorios deben estar completos.
        /// - La fecha de nacimiento debe ser válida y estar en el pasado.
        /// - El email debe tener un formato válido.
        /// - El CUIT debe ser numérico y tener exactamente 11 dígitos.
        /// - El CUIT debe ser único en la base de datos.
        /// </remarks>
        /// <exception cref="SqlException">
        /// Lanzada si ocurre un error durante la ejecución de la consulta SQL.
        /// </exception>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var nuevoCliente = new Cliente
        /// {
        ///     Nombre = "Juan",
        ///     Apellido = "Pérez",
        ///     FechaNacimiento = new DateTime(1990, 5, 20),
        ///     CUIT = "20304567891",
        ///     Domicilio = "Calle Falsa 123",
        ///     TelefonoCelular = "1156789123",
        ///     Email = "juan.perez@mail.com"
        /// };
        /// clienteService.Insert(nuevoCliente);
        /// </code>
        /// </example>
        public void Insert(Cliente cliente)
        {
            //Primero Valido que los campos no esten vacios

            if (string.IsNullOrWhiteSpace(cliente.Nombre) ||
                string.IsNullOrWhiteSpace(cliente.Apellido) ||
                string.IsNullOrWhiteSpace(cliente.CUIT) ||
                string.IsNullOrWhiteSpace(cliente.TelefonoCelular) ||
                string.IsNullOrWhiteSpace(cliente.Email))
            {
                Console.WriteLine("Error: Todos los campos obligatorios deben ser completados.");
                return;  //Salgo si falta informacion necesaria
            }

            //Verifico que la fecha de nacimiento sea valida y este en el pasado.
            if (cliente.FechaNacimiento == null || cliente.FechaNacimiento > DateTime.Now)
            {
                Console.WriteLine("Error: La fecha de nacimiento no es válida.");
                return;
            }

            //Valido el formato del mail
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!emailRegex.IsMatch(cliente.Email))
            {
                Console.WriteLine("Error: El email no tiene un formato válido.");
                return;
            }


            if (string.IsNullOrWhiteSpace(cliente.CUIT) || cliente.CUIT.Length != 11 || !long.TryParse(cliente.CUIT, out _))
            {
                Console.WriteLine("El CUIT debe tener exactamente 11 dígitos numéricos.");
                return;
            }

            if (!ValidarCUITUnico(cliente.CUIT))
            {
                Console.WriteLine("Error: El CUIT ya está registrado.");
                return;
            }

            Conectar();

            string query = "INSERT INTO Clientes (Nombres, Apellidos, FechaNacimiento, CUIT, Domicilio, TelefonoCelular, Email) " +
                           "VALUES (@Nombres, @Apellidos, @FechaNacimiento, @CUIT, @Domicilio, @TelefonoCelular, @Email)";


            SqlCommand command = new SqlCommand(query, _connection);

            command.Parameters.AddWithValue("@Nombres", cliente.Nombre);
            command.Parameters.AddWithValue("@Apellidos", cliente.Apellido);
            command.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);
            command.Parameters.AddWithValue("@CUIT", cliente.CUIT);
            command.Parameters.AddWithValue("@Domicilio", cliente.Domicilio);
            command.Parameters.AddWithValue("@TelefonoCelular", cliente.TelefonoCelular);
            command.Parameters.AddWithValue("@Email", cliente.Email);

            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Cliente insertado exitosamente.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al insertar cliente: {ex.Message}");
            }
            finally
            {
                Desconectar();
            }
        }


        /// <summary>
        /// Verifica si un CUIT es único en la base de datos.
        /// </summary>
        /// <param name="cuit">
        /// CUIT a verificar, representado como una cadena.
        /// </param>
        /// <returns>
        /// Devuelve <c>true</c> si el CUIT no existe en la base de datos; de lo contrario, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Este método utiliza una consulta SQL para contar las ocurrencias del CUIT en la tabla `Clientes`.
        /// Si el CUIT ya está registrado, la validación falla.
        /// </remarks>
        /// <exception cref="SqlException">
        /// Puede lanzarse si ocurre un problema durante la ejecución de la consulta SQL.
        /// </exception>
        private bool ValidarCUITUnico(string cuit)
        {
            // Conectar a la base de datos para verificar si el CUIT ya existe
            Conectar();

            // Consulta SQL para verificar la unicidad del CUIT
            string query = "SELECT COUNT(1) FROM Clientes WHERE CUIT = @CUIT";
            SqlCommand command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@CUIT", cuit);

            int count = Convert.ToInt32(command.ExecuteScalar());

            Desconectar();

            // Si el CUIT ya existe, devolver false
            return count == 0;
        }


        /// <summary>
        /// Actualiza los datos de un cliente en la base de datos.
        /// </summary>
        /// <param name="obj">
        /// Objeto de tipo <see cref="Cliente"/> que contiene los datos actualizados del cliente.
        /// </param>
        /// <remarks>
        /// La actualización se realiza según el <c>Id</c> del cliente.
        /// Todos los campos se actualizan si el cliente existe; de lo contrario, no se realizan cambios.
        /// </remarks>
        /// <exception cref="SqlException">
        /// Puede lanzarse si ocurre un problema durante la ejecución de la consulta SQL.
        /// </exception>
        public void Actualizar(Cliente obj)
        {
            Conectar();

            try
            {
                // Consulta SQL para actualizar los datos del cliente según el ID
                string query = "UPDATE Clientes SET " +
                               "nombres = @Nombres, " +
                               "apellidos = @Apellidos, " +
                               "FechaNacimiento = @FechaNacimiento, " +
                               "cuit = @CUIT, " +
                               "domicilio = @Domicilio, " +
                               "TelefonoCelular = @TelefonoCelular, " +
                               "email = @Email " +
                               "WHERE id = @Id";

                // Crear un comando SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(query, _connection);

                // Agregar los parámetros a la consulta
                command.Parameters.AddWithValue("@Nombres", obj.Nombre);
                command.Parameters.AddWithValue("@Apellidos", obj.Apellido);
                command.Parameters.AddWithValue("@FechaNacimiento", obj.FechaNacimiento);
                command.Parameters.AddWithValue("@CUIT", obj.CUIT);
                command.Parameters.AddWithValue("@Domicilio", obj.Domicilio);
                command.Parameters.AddWithValue("@TelefonoCelular", obj.TelefonoCelular);
                command.Parameters.AddWithValue("@Email", obj.Email);
                command.Parameters.AddWithValue("@Id", obj.Id); // El ID para actualizar el cliente

                // Ejecutar el comando SQL
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No se encontró el cliente con el ID especificado.");
                }
            }
            catch (SqlException ex)
            {
                // Capturar cualquier error durante la ejecución
                Console.WriteLine($"Error al actualizar cliente: {ex.Message}");
            }
            finally
            {
                // Desconectar de la base de datos
                Desconectar();
            }
        }


        public bool ValidarIdUnico(int id)
        {
            Conectar();
            string query = "SELECT COUNT(1) FROM Clientes WHERE id = @Id";
            SqlCommand command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Id", id);

            int count = Convert.ToInt32(command.ExecuteScalar());

            Desconectar();

            return count == 0; // Si no existe el ID, es único
        }


    }
}
