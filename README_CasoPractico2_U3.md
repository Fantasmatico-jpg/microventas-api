Caso Práctico 2 U3

Despliegue de una arquitectura distribuida

Proyecto de Aplicaciones Distribuidas – Unidad 3.

Este proyecto integra los microservicios de Clientes, Productos y Ventas en una arquitectura distribuida desplegada con Docker Compose. La solución incorpora Kong API Gateway para el acceso externo y RabbitMQ para la comunicación asíncrona basada en eventos.

1. Objetivo

Desplegar una arquitectura distribuida completa integrando microservicios, bases de datos, API Gateway y mensajería asíncrona mediante Docker Compose.

La solución implementa:

MicroClientes: .NET 8 / ASP.NET Core + SQL Server.

MicroProductos: FastAPI / Python + MySQL.

MicroVentas: Spring Boot / Java + PostgreSQL.

Kong API Gateway.

RabbitMQ.

2. Arquitectura implementada

                         Frontend
                            |
                            v
                    Kong API Gateway
                            |
             +--------------+--------------+
             |              |              |
             v              v              v
       /clientes       /productos       /ventas
             |              |              |
             v              v              v
       MicroClientes   MicroProductos   MicroVentas
        .NET 8          FastAPI        Spring Boot
             |              |              |
             v              v              v
        SQL Server        MySQL        PostgreSQL
                            |
                            v
                        RabbitMQ
                     (eventos)

Kong gestiona el acceso externo mediante las rutas:

/clientes
/productos
/ventas

RabbitMQ se utiliza para la comunicación asíncrona de los eventos generados por MicroVentas.

3. Componentes de la arquitectura

Componente

Tecnología

Responsabilidad

MicroClientes

.NET 8 / ASP.NET Core

Gestión de clientes e historial

MicroProductos

FastAPI / Python

Gestión de productos y stock

MicroVentas

Spring Boot / Java

Gestión de ventas y publicación de eventos

SQL Server

SQL Server 2022

Base de datos de Clientes

MySQL

MySQL 8

Base de datos de Productos

PostgreSQL

PostgreSQL 16

Base de datos de Ventas

Kong

Kong 3.9

API Gateway

RabbitMQ

RabbitMQ 3 Management

Mensajería asíncrona

4. Docker Compose

El proyecto utiliza un archivo docker-compose.yml central para integrar los servicios de la solución.

Servicios definidos:

postgres
mysql
sqlserver
rabbitmq
microventas
microproductos
microclientes
kong

La red común utilizada por la arquitectura es:

microclientesapi_net-app-distribuidas

Para verificar la configuración:

docker compose config

Para iniciar la arquitectura:

docker compose up -d

Para consultar el estado:

docker compose ps

Para detener los servicios:

docker compose down

5. Red Docker

Todos los componentes principales utilizan una red Docker compartida para permitir la comunicación mediante nombres de servicio.

La red utilizada es:

microclientesapi_net-app-distribuidas

Dentro de esta red, Kong puede resolver:

microclientes
microproductos
microventas

Esto permite que Kong enrute las solicitudes hacia los microservicios mediante sus nombres DNS internos.

6. Persistencia de datos

La arquitectura utiliza volúmenes Docker para conservar la información de las bases de datos.

Volúmenes principales:

appmicroventasapi_postgres-ventas-data
microproductos-api_mysql_productos_data
microclientesapi_sql_clientes_data

Estos volúmenes permiten mantener la información de PostgreSQL, MySQL y SQL Server aunque los contenedores sean recreados.

7. Kong API Gateway

Kong se ejecuta en modo declarativo utilizando:

KONG_DATABASE=off
KONG_DECLARATIVE_CONFIG=/kong/declarative/kong.yml

El archivo utilizado para la configuración es:

kong.yml

Rutas públicas

Ruta pública

Servicio interno

/clientes

http://microclientes:8080/api/cliente

/productos

http://microproductos:8000/api/productos

/ventas

http://microventas:8080/api/ventas

Ejemplos

http://localhost:8000/clientes
http://localhost:8000/productos
http://localhost:8000/ventas

Las tres rutas fueron probadas correctamente mediante el Gateway.

8. MicroClientes

MicroClientes está desarrollado con .NET 8 y ASP.NET Core.

Funcionalidades verificadas:

GET de clientes.

POST de clientes.

GET por identificador.

PUT de clientes.

DELETE de clientes.

Historial de compras mediante eventos.

También se verificó el acceso por Kong mediante:

http://localhost:8000/clientes

y:

http://localhost:8000/clientes/{id}

9. MicroProductos

MicroProductos está desarrollado con FastAPI y Python.

Funcionalidades verificadas:

GET de productos.

POST de productos.

GET por identificador.

PUT de productos.

DELETE de productos.

Filtros mediante Query Parameters.

Query Parameters disponibles:

nombre
categoria
estado

Ejemplo probado mediante Kong:

http://localhost:8000/productos?nombre=Laptop%20HP

El Gateway devolvió correctamente el producto filtrado.

10. MicroVentas

MicroVentas está desarrollado con Spring Boot y Java.

Funcionalidades verificadas:

GET de ventas.

POST de ventas.

GET por identificador.

PUT de ventas.

DELETE de ventas.

Filtros mediante Query Parameters.

Query Parameters disponibles:

cliente
estado
fecha

Ejemplo probado mediante Kong:

http://localhost:8000/ventas?cliente=Cliente%202

La respuesta devolvió únicamente las ventas correspondientes a Cliente 2.

11. RabbitMQ

RabbitMQ actúa como broker de mensajería asíncrona.

El evento implementado en el Caso Práctico 1 es:

VentaCreada

El flujo es:

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

La integración de Publisher y Consumers fue verificada previamente y forma parte de la arquitectura desplegada.

12. Swagger / OpenAPI

Las APIs disponen de documentación OpenAPI/Swagger.

MicroClientes:

http://localhost:8081/swagger

MicroProductos:

http://localhost:8002/docs

MicroVentas:

http://localhost:8082/swagger-ui/index.html

Durante las pruebas se verificaron los endpoints CRUD de los tres microservicios.

13. Pruebas realizadas

MicroClientes

Se realizó un CRUD completo mediante Swagger:

GET     ✅
POST    ✅
GET/{id} ✅
PUT     ✅
DELETE  ✅

También se comprobó la eliminación mediante Kong.

MicroProductos

Se realizó un CRUD completo utilizando un producto temporal:

POST     ✅
GET/{id} ✅
PUT      ✅
DELETE   ✅

También se probó un filtro mediante Query Parameter:

/productos?nombre=Laptop HP

MicroVentas

Se realizó un CRUD completo utilizando una venta temporal:

GET       ✅
POST      ✅
GET/{id}  ✅
PUT       ✅
DELETE    ✅

También se probó un filtro mediante Query Parameter:

/ventas?cliente=Cliente 2

14. Docker Hub

Las versiones finales utilizadas y probadas fueron publicadas en Docker Hub:

kenwaylabs/microclientes-api:2.0
kenwaylabs/microproductos-api:2.0
kenwaylabs/microventas-api:2.0

Las imágenes 2.0 publicadas fueron verificadas y coinciden con las imágenes locales utilizadas durante las pruebas.

15. GitHub

El proyecto se encuentra versionado en GitHub:

https://github.com/Fantasmatico-jpg/microventas-api

El repositorio contiene:

MicroClientes.

MicroProductos.

MicroVentas.

docker-compose.yml.

kong.yml.

README.md.

Configuración y archivos necesarios para la arquitectura.

El repositorio quedó sincronizado con la rama master.

16. Resultados obtenidos

Se logró desplegar una arquitectura distribuida compuesta por:

MicroClientes
MicroProductos
MicroVentas
SQL Server
MySQL
PostgreSQL
RabbitMQ
Kong API Gateway

Las pruebas realizadas demostraron:

Funcionamiento de los tres microservicios.

Funcionamiento de las tres bases de datos.

Funcionamiento de RabbitMQ.

Funcionamiento de Kong API Gateway.

Acceso mediante /clientes, /productos y /ventas.

CRUD funcional en los tres microservicios.

Funcionamiento de Query Parameters.

Comunicación asíncrona mediante eventos.

Publicación de las imágenes finales en Docker Hub.

Actualización del repositorio GitHub.

17. Dificultades encontradas

Durante la implementación se presentaron dificultades relacionadas principalmente con la integración de Docker, redes y contenedores previamente existentes.

También fue necesario ajustar la configuración de Kong para que pudiera resolver correctamente los nombres de los microservicios dentro de la red Docker compartida.

Finalmente, la arquitectura fue estabilizada y se verificó el funcionamiento conjunto de todos los componentes.

18. Conclusiones

La implementación permitió desplegar una arquitectura distribuida completa utilizando Docker Compose como mecanismo de orquestación.

Kong API Gateway permitió centralizar el acceso externo a los microservicios mediante las rutas /clientes, /productos y /ventas.

RabbitMQ permitió mantener la comunicación basada en eventos utilizada entre MicroVentas, MicroProductos y MicroClientes.

Las pruebas realizadas demostraron que los microservicios, las bases de datos, el API Gateway y el broker de mensajería pueden funcionar coordinadamente dentro de una arquitectura distribuida.

19. Recomendaciones

Mantener una red Docker común para los componentes que necesiten comunicarse.

Utilizar nombres de servicio de Docker en las configuraciones internas.

Mantener las imágenes finales versionadas en Docker Hub.

Mantener actualizado el repositorio GitHub.

Documentar las rutas de Kong y los endpoints de cada microservicio.

Mantener evidencias de las pruebas realizadas.

20. Evidencias

Para el informe del Caso Práctico 2 se recomienda incluir:

Arquitectura final.

docker-compose.yml.

kong.yml.

Contenedores activos.

Red Docker.

Volúmenes Docker.

Swagger MicroClientes.

Swagger MicroProductos.

Swagger MicroVentas.

CRUD Clientes.

CRUD Productos.

CRUD Ventas.

Query Parameters.

RabbitMQ.

Queues y Exchanges.

Mensajes publicados y consumidos.

Pruebas mediante Kong.

Docker Hub de los tres microservicios.

Repositorio GitHub.

21. Tecnologías utilizadas

Docker

Docker Compose

Kong 3.9

RabbitMQ

.NET 8

ASP.NET Core

Entity Framework Core

FastAPI

Python

SQLAlchemy

Java 21

Spring Boot

Hibernate

SQL Server 2022

MySQL 8

PostgreSQL 16

Swagger / OpenAPI

Git

GitHub

Docker Hub