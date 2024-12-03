using Microsoft.AspNetCore.Mvc;
using ABMClientes.DataBase;
using ABMClientes.Models;


namespace ABMClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly ClienteDataBase _clienteDataBase;

        /// <summary>
        /// Constructor del controlador que inicializa el logger y la base de datos de clientes.
        /// </summary>
        /// <param name="logger">Interfaz para registrar eventos y mensajes.</param>
        public ClienteController(ILogger<ClienteController> logger)
        {
            _logger = logger;
            _clienteDataBase = new ClienteDataBase();
        }

        /// <summary>
        /// Endpoint para obtener todos los clientes registrados.
        /// </summary>
        /// <remarks>
        /// Este método no requiere parámetros. Devuelve una lista con todos los clientes disponibles en la base de datos.
        /// </remarks>
        /// <returns>Una colección de clientes.</returns>
        [HttpGet(Name = "ObtenerTodosLosEmpleados")]
        public IEnumerable<Cliente> ObtenerTodos()
        {
            return _clienteDataBase.ObtenerTodos();
        }


        /// <summary>
        /// Endpoint para obtener un cliente por su identificador único.
        /// </summary>
        /// <param name="id">El ID del cliente que se desea consultar.</param>
        /// <returns>El cliente correspondiente al ID o un estado 404 si no se encuentra.</returns>
        [HttpGet("{id}")]
        public ActionResult<Cliente> ObtenerPorId(int id)
        {
            var cliente = _clienteDataBase.ObtenerPorId(id);
            if (cliente == null)
            {
                return NotFound(); // Retorna 404 si el cliente no existe.
            }
            return Ok(cliente); // Retorna el cliente si se encuentra.
        }




        /// <summary>
        /// Endpoint para buscar clientes por nombre.
        /// </summary>
        /// <param name="nombre">El nombre o parte del nombre del cliente que se desea buscar.</param>
        /// <remarks>
        /// Utiliza una consulta de búsqueda que devuelve todos los clientes cuyos nombres coincidan parcialmente con el parámetro proporcionado.
        /// </remarks>
        /// <returns>Una lista de clientes que coinciden con la búsqueda o un estado 404 si no se encuentran resultados.</returns>

        [HttpGet("Search")]
        public ActionResult<List<Cliente>> Search([FromQuery] string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El nombre no puede estar vacío.");
            }

            var clientes = _clienteDataBase.Search(nombre);

            if (clientes == null || clientes.Count == 0)
            {
                return NotFound("No se encontraron clientes con ese nombre.");
            }

            return Ok(clientes);


        }




        /// <summary>
        /// Endpoint para agregar un nuevo cliente a la base de datos.
        /// </summary>
        /// <param name="cliente">El objeto cliente que se desea insertar.</param>
        /// <remarks>
        /// El cliente debe contener datos válidos y todos los campos obligatorios.
        /// </remarks>
        /// <returns>El cliente recién agregado.</returns>
        [HttpPost]
        public ActionResult<Cliente> InsertarCliente(Cliente cliente)
        {
            _clienteDataBase.Insert(cliente);
            return Ok(cliente);
        }



        /// <summary>
        /// Endpoint para actualizar los datos de un cliente existente.
        /// </summary>
        /// <param name="id">El ID del cliente que se desea actualizar.</param>
        /// <param name="cliente">El objeto cliente con los datos actualizados.</param>
        /// <remarks>
        /// El ID en la ruta debe coincidir con el ID proporcionado en el objeto cliente.
        /// </remarks>
        /// <returns>Un estado 204 (No Content) si la actualización es exitosa o 400 si los datos son inválidos.</returns>
        [HttpPut("{id}")]
        public IActionResult ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            if (cliente == null || id != cliente.Id)
            {
                return BadRequest();
            }

            // Llamar al método de base de datos para actualizar el cliente
            _clienteDataBase.Actualizar(cliente);

            return NoContent(); // Retorna un 204 No Content si la actualización es exitosa
        }



    }
}
