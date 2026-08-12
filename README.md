# Caso Práctico 1 U3
## Integración de microservicios mediante eventos

Proyecto de Aplicaciones Distribuidas – Unidad 3.

Este proyecto implementa una arquitectura de microservicios basada en eventos utilizando RabbitMQ como broker de mensajería. La solución integra tres microservicios desarrollados con diferentes tecnologías:

- **Microservicio Clientes:** .NET 8 / ASP.NET Core
- **Microservicio Productos:** FastAPI / Python
- **Microservicio Ventas:** Spring Boot / Java
- **Broker de mensajería:** RabbitMQ
- **API Gateway:** Kong

---

## 1. Objetivo

Integrar los microservicios de Clientes, Productos y Ventas mediante eventos publicados y consumidos a través de RabbitMQ, evitando la comunicación HTTP directa entre los microservicios para el procesamiento de una venta.

Cuando se registra una nueva venta, MicroVentas publica un evento `VentaCreada`. RabbitMQ distribuye este evento hacia MicroProductos y MicroClientes.

Cada consumidor reacciona de manera independiente:

- MicroProductos disminuye el stock.
- MicroClientes registra la compra en el historial del cliente.

---

## 2. Arquitectura

La arquitectura implementada es:

```text
                         Frontend
                            |
                            v
                     Kong API Gateway
                            |
                            v
                    +---------------+
                    |  MicroVentas  |
                    |   Publisher   |
                    +-------+-------+
                            |
                     Evento VentaCreada
                            |
                            v
                    +---------------+
                    |    RabbitMQ    |
                    |     Broker     |
                    +-------+-------+
                            |
                +-----------+-----------+
                |                       |
                v                       v
      +------------------+    +------------------+
      | MicroProductos   |    |  MicroClientes   |
      |    Consumer      |    |    Consumer      |
      +--------+---------+    +--------+---------+
               |                       |
               v                       v
      +------------------+    +------------------+
      |      MySQL       |    |    SQL Server    |
      | MicroProductosDB |    |   microCliente   |
      +------------------+    +------------------+

                    MicroVentas
                         |
                         v
                   PostgreSQL
                 MicroVentasDB

3. Microservicios
MicroVentas

Tecnología:

Spring Boot
Java
PostgreSQL
RabbitMQ

Responsabilidad:

Registrar la venta.
Guardar la venta en PostgreSQL.
Crear el evento VentaCreada.
Publicar el evento en RabbitMQ.
MicroProductos

Tecnología:

FastAPI
Python
MySQL
Pika

Responsabilidad:

Consumir el evento VentaCreada.
Identificar el producto vendido.
Validar el stock disponible.
Disminuir automáticamente el stock.
Confirmar el procesamiento del mensaje.
MicroClientes

Tecnología:

.NET 8
ASP.NET Core
Entity Framework Core
SQL Server
RabbitMQ.Client

Responsabilidad:

Consumir el evento VentaCreada.
Crear el registro de historial de compra.
Guardar la información en SQL Server.
Confirmar el procesamiento del evento.
4. RabbitMQ

RabbitMQ funciona como broker central de mensajería.

Exchange
ventas.exchange

Tipo:

direct
Routing Key
ventas.creada
Queues
productos.stock
microclientes.historial

La configuración permite que un mismo evento sea recibido por ambos consumidores.

                    ventas.exchange
                          |
                    ventas.creada
                     /          \
                    /            \
                   v              v
          productos.stock   microclientes.historial
                |                    |
                v                    v
        MicroProductos        MicroClientes
5. Evento VentaCreada

El evento utilizado para integrar los microservicios tiene la siguiente estructura:

{
  "ventaId": 47,
  "clienteId": 1,
  "productoId": 4,
  "cantidad": 1,
  "total": 1200.50,
  "fecha": "2026-08-12"
}
Campos
Campo	Descripción
ventaId	Identificador de la venta
clienteId	Identificador del cliente
productoId	Identificador del producto
cantidad	Cantidad vendida
total	Total de la venta
fecha	Fecha de la compra
6. Flujo de eventos

El flujo implementado es:

1. Cliente registra una venta
              |
              v
2. MicroVentas guarda la venta
              |
              v
3. MicroVentas publica VentaCreada
              |
              v
4. RabbitMQ recibe el evento
              |
        +-----+-----+
        |           |
        v           v
5. MicroProductos  6. MicroClientes
        |           |
        v           v
7. Actualiza       8. Registra
   stock               historial

No existe una llamada HTTP directa entre MicroVentas, MicroProductos y MicroClientes para procesar la venta. La integración se realiza mediante el evento publicado en RabbitMQ.

7. Docker Compose

El proyecto dispone de un docker-compose.yml central que integra los componentes de la arquitectura.

Servicios incluidos:

postgres
mysql
sqlserver
rabbitmq
microventas
microproductos
microclientes

Para iniciar la plataforma:

docker compose up -d

Para comprobar el estado de los servicios:

docker compose ps

Para detener los servicios:

docker compose down
8. Puertos utilizados
Servicio	Puerto
MicroClientes	8081
MicroVentas	8082
MicroProductos	8002
RabbitMQ AMQP	5672
RabbitMQ Management	15672
PostgreSQL	5435
MySQL	3307
SQL Server	1434
Kong	8000 / 8001
9. RabbitMQ Management

La consola de administración de RabbitMQ está disponible en:

http://localhost:15672

Credenciales utilizadas durante el desarrollo:

Usuario: admin
Contraseña: admin

Desde esta consola se pueden revisar:

Exchanges
Queues
Bindings
Connections
Channels
Consumers
Estadísticas de mensajes
10. Swagger
MicroVentas
http://localhost:8082/swagger-ui/index.html
MicroClientes
http://localhost:8081/swagger

MicroProductos expone su API mediante FastAPI.

11. Prueba de integración

Se realizó una prueba completa utilizando una venta registrada desde MicroVentas.

Venta de prueba
VentaId: 47
ClienteId: 1
ProductoId: 4
Cantidad: 1
Total: 1200.50
Resultado en MicroVentas

MicroVentas publicó:

[RabbitMQ] Evento VentaCreada publicado: 47
Resultado en MicroProductos

MicroProductos recibió el evento:

{"ventaId":47,"clienteId":1,"productoId":4,"cantidad":1,"total":1200.50,"fecha":"2026-08-12"}

Y actualizó el stock:

Producto: 4
Stock anterior: 9
Stock nuevo: 8
Resultado en MicroClientes

MicroClientes recibió el mismo evento y registró el historial:

VentaId: 47
ClienteId: 1
ProductoId: 4
Cantidad: 1
Total: 1200.50

El registro fue almacenado correctamente:

Historial guardado correctamente. Id: 2003
12. Resultado de la integración

La prueba permitió comprobar el funcionamiento completo de la arquitectura:

MicroVentas
     |
     | VentaCreada
     v
RabbitMQ
   /   \
  /     \
 v       v
Productos Clientes
   |       |
   v       v
Stock     Historial

Resultados obtenidos:

Publicación del evento VentaCreada.
Recepción del evento por MicroProductos.
Actualización automática del stock.
Recepción del evento por MicroClientes.
Registro automático del historial de compra.
Comunicación asíncrona mediante RabbitMQ.
Integración de los tres microservicios mediante Docker Compose.
13. Evidencias

El proyecto incluye evidencias de:

Arquitectura implementada.
Servicios ejecutándose mediante Docker Compose.
Registro de una venta en Swagger.
Publicación del evento VentaCreada.
Consumo del evento por MicroProductos.
Actualización automática del stock.
Consumo del evento por MicroClientes.
Registro del historial de compras.
Consola de administración de RabbitMQ.
Exchange ventas.exchange.
Queues productos.stock y microclientes.historial.
Routing Key ventas.creada.
Verificación de los datos persistidos en MySQL y SQL Server.
14. Tecnologías utilizadas
.NET 8
ASP.NET Core
Entity Framework Core
Java 21
Spring Boot
Python
FastAPI
RabbitMQ
PostgreSQL 16
MySQL 8
SQL Server 2022
Docker
Docker Compose
Kong
Swagger / OpenAPI
Git / GitHub
15. Repositorio

El proyecto se encuentra versionado mediante Git y actualizado en GitHub.

Repositorio:

https://github.com/Fantasmatico-jpg/microventas-api
16. Conclusión

La implementación permitió integrar tres microservicios mediante una arquitectura basada en eventos.

MicroVentas actúa como Publisher y genera el evento VentaCreada. RabbitMQ funciona como broker y distribuye el evento a los consumidores. MicroProductos actualiza automáticamente el stock y MicroClientes registra el historial de compras.

El resultado demuestra una comunicación asíncrona y desacoplada, donde los consumidores pueden reaccionar al mismo evento sin establecer una comunicación HTTP directa con MicroVentas.