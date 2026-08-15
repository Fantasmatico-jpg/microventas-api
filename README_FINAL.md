# Aplicaciones Distribuidas — Proyecto Integrador U3

Proyecto de **Aplicaciones Distribuidas – Unidad 3**.

La solución integra tres microservicios desarrollados con tecnologías diferentes, cada uno con su propia base de datos, Docker Compose como mecanismo de despliegue, Kong API Gateway para centralizar el acceso y RabbitMQ para la comunicación asíncrona basada en eventos.

## 1. Objetivo

Desplegar, integrar y validar una arquitectura distribuida completa mediante Docker Compose.

La solución implementa:

- **MicroClientes:** .NET / ASP.NET Core + SQL Server.
- **MicroProductos:** FastAPI / Python + MySQL.
- **MicroVentas:** Spring Boot / Java + PostgreSQL.
- **Kong API Gateway:** punto de entrada para el acceso externo.
- **RabbitMQ:** broker de mensajería asíncrona.
- **Frontend:** HTML, CSS y JavaScript utilizando Fetch API.

## 2. Arquitectura implementada

```text
                           FRONTEND
                    HTML + CSS + JavaScript
                              |
                              | Fetch API
                              v
                      Kong API Gateway
                           :8000
                              |
          +-------------------+-------------------+
          |                   |                   |
          v                   v                   v
   /clientes              /productos           /ventas
   /clientes/             :8000                 :8000
   obtener-todos
          |                   |                   |
          v                   v                   v
   MicroClientes       MicroProductos       MicroVentas
     .NET / API          FastAPI / API       Spring Boot
          |                   |                   |
          v                   v                   v
    SQL Server               MySQL            PostgreSQL
                              ^
                              |
                         RabbitMQ
                              ^
                              |
                        VentaCreada
                              |
                        MicroVentas
```

Flujo principal:

```text
Frontend
   |
   v
Kong API Gateway
   |
   +--> MicroClientes --> SQL Server
   |
   +--> MicroProductos --> MySQL
   |
   +--> MicroVentas --> PostgreSQL
                         |
                         | VentaCreada
                         v
                      RabbitMQ
                         |
                         +--> MicroProductos --> Actualiza stock
                         |
                         +--> MicroClientes --> Guarda historial
```

## 3. Componentes de la arquitectura

| Componente | Tecnología | Responsabilidad |
|---|---|---|
| MicroClientes | .NET / ASP.NET Core | Gestión de clientes, historial y consumo de eventos |
| MicroProductos | FastAPI / Python | Gestión de productos y actualización de stock |
| MicroVentas | Spring Boot / Java | Gestión de ventas y publicación de eventos |
| SQL Server | SQL Server 2022 | Base de datos de MicroClientes |
| MySQL | MySQL 8 | Base de datos de MicroProductos |
| PostgreSQL | PostgreSQL 16 | Base de datos de MicroVentas |
| Kong | Kong 3.9 | API Gateway |
| RabbitMQ | RabbitMQ 3 Management | Mensajería asíncrona |
| Frontend | HTML + CSS + JavaScript | Interfaz para consumir las APIs |

## 4. Docker Compose

La arquitectura se ejecuta mediante un único archivo `docker-compose.yml`.

Servicios definidos:

```text
microclientes
microproductos
microventas
sqlserver
mysql
postgres
rabbitmq
kong
```

Comandos principales:

```bash
docker compose config
docker compose up -d
docker compose ps
docker compose down
```

## 5. Red Docker

La arquitectura utiliza una red compartida:

```text
microclientesapi_net-app-distribuidas
```

Dentro de esta red, los servicios pueden resolverse mediante sus nombres Docker:

```text
microclientes
microproductos
microventas
rabbitmq
sqlserver
mysql
postgres
kong
```

## 6. Persistencia de datos

La solución utiliza volúmenes Docker para conservar la información después de recrear los contenedores.

Durante las pruebas se validó la persistencia mediante:

```bash
docker compose down
docker compose up -d
```

Después del reinicio continuaron disponibles los datos de clientes y productos. El producto `Laptop HP` mantuvo su stock en `5`.

## 7. Kong API Gateway

Kong funciona en modo declarativo mediante `kong.yml`.

```text
KONG_DATABASE=off
KONG_DECLARATIVE_CONFIG=/kong/declarative/kong.yml
```

### Rutas principales

```text
/clientes
/clientes/obtener-todos
/productos
/ventas
```

MicroClientes utiliza `/clientes` para las operaciones CRUD y `/clientes/obtener-todos` para el listado general.

MicroProductos utiliza `/productos` y MicroVentas utiliza `/ventas`.

Todas las APIs fueron probadas a través de:

```text
http://localhost:8000
```

## 8. MicroClientes

MicroClientes utiliza .NET / ASP.NET Core y SQL Server.

Endpoints principales:

```text
GET    /api/Cliente
POST   /api/Cliente
GET    /api/Cliente/{id}
PUT    /api/Cliente/{id}
DELETE /api/Cliente/{id}
GET    /api/Cliente/obtener-todos
```

Se verificó el CRUD completo mediante Kong y el registro de historial de compras mediante eventos RabbitMQ.

## 9. MicroProductos

MicroProductos utiliza FastAPI / Python y MySQL.

```text
GET    /api/productos
POST   /api/productos
GET    /api/productos/{producto_id}
PUT    /api/productos/{producto_id}
DELETE /api/productos/{producto_id}
```

También se verificaron Query Parameters, por ejemplo:

```text
http://localhost:8000/productos?nombre=Laptop%20HP
```

## 10. MicroVentas

MicroVentas utiliza Spring Boot / Java y PostgreSQL.

```text
GET    /api/ventas
POST   /api/ventas
GET    /api/ventas/{id}
PUT    /api/ventas/{id}
DELETE /api/ventas/{id}
```

Cuando se registra una venta, MicroVentas publica el evento `VentaCreada`.

## 11. RabbitMQ y comunicación asíncrona

RabbitMQ funciona como broker de mensajería.

Configuración principal:

```text
Exchange: ventas.exchange
Queue: productos.stock
Queue: microclientes.historial
Routing Key: ventas.creada
```

Flujo:

```text
MicroVentas
    |
    | VentaCreada
    v
RabbitMQ
    |
    +----------------------+
    |                      |
    v                      v
MicroProductos       MicroClientes
    |                      |
    v                      v
Actualiza stock       Guarda historial
```

MicroProductos consume el evento para actualizar el stock y MicroClientes registra el historial de compra.

## 12. Swagger / OpenAPI

- MicroClientes: `http://localhost:8081/swagger/index.html`
- MicroProductos: `http://localhost:8002/docs`
- MicroVentas: `http://localhost:8082/swagger-ui/index.html`

Swagger/OpenAPI se utilizó para comprobar los endpoints antes de las pruebas funcionales.

## 13. Frontend

Se implementó un frontend sencillo utilizando:

```text
HTML
CSS
JavaScript
Fetch API
```

La aplicación se ejecuta con Live Server, por ejemplo:

```text
http://127.0.0.1:5500
```

El frontend consume las APIs a través de Kong y muestra clientes, productos y ventas en un panel básico.

### CORS

Para permitir el consumo desde Live Server se configuró CORS en Kong para:

```text
http://127.0.0.1:5500
http://localhost:5500
```

con métodos `GET`, `POST`, `PUT`, `DELETE` y `OPTIONS`.

## 14. Pruebas realizadas

### MicroClientes

```text
GET       ✅
POST      ✅
GET/{id}  ✅
PUT       ✅
DELETE    ✅
```

### MicroProductos

```text
GET       ✅
POST      ✅
GET/{id}  ✅
PUT       ✅
DELETE    ✅
```

### MicroVentas

```text
GET       ✅
POST      ✅
GET/{id}  ✅
PUT       ✅
DELETE    ✅
```

### Query Parameters

Se verificó una consulta filtrada mediante Kong:

```text
/productos?nombre=Laptop%20HP
```

### Códigos HTTP

Durante las pruebas se observaron respuestas `200`, `201`, `204`, `400` y `404` en operaciones exitosas y de validación.

## 15. Prueba de integración Venta → RabbitMQ → Stock

En la prueba final se registró una venta con:

```text
VentaId: 54
ClienteId: 1
ProductoId: 4
Cantidad: 1
Total: 1200.50
```

MicroVentas publicó `VentaCreada`, RabbitMQ distribuyó el mensaje y MicroProductos registró:

```text
Producto: 4
Anterior: 6
Nuevo: 5
```

MicroClientes también recibió el evento y guardó el historial correspondiente.

## 16. Docker Hub

Imágenes utilizadas:

```text
kenwaylabs/microclientes-api:2.0
kenwaylabs/microproductos-api:2.0
kenwaylabs/microventas-api:2.0
```

## 17. GitHub

Repositorio:

```text
https://github.com/Fantasmatico-jpg/microventas-api
```

El repositorio contiene los microservicios, `docker-compose.yml`, `kong.yml`, `README.md` y la configuración necesaria para ejecutar la arquitectura.

## 18. Dificultades encontradas y soluciones

### Rutas de Kong

Se ajustó `kong.yml` para que las rutas públicas coincidieran con los endpoints reales de MicroClientes y permitieran tanto CRUD como la consulta general.

### PowerShell y JSON

Algunas solicitudes con `curl.exe` presentaron conflictos de comillas. Se utilizó `ConvertTo-Json` junto con `Invoke-RestMethod` para construir y enviar los cuerpos JSON correctamente.

### RabbitMQ durante reinicio

RabbitMQ presentó temporalmente un estado `unhealthy` después de un reinicio completo. Tras su recuperación, `docker compose up -d` permitió iniciar los servicios dependientes.

### CORS

Al integrar el frontend mediante Live Server, el navegador bloqueó inicialmente las solicitudes. Se solucionó configurando CORS en Kong para los orígenes del frontend.

## 19. Resultados obtenidos

La arquitectura final quedó integrada por:

```text
MicroClientes
MicroProductos
MicroVentas
SQL Server
MySQL
PostgreSQL
RabbitMQ
Kong API Gateway
Frontend
```

Se verificó:

- Integración de los tres microservicios.
- Acceso mediante Kong.
- CRUD completo.
- Query Parameters.
- Swagger / OpenAPI.
- Comunicación asíncrona con RabbitMQ.
- Actualización automática del stock.
- Registro del historial en MicroClientes.
- Persistencia mediante volúmenes Docker.
- Consumo desde el frontend.
- CORS para el frontend.
- Versionamiento mediante GitHub.

## 20. Comandos principales

```bash
docker compose up -d
docker compose ps
docker compose down
docker compose restart microventas
```

Logs:

```bash
docker compose logs --tail 30 microclientes
docker compose logs --tail 30 microproductos
docker compose logs --tail 30 microventas
docker compose logs --tail 30 rabbitmq
docker compose logs --tail 30 kong
```

## 21. Tecnologías utilizadas

- Docker
- Docker Compose
- Kong 3.9
- RabbitMQ 3 Management
- .NET / ASP.NET Core
- Entity Framework Core
- FastAPI
- Python
- SQLAlchemy
- Java 21
- Spring Boot
- Hibernate
- SQL Server 2022
- MySQL 8
- PostgreSQL 16
- Swagger / OpenAPI
- HTML5
- CSS3
- JavaScript
- Fetch API
- Live Server
- Git
- GitHub
- Docker Hub

## 22. Conclusión

El proyecto demuestra que los tres microservicios pueden operar como un único sistema distribuido aunque utilicen diferentes tecnologías y bases de datos. Docker Compose permite desplegar la solución de forma coordinada, Kong centraliza el acceso a las APIs y RabbitMQ desacopla la comunicación basada en eventos.

Las pruebas funcionales, de integración, CRUD, Query Parameters, persistencia y frontend permitieron comprobar el funcionamiento coordinado de la arquitectura y dejan una base preparada para futuras mejoras.
