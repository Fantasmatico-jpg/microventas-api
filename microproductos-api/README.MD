# MicroProductos API

Microservicio desarrollado para la gestión de productos utilizando FastAPI, SQLAlchemy y MySQL. El proyecto implementa una API REST con operaciones CRUD, documentación automática mediante Swagger, dockerización e integración con Kong API Gateway.

---

# Tecnologías utilizadas

- Python 3.12
- FastAPI
- SQLAlchemy
- Pydantic
- MySQL 8.0
- PyMySQL
- Docker
- Docker Compose
- Kong API Gateway
- Uvicorn

---

# Arquitectura

Cliente
↓
Kong API Gateway
↓
MicroProductos API (FastAPI)
↓
SQLAlchemy
↓
MySQL

---

# Instalación de dependencias

Crear un entorno virtual:

```bash
python -m venv venv
```

Activar el entorno virtual:

Windows

```bash
venv\Scripts\activate
```

Instalar dependencias:

```bash
pip install -r requirements.txt
```

---

# Ejecución local

Ejecutar el microservicio:

```bash
uvicorn app.main:app --reload --port 8002
```

Swagger:

```
http://localhost:8002/docs
```

Health Check:

```
http://localhost:8002/health
```

---

# Construcción de la imagen Docker

Construir la imagen:

```bash
docker build -t microproductos-api:1.0 .
```

Verificar:

```bash
docker images
```

---

# Ejecución con Docker Compose

Levantar los servicios:

```bash
docker compose up -d --build
```

Verificar los contenedores:

```bash
docker compose ps
```

---

# Endpoints disponibles

## Health Check

```
GET /health
```

## Productos

```
GET /api/productos
```

```
GET /api/productos/{id}
```

```
POST /api/productos
```

```
PUT /api/productos/{id}
```

```
DELETE /api/productos/{id}
```

### Query Parameters

```
GET /api/productos?nombre=Laptop
```

```
GET /api/productos?categoria=Tecnología
```

```
GET /api/productos?estado=true
```

---

# URL mediante Kong

```
GET http://localhost:8000/productos
```

---

# Docker Hub

Repositorio:

```
https://hub.docker.com/r/kenwaylabs/microproductos-api
```

---

# GitHub

Repositorio:

```
https://github.com/Fantasmatico-jpg/microproductos-api
```
## Comunicación asíncrona con RabbitMQ

MicroProductos funciona como Consumer.

El servicio permanece escuchando la cola:

productos.stock

Cuando recibe un evento VentaCreadaEvent:

1. Lee el mensaje.
2. Obtiene el producto.
3. Verifica el stock.
4. Descuenta la cantidad.
5. Guarda el cambio en MySQL.
6. Confirma el mensaje mediante ACK.