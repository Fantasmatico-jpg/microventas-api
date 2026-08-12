from contextlib import asynccontextmanager
from threading import Thread

from fastapi import FastAPI

from app.database import Base, engine
from app.routers.productos import router as productos_router
from app.rabbitmq_consumer import iniciar_consumer


# Crear las tablas
Base.metadata.create_all(bind=engine)


# Iniciar RabbitMQ cuando arranque FastAPI
@asynccontextmanager
async def lifespan(app: FastAPI):

    print("[MicroProductos] Iniciando RabbitMQ Consumer...", flush=True)

    consumer_thread = Thread(
        target=iniciar_consumer,
        daemon=True
    )

    consumer_thread.start()

    print("[MicroProductos] RabbitMQ Consumer iniciado en segundo plano", flush=True)

    yield

    print("[MicroProductos] Aplicación cerrándose...", flush=True)


app = FastAPI(
    title="MicroProductos API",
    description="Microservicio para la gestión de productos",
    version="1.0.0",
    lifespan=lifespan
)


@app.get("/")
def root():
    return {
        "mensaje": "MicroProductos API funcionando correctamente"
    }


@app.get("/health")
def health():
    return {
        "status": "OK"
    }


app.include_router(productos_router) 