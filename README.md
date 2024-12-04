---------------------------------------
Clientes API - Descripción del Proyecto
---------------------------------------


La Clientes API es una aplicación desarrollada en .NET 8 que permite gestionar información de clientes para un negocio.
La API está diseñada siguiendo principios de arquitectura RESTful y utiliza SQL Server como base de datos para almacenar la información.
Este proyecto puede integrarse fácilmente con aplicaciones frontend, como Angular, para crear soluciones completas de gestión empresarial.



---------------------------------------
  Tecnologías Utilizadas
---------------------------------------
- .NET 8: Framework para construir servicios web modernos y escalables.

- Entity Framework Core: ORM (Object-Relational Mapping) para la interacción con la base de datos.

- C#: Lenguaje principal para la implementación de la lógica del servidor.

- SQL Server: Sistema de gestión de bases de datos relacionales.

- Swagger/OpenAPI: Documentación interactiva para la API, lo que facilita el consumo y prueba de los endpoints.


---------------------------------------
 Herramientas
---------------------------------------
- Postman: Herramienta para probar y consumir los endpoints de la API.

- Visual Studio: Entorno de desarrollo integrado (IDE) para la implementación de la solución.

- Git: Control de versiones para el código fuente.




---------------------------------------
Estructura del Proyecto
---------------------------------------

El proyecto sigue una arquitectura de capas, organizada de la siguiente manera:

1. Controllers: Maneja las solicitudes HTTP y define los endpoints.

2. Services: Contiene la lógica de negocio.

3. Repositories: Interactúa directamente con la base de datos utilizando Entity Framework Core.

4. Models: Define las clases y entidades del dominio.

5. Data: Contiene el contexto de la base de datos y configuraciones relacionadas.



---------------------------------------
Flujo de Funcionamiento
---------------------------------------

1. Recepción de Solicitudes:
El cliente (frontend o herramienta como Postman) realiza solicitudes HTTP a los endpoints definidos en el controlador.

2. Validación de Datos:
Los datos recibidos son validados antes de proceder con cualquier operación.

3. Procesamiento en el Servicio:
La lógica de negocio se implementa en la capa de servicios, que delega las operaciones CRUD (Crear, Leer, Actualizar, Eliminar) a los repositorios.

4. Interacción con la Base de Datos:
Los repositorios usan Entity Framework Core para interactuar con SQL Server y ejecutar consultas.

5. Respuesta:
El controlador devuelve una respuesta en formato JSON con los datos solicitados o mensajes de error en caso de que algo salga mal.



---------------------------------------
Endpoints Principales
---------------------------------------

- GET /api/clientes: Obtiene la lista de todos los clientes.

- GET /api/clientes/{id}: Obtiene los detalles de un cliente específico.

- POST /api/clientes: Crea un nuevo cliente.

- PUT /api/clientes/{id}: Actualiza la información de un cliente existente.

- DELETE /api/clientes/{id}: Elimina un cliente por su ID



---------------------------------------
Configuración y Ejecución
---------------------------------------

1. Clonar el Repositorio:
git clone https://github.com/MauroDelle/CHALLENGE-INTUIT-BACKEND.git

2. cd ClientesAPI
Asegúrate de tener SQL Server instalado.

3. Configura la cadena de conexión en el archivo appsettings.json.
  
4. dotnet ef database update

5. dotnet run

---------------------------------------
Características Adicionales
---------------------------------------

- Seguridad: Se puede configurar autenticación y autorización con JWT (JSON Web Tokens) o cualquier otro proveedor de identidad.

- Validaciones: Implementación de validaciones en los modelos utilizando anotaciones de datos ([Required], [EmailAddress], etc.).

- Manejo de Errores: Respuestas consistentes para errores con códigos de estado HTTP y mensajes descriptivos.



Diagrama: 
<img src="https://github.com/user-attachments/assets/37ba2400-f8a4-4c9f-bbb4-344366242981" alt="ABMCliente" width="230">

