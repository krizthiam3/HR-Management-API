# HRManagement API (.NET 8 + EF Core)

Sistema de Gestión de Recursos Humanos desarrollado como desafío técnico para evaluar habilidades en desarrollo Fullstack con .NET

---

## 📌 Descripción

Esta API permite gestionar empleados, departamentos y puestos (positions) en una organización. Implementa operaciones CRUD completas para cada entidad, con buenas prácticas de arquitectura, validaciones, desacoplamiento y seguridad básica.

---

## 🛠️ Tecnologías Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (Code First)
- SQL Server (Instancia configurada)
- Swagger (documentación interactiva)
- Serilogs (opcional)
- MSTest y Moq
- JWT
- Migrations

---

## 🧱 Estructura del Proyecto

HRManagement.API/
├── Controllers/
├── Models/
├── DTOs/
├── Repositories/
│ ├── Interfaces/
│ └── Implementations/
├── Data/
│ └── AppDbContext.cs
├── Migrations/
├── Program.cs



---

## 🧠 Principios Aplicados

- ✅ Inyección de Dependencias (DI)
- ✅ Repositorio por entidad (`IEmployeeRepository`, etc.)
- ✅ Separación de responsabilidades (SRP)
- ✅ Validaciones con `[Required]`, `[EmailAddress]`
- ✅ DTOs para control de exposición de datos
- ✅ Código limpio y mantenible

---

## 🔧 Instrucciones para ejecutar localmente

### 1. Clona el repositorio

```bash
git [clone https://github.com/krizthiam3/hrmanagement-api.git](https://github.com/krizthiam3/HR-Management-API.git)
cd hrmanagement-api


2. Configura la base de datos en appSetting.json

"DefaultConnection": 
"Server=localhost; <-- Cambiar nombre del servidor
Database=HRManagementDb;  <-- Cambiar nombre de la base de datos, o crear una con este mismo nombre
User Id=usuario;  <-- Cambiar usuario
Password=clave;  <-- Cambiar contraseña
TrustServerCertificate=True;"


4. Ejecutar migraciones y crear la base de datos

dotnet tool install --global dotnet-ef
dotnet ef database update

Si no se creo la BD Ejectutat este comando o crearla manual en el Administrador de BD (SOLO LA BASE DE DATOS)

PASO 1,
-- Crear base de datos para el sistema de gestión de empleados
CREATE DATABASE HRDB;
GO

PASO 2, voler a ejecutar

dotnet tool install --global dotnet-ef
dotnet ef database update


Crear los usuario de ingreso desde el ENDPOINTS
![image](https://github.com/user-attachments/assets/4984e623-dafe-4e33-ae51-4524a7b22cbf)

O usando esta sentencia SQL

USE [HRManagementDb]
GO

INSERT INTO [dbo].[Users] ([Username] ,[PasswordHash] ,[FullName] ,[Role] ,[IsActive] ,[CreatedAt])
 VALUES ('user1@email.com' ,'$2a$11$3WY4Joy52ba6WFwzlk8LauvGimYd6LTRoAu70WmrByRSTMiHA9cF.'  -- user123 
           ,'Usuario' 
           ,'User'
           ,1
           ,GETDATE())
GO

INSERT INTO [dbo].[Users] ([Username], [PasswordHash] ,[FullName] ,[Role] ,[IsActive] ,[CreatedAt])
     VALUES ('admin1@email.com'
           ,'$2a$11$4KAFOIfKOx97mxBMJJNW7.zXzI3S5uAmVfxlZaQb2IISrIY4.dj3y'  -- admin123
           ,'Usuario Administrador'
           ,'Admin'
           ,1
           ,GETDATE())
GO

5. Correr la aplicación

dotnet run
🔗 Abre: https://localhost:5001/swagger


📦 Arquitectura y componentes clave
🧩 DTOs
Separan el modelo de dominio de los datos enviados/recibidos en la API.

📚 Repositorios
Cada entidad tiene su IRepository y una implementación desacoplada:


🔄 Inyección de Dependencias (DI)
Todas las dependencias son resueltas por el contenedor de servicios de .NET.

✅ Funcionalidades Implementadas

Funcionalidad	Estado
CRUD Empleados, Departamentos, Puestos	
Validación de datos ([Required], etc.)	
Uso de DTOs para limpieza de modelo	
Repositorios personalizados	
Inyección de dependencias (DbContext y Repos)	
Documentación con Swagger	
Manejo adecuado de errores HTTP	
Relaciones EF Core (Employee → Department/Position)	
Autenticación y autorización por roles (Admin, User)

🧪 Pruebas unitarias 
📊 Paginación, orden y filtrado
📦 Contenedorización con Docker


🧔 Autor
Cristian Valencia
👨‍💻 Fullstack Developer (.NET + Angular)
  
