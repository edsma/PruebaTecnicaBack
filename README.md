Desafío de Backend Shopping
Este repositorio contiene una solución de backend para el desafío de Shopping, implementado utilizando .NET Core. Proporciona una aplicación para gestionar productos y órdenes. Sigue las instrucciones a continuación para configurar y utilizar la aplicación.

Requisitos previos
SQL Server: Asegúrate de tener SQL Server instalado y en funcionamiento.
Configuración TCP/IP: Habilita el protocolo TCP/IP en SQL Server para permitir conexiones.
Autenticación de Windows: La aplicación utiliza la autenticación de Windows para el inicio de sesión de usuarios. Asegúrate de que el usuario tenga los permisos necesarios para acceder a SQL Server.

Configuración
Clona el repositorio en tu máquina local.
Abre la solución en Visual Studio o en tu entorno de desarrollo .NET Core preferido (.net 6.0).

Configuración de la base de datos
Abre SQL Server Management Studio (SSMS) u otro cliente de SQL Server.
Crea una nueva base de datos llamada "Shopping".

Configuración de la cadena de conexión
En el archivo appsettings.json, busca la sección ConnectionStrings.
Actualiza el valor DefaultConnection con tu cadena de conexión de SQL Server. Asegúrate de reemplazar los valores de marcador de posición (por ejemplo, ServerName, DatabaseName) con la información real de tu servidor y base de datos.

Uso
Compila y ejecuta la aplicación desde tu entorno de desarrollo.
Accede a la aplicación utilizando los endpoints proporcionados, por ejemplo, http://localhost:7038/api/products, http://localhost:7038/api/orders.

Utiliza una herramienta de prueba de API (por ejemplo, Postman) para interactuar con los endpoints disponibles y realizar operaciones CRUD en productos y órdenes.
Para la autenticación de usuarios, la aplicación utilizará automáticamente las credenciales del usuario de Windows actual para iniciar sesión.
