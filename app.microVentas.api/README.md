# MicroVentas API

Microservicio REST para la gestión de ventas desarrollado con Java 21, Spring Boot 3.5.16, Spring Data JPA, Hibernate y PostgreSQL. Forma parte de una arquitectura distribuida integrada con Docker, Docker Compose y Kong API Gateway.

## Arquitectura

```text
Cliente / Frontend
       |
       v
Kong API Gateway :8000
       |
       | /ventas
       v
MicroVentas API :8080
       |
       v
Service Layer
       |
       v
Repository (JPA)
       |
       v
Hibernate
       |
       v
PostgreSQL :5432
```

## Tecnologías

- Java 21
- Spring Boot 3.5.16
- Spring Web
- Spring Data JPA
- Hibernate
- PostgreSQL 16
- Maven
- Spring Validation
- Lombok
- SpringDoc OpenAPI / Swagger
- Docker
- Docker Compose
- Kong Gateway 3.9
- Docker Hub

## Estructura

```text
microventas-api/
├── src/main/java/com/example/microventas/
│   ├── controller/
│   ├── dto/
│   ├── entity/
│   ├── repository/
│   ├── service/
│   ├── config/
│   └── exception/
├── src/main/resources/
│   └── application.properties
├── Dockerfile
├── docker-compose.yml
├── kong.yml
├── pom.xml
└── README.md
```

- **controller:** endpoints REST.
- **dto:** objetos de entrada y salida de la API.
- **entity:** entidades JPA.
- **repository:** acceso a datos mediante Spring Data JPA.
- **service:** lógica de negocio.
- **config:** configuración adicional.
- **exception:** manejo de excepciones.

## Entidad Venta

La entidad `Venta` contiene:

- `id`
- `numeroVenta`
- `fecha`
- `cliente`
- `total`
- `estado`
- `observacion`

## DTOs

- `VentaRequestDTO`: recibe datos para crear o actualizar ventas.
- `VentaResponseDTO`: representa la información devuelta por la API.

Los DTOs separan la representación de la API de las entidades de persistencia.

## Repository Pattern

Se utilizan:

- `VentaRepository`
- `IVentaService`
- `VentaService`

El repositorio gestiona el acceso a datos y la capa Service concentra la lógica de negocio. El controlador utiliza inyección de dependencias.

## Endpoints

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/ventas` | Obtener todas las ventas |
| GET | `/api/ventas/{id}` | Obtener una venta |
| POST | `/api/ventas` | Crear una venta |
| PUT | `/api/ventas/{id}` | Actualizar una venta |
| DELETE | `/api/ventas/{id}` | Eliminar una venta |

### Query Parameters

```http
GET /api/ventas?cliente=Carlos
GET /api/ventas?estado=true
GET /api/ventas?fecha=2026-08-01
```

## Swagger

```text
http://localhost:8082/swagger-ui/index.html
```

La documentación se genera mediante SpringDoc OpenAPI.

## PostgreSQL

Base de datos:

```text
MicroVentasDB
```

PostgreSQL se ejecuta mediante Docker:

```text
Host: localhost
Puerto publicado: 5435
Puerto interno: 5432
Usuario: admin
```

## Docker

Construcción de la imagen:

```powershell
mvn clean package -DskipTests
docker build -t microventas-api:1.0 .
```

La aplicación se ejecuta dentro del contenedor en el puerto `8080` y se publica localmente mediante `8082`.

## Docker Compose

Servicios principales:

- `microventas-api`
- `database-postgres-ventas`

Iniciar:

```powershell
docker compose up -d
```

Comprobar:

```powershell
docker compose ps
```

Logs:

```powershell
docker compose logs microventas
```

## Kong API Gateway

Kong utiliza configuración declarativa mediante `kong.yml`.

Endpoint público:

```http
GET http://localhost:8000/ventas
```

Kong redirige las solicitudes hacia el microservicio MicroVentas.

La ruta fue probada correctamente y devolvió:

```json
[]
```

Esto confirma el consumo del microservicio mediante Kong; el arreglo vacío indica que no había ventas en la consulta.

## Docker Hub

Imagen publicada:

```text
kenwaylabs/microventas-api:1.0
```

Tag:

```powershell
docker tag microventas-api:1.0 kenwaylabs/microventas-api:1.0
```

Publicación:

```powershell
docker push kenwaylabs/microventas-api:1.0
```

## Pruebas realizadas

- Spring Boot iniciado correctamente.
- PostgreSQL conectado correctamente.
- Persistencia mediante Hibernate/JPA.
- CRUD completo.
- Query Parameters.
- Swagger / OpenAPI.
- Docker Build.
- Docker Compose.
- Contenedores funcionando.
- Integración con Kong.
- Consumo mediante Kong.
- Imagen publicada en Docker Hub.

## Repositorios

**GitHub:** agregar aquí el enlace al repositorio del proyecto.

**Docker Hub:** `kenwaylabs/microventas-api:1.0`

## Autor

Proyecto académico para la asignatura **Aplicaciones Distribuidas**.
