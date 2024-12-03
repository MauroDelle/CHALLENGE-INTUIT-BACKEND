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

        public ClienteController(ILogger<ClienteController> logger)
        {
            _logger = logger;
            _clienteDataBase = new ClienteDataBase();
        }

        //EndPoint para obtener todos los empleados
        [HttpGet(Name = "ObtenerTodosLosEmpleados")]
        public IEnumerable<Cliente> ObtenerTodos()
        {
            return _clienteDataBase.ObtenerTodos();
        }


        //EndPoint para obtener un cliente por Id
        [HttpGet("{id}")]
        public ActionResult<Cliente> ObtenerPorId(int id)
        {
            var cliente = _clienteDataBase.ObtenerPorId(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

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


        // POST: api/clientes
        [HttpPost]
        public ActionResult<Cliente> InsertarCliente(Cliente cliente)
        {
            _clienteDataBase.Insert(cliente);
            return Ok(cliente);
        }




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
