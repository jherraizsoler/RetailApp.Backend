# RetailApp.Backend    ![Logo de RetailApp - Espacio para tu logo](https://github.com/jherraizsoler/RetailApp.Backend/blob/master/Logo%20Backend.png)

Este repositorio contiene el código fuente del backend (API RESTful) para **RetailApp**, una solución integral diseñada para una empresa internacional de ropa. El objetivo principal de este proyecto es proporcionar una API robusta y escalable que permita la gestión de productos, inventarios, usuarios, tiendas y pedidos, sirviendo como el cerebro de las operaciones digitales de la compañía.

## 🚀 Propósito del Proyecto

En el dinámico mundo del comercio minorista, especialmente en el sector de la moda, la eficiencia operativa y una experiencia de cliente fluida son clave. RetailApp.Backend nace de la necesidad de centralizar y optimizar la gestión de recursos para una empresa de ropa con presencia global.

Este backend actúa como el pilar fundamental que:
* **Centraliza los Datos:** Gestiona de forma unificada información vital como productos (con variantes, imágenes, descripciones detalladas), inventario en múltiples almacenes y tiendas, marcas, categorías y colecciones.
* **Soporta Múltiples Clientes:** Ofrece una API RESTful que puede ser consumida por diversas aplicaciones cliente (Web, Móvil, Escritorio), garantizando una experiencia consistente y actualizada.
* **Optimiza Operaciones:** Permite la gestión eficiente de usuarios, carritos de compra, pedidos y la asignación de productos a tiendas y almacenes.
* **Escalabilidad y Mantenibilidad:** Construido con tecnologías modernas y principios de diseño robustos para facilitar el crecimiento y la evolución futura del negocio.

## ✨ Tecnologías Utilizadas

Este proyecto backend está desarrollado con el ecosistema de .NET, aprovechando las siguientes tecnologías clave:

* **.NET (ASP.NET Core):** El framework principal para construir aplicaciones web y APIs de alto rendimiento y multiplataforma.
* **C#:** El lenguaje de programación moderno y orientado a objetos de Microsoft, conocido por su potencia y versatilidad.
* **Entity Framework Core (EF Core):** Un potente Object-Relational Mapper (ORM) que simplifica la interacción con la base de datos, permitiendo trabajar con objetos C# en lugar de escribir SQL directamente. Facilita las operaciones CRUD y la gestión de migraciones del esquema de la base de datos.
* **SQL Server LocalDB:** Una versión ligera y gratuita de SQL Server, ideal para el desarrollo local, utilizada como base de datos relacional para la persistencia de datos.
* **API RESTful:** La arquitectura de la API sigue los principios REST, proporcionando endpoints claros y estándar para la comunicación entre el backend y las aplicaciones cliente.
* **Swagger/OpenAPI:** Integrado para la documentación interactiva de la API, facilitando la exploración, prueba y consumo de los endpoints por parte de los desarrolladores.
* **Patrones de Diseño:**
    * **Inyección de Dependencias:** Utilizada para desacoplar componentes y mejorar la modularidad y la capacidad de prueba.
    * **Capas de Servicios:** Separación de la lógica de negocio en una capa de servicios dedicada para mantener los controladores limpios y enfocados en la interacción HTTP.

## 📦 Estructura del Proyecto

El proyecto `RetailApp.Backend` sigue una estructura organizada para facilitar su desarrollo y mantenimiento:

* **`Controllers/`**: Contiene los controladores de la API que exponen los endpoints HTTP para interactuar con los datos (ej. `UsersController`, `ProductsController`).
* **`Data/`**: Aloja el `ApplicationDbContext`, la clase central de Entity Framework Core que representa la sesión con la base de datos y permite consultar y guardar datos.
* **`Models/`**: Define las clases de entidad (POCOs) que representan las tablas de la base de datos y sus relaciones (ej. `User`, `Product`, `Store`, `Order`).
* **`Interfaces/`**: Define las interfaces para los servicios de la capa de negocio, promoviendo la inyección de dependencias y un código más testable.
* **`Services/`**: Contiene las implementaciones de las interfaces de servicio, encapsulando la lógica de negocio y la interacción directa con el `DbContext`.
* **`Migrations/`**: Generado por Entity Framework Core, contiene los scripts para la evolución del esquema de la base de datos.
* **`appsettings.json`**: Archivo de configuración para la aplicación, incluyendo la cadena de conexión a la base de datos.
* **`Program.cs`**: Punto de entrada de la aplicación donde se configura el servidor web, la inyección de dependencias y el middleware.

## 🚀 Primeros Pasos

Para poner en marcha el proyecto `RetailApp.Backend` en tu máquina local:

1.  **Clona el Repositorio:**
    ```bash
    git clone [https://github.com/TuUsuario/RetailApp.Backend.git](https://github.com/TuUsuario/RetailApp.Backend.git)
    cd RetailApp.Backend
    ```
    *(Reemplaza `TuUsuario` con el nombre de tu usuario/organización en GitHub)*

2.  **Configura la Cadena de Conexión:**
    * Abre el archivo `appsettings.json`.
    * Asegúrate de que la cadena de conexión `DefaultConnection` apunte a tu instancia de SQL Server LocalDB. Por defecto, debería ser similar a:
        ```json
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RetailAppDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
        ```

3.  **Aplica las Migraciones de la Base de Datos:**
    * Abre la **Consola del Administrador de Paquetes** (Package Manager Console) en Visual Studio.
    * Asegúrate de que `RetailApp.Backend` esté seleccionado como "Proyecto predeterminado".
    * Ejecuta los siguientes comandos para crear la base de datos y sus tablas:
        ```powershell
        Add-Migration InitialCreate
        Update-Database
        ```
        *(Si has realizado cambios en los modelos después de la `InitialCreate`, puedes necesitar una nueva migración, por ejemplo: `Add-Migration AddProductVariants` y luego `Update-Database`)*

4.  **Ejecuta la Aplicación:**
    * Compila el proyecto (`Ctrl + Shift + B`).
    * Ejecuta la aplicación (`F5` o el botón "IIS Express" en Visual Studio).
    * Esto abrirá automáticamente la interfaz de **Swagger UI** en tu navegador (`https://localhost:XXXXX/swagger`), donde podrás explorar y probar todos los endpoints de la API.

## 🧪 Pruebas de la API con Swagger UI

Una vez que la aplicación esté en ejecución y Swagger UI abierta:

1.  **Crea entidades con dependencias primero:** Para crear un `Product`, necesitarás un `Brand` y una `Category` existentes.
    * Expande el controlador `Brands` y usa `POST /api/Brands` para crear una marca (ej. `{"name": "Nike"}`). Anota el `Id` devuelto.
    * Expande el controlador `Categories` y usa `POST /api/Categories` para crear una categoría (ej. `{"name": "Calzado Deportivo"}`). Anota el `Id` devuelto.
2.  **Crea un Producto:**
    * Expande el controlador `Products` y usa `POST /api/Products`.
    * En el "Request body", proporciona los datos del producto, incluyendo los `Id`s de la marca y categoría que creaste previamente.
    * Ejemplo de payload:
        ```json
        {
          "name": "Zapatillas Correr Ultraboost",
          "shortDescription": "Zapatillas de alto rendimiento.",
          "longDescription": "Diseñadas para la máxima amortiguación.",
          "brandId": 1,        // ID de tu Brand
          "categoryId": 1,     // ID de tu Category
          "basePrice": 150.99,
          "isActive": true,
          "createdDate": "2025-07-30T00:00:00Z",
          "url_Slug": "zapatillas-correr-ultraboost"
          // ... otros campos según tu modelo Product
        }
        ```
3.  **Crea un Usuario:**
    * Expande el controlador `Users` y usa `POST /api/Users`.
    * Proporciona los datos del usuario en el "Request body".
    * Ejemplo de payload:
        ```json
        {
          "email": "usuario@ejemplo.com",
          "passwordHash": "passwordhashseguro",
          "firstName": "Juan",
          "lastName": "Perez",
          "registrationDate": "2025-07-30T00:00:00Z"
          // ... otros campos según tu modelo User
        }
        ```
4.  **Prueba los GET:** Una vez creadas las entidades, utiliza los endpoints `GET` (ej. `GET /api/Users`, `GET /api/Products`) para verificar que los datos se hayan guardado y puedan ser recuperados.

## 🤝 Contribuciones

¡Las contribuciones son bienvenidas! Si deseas contribuir a este proyecto, por favor, sigue estos pasos:

1.  Haz un "fork" del repositorio.
2.  Crea una nueva rama (`git checkout -b feature/nombre-de-la-caracteristica`).
3.  Realiza tus cambios y haz "commit" (`git commit -m 'feat: Añade nueva característica'`).
4.  Haz "push" a tu rama (`git push origin feature/nombre-de-la-caracteristica`).
5.  Abre un "Pull Request".

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Consulta el archivo `LICENSE` para más detalles.

---

Este `README.md` es un punto de partida. Puedes personalizarlo añadiendo:
* Un logo real de RetailApp.
* Más detalles sobre la visión de la empresa.
* Instrucciones más detalladas para la configuración del entorno si hay dependencias específicas.
* Sección de "Roadmap" si tienes planes futuros para el proyecto.
* Capturas de pantalla de Swagger UI o de la base de datos.
