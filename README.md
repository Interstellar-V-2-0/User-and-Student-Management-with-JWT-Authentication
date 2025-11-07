# User-and-Student-Management-with-JWT-Authentication

## Descripción General
Este proyecto es una **API RESTful desarrollada en C# con ASP.NET Core**, diseñada para gestionar **usuarios, estudiantes y roles**, con un sistema de **autenticación JWT (JSON Web Token)**.

Su objetivo es demostrar cómo se estructura y protege una aplicación moderna en .NET, utilizando capas separadas (controladores, servicios, entidades, DTOs) para mantener un código limpio, seguro y fácil de mantener.

---

## Arquitectura del Proyecto

```
User-and-Student-Management-with-JWT-Authentication/
├── UserStudentMgmt.API/             → Controladores y configuración de la API
├── UserStudentMgmt.Application/     → Lógica de negocio, DTOs y servicios
├── UserStudentMgmt.Domain/          → Entidades (clases que representan los datos)
├── UserStudentMgmt.Infrastructure/  → Conexión a base de datos y repositorios
└── appsettings.json                 → Configuración de la aplicación (cadena de conexión, JWT, etc.)
```

### Flujo General

1. El cliente (por ejemplo, Postman o una aplicación frontend) envía una solicitud HTTP.
2. Un **Controlador** recibe la solicitud y valida los datos.
3. El **Servicio** aplica la lógica del negocio.
4. El **Repositorio** accede o modifica los datos en la base de datos.
5. Se devuelven respuestas utilizando **DTOs** (objetos ligeros de transferencia de datos).
6. Si el usuario está autenticado, el sistema valida su **token JWT** antes de permitirle acceder a los endpoints protegidos.

---

## Entidades Principales (Domain Layer)

### `User`
Representa a una persona que puede autenticarse en el sistema.

| Propiedad | Tipo | Descripción |
|------------|------|-------------|
| `Id` | int | Identificador único del usuario |
| `Username` | string | Nombre de usuario usado para iniciar sesión |
| `PasswordHash` | string | Contraseña en formato encriptado |
| `Role` | string | Rol del usuario (`Admin`, `Student`, etc.) |

### `Student`
Representa a un estudiante vinculado a un documento de identidad y otros datos personales.

| Propiedad | Tipo | Descripción |
|------------|------|-------------|
| `Id` | int | Identificador único |
| `FirstName` | string | Nombres del estudiante |
| `LastName` | string | Apellidos del estudiante |
| `DocumentTypeId` | int | Tipo de documento (relación con DocumentType) |
| `DocumentType` | DocumentType | Objeto que describe el tipo de documento |

### `DocumentType`
Catálogo de tipos de documento (CC, TI, Pasaporte, etc.).

| Propiedad | Tipo | Descripción |
|------------|------|-------------|
| `Id` | int | Identificador |
| `Name` | string | Nombre del tipo de documento |
| `Students` | List<Student> | Lista de estudiantes con este tipo de documento |

---

## DTOs (Data Transfer Objects)

Los **DTOs** son clases intermedias que transportan datos entre la API y el cliente.  
Evitan exponer directamente las entidades del dominio y permiten controlar qué información se envía o recibe.

### Ejemplo: `UserLoginDTO`
```csharp
public class UserLoginDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
}
```

**Explicación detallada:**
- `Username`: el nombre que el usuario usa para iniciar sesión.  
- `Password`: contraseña en texto plano (solo se usa al momento de enviar la solicitud).  
- Este DTO se usa en el método de login para autenticar al usuario y generar el token JWT.

### Ejemplo: `UserRegisterDTO`
```csharp
public class UserRegisterDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
```

**Uso:**
- Se utiliza al crear un nuevo usuario.
- El backend convierte este DTO en una entidad `User`, encripta la contraseña y guarda el usuario en la base de datos.

### Ejemplo: `StudentDTO`
```csharp
public class StudentDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int DocumentTypeId { get; set; }
}
```
**Uso:**
- Transfiere los datos de los estudiantes entre cliente y servidor.
- Permite crear o actualizar estudiantes sin exponer propiedades internas.

---

## Autenticación JWT

El sistema usa **JSON Web Tokens (JWT)** para autenticar y autorizar usuarios.

### Flujo de Autenticación
1. El usuario envía sus credenciales (`Username`, `Password`) al endpoint `/api/auth/login`.
2. El sistema valida las credenciales en la base de datos.
3. Si son correctas, se genera un **token JWT** con:
   - ID del usuario
   - Nombre de usuario
   - Rol
   - Tiempo de expiración
4. En las siguientes solicitudes, el cliente debe incluir el token en el encabezado:
   ```
   Authorization: Bearer <tu_token_aquí>
   ```
5. El servidor valida el token antes de permitir el acceso a rutas protegidas.

---

## Controladores (Controllers)

### `AuthController`
Encargado del inicio de sesión y registro.

| Método | Ruta | Descripción |
|---------|------|-------------|
| `POST` | `/api/auth/register` | Crea un nuevo usuario |
| `POST` | `/api/auth/login` | Inicia sesión y devuelve un token JWT |

### `StudentController`
Gestiona la información de los estudiantes.

| Método | Ruta | Descripción |
|---------|------|-------------|
| `GET` | `/api/students` | Lista todos los estudiantes |
| `GET` | `/api/students/{id}` | Obtiene un estudiante por ID |
| `POST` | `/api/students` | Crea un nuevo estudiante |
| `PUT` | `/api/students/{id}` | Actualiza un estudiante existente |
| `DELETE` | `/api/students/{id}` | Elimina un estudiante |

---

## Ejemplos de Peticiones HTTP

### Registrar Usuario
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "admin1",
  "password": "123456",
  "role": "Admin"
}
```

### Iniciar Sesión
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin1",
  "password": "123456"
}
```

**Respuesta esperada:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Crear Estudiante
```http
POST /api/students
Authorization: Bearer <token>
Content-Type: application/json

{
  "firstName": "Andrea",
  "lastName": "Ospino",
  "documentTypeId": 1
}
```

---

## Glosario para Principiantes

| Concepto | Significado |
|-----------|-------------|
| **API REST** | Servicio que permite enviar y recibir datos mediante HTTP. |
| **Controller** | Clase que gestiona las solicitudes de la API. |
| **DTO** | Objeto que transporta datos entre capas. |
| **Entidad** | Representa una tabla en la base de datos. |
| **Servicio (Service)** | Contiene la lógica del negocio. |
| **Repositorio** | Maneja la interacción directa con la base de datos. |
| **JWT** | Token digital que autentica al usuario en la API. |

---

## Tecnologías Utilizadas
- **ASP.NET Core 8**
- **Entity Framework Core**
- **JWT Authentication**
- **SQL Server**
- **C# 10**

---
