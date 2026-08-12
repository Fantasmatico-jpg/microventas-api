import json
import os
import time

import pika
from sqlalchemy.orm import Session

from app.database import SessionLocal
from app.models import Producto


RABBITMQ_HOST = os.getenv("RABBITMQ_HOST", "localhost")
RABBITMQ_PORT = int(os.getenv("RABBITMQ_PORT", "5672"))
RABBITMQ_USERNAME = os.getenv("RABBITMQ_USERNAME", "admin")
RABBITMQ_PASSWORD = os.getenv("RABBITMQ_PASSWORD", "admin")

EXCHANGE = "ventas.exchange"
QUEUE = "productos.stock"
ROUTING_KEY = "ventas.creada"


def procesar_mensaje(ch, method, properties, body):
    db: Session = SessionLocal()

    try:
        print(f"[RabbitMQ] BODY RECIBIDO: {body}", flush=True)

        mensaje = json.loads(body.decode("utf-8"))

        print(f"[RabbitMQ] Mensaje recibido: {mensaje}", flush=True)

        venta_id = mensaje.get("ventaId")
        producto_id = mensaje.get("productoId")
        cantidad = mensaje.get("cantidad")

        if not venta_id or not producto_id or not cantidad:
            print(
                "[RabbitMQ] Mensaje inválido: faltan datos",
                flush=True
            )

            ch.basic_nack(
                delivery_tag=method.delivery_tag,
                requeue=False
            )
            return

        if cantidad <= 0:
            print(
                "[RabbitMQ] Mensaje inválido: cantidad no válida",
                flush=True
            )

            ch.basic_nack(
                delivery_tag=method.delivery_tag,
                requeue=False
            )
            return

        producto = (
            db.query(Producto)
            .filter(Producto.id == producto_id)
            .first()
        )

        if not producto:
            print(
                f"[RabbitMQ] Producto {producto_id} no encontrado",
                flush=True
            )

            ch.basic_nack(
                delivery_tag=method.delivery_tag,
                requeue=False
            )
            return

        if producto.stock < cantidad:
            print(
                f"[RabbitMQ] Stock insuficiente. "
                f"Stock actual: {producto.stock}, "
                f"cantidad solicitada: {cantidad}",
                flush=True
            )

            ch.basic_nack(
                delivery_tag=method.delivery_tag,
                requeue=False
            )
            return

        stock_anterior = producto.stock

        producto.stock -= cantidad

        db.commit()
        db.refresh(producto)

        print(
            f"[RabbitMQ] Stock actualizado. "
            f"Producto: {producto.id} | "
            f"Anterior: {stock_anterior} | "
            f"Nuevo: {producto.stock}",
            flush=True
        )

        ch.basic_ack(
            delivery_tag=method.delivery_tag
        )

    except json.JSONDecodeError:
        print(
            "[RabbitMQ] Error: mensaje JSON inválido",
            flush=True
        )

        ch.basic_nack(
            delivery_tag=method.delivery_tag,
            requeue=False
        )

    except Exception as e:
        db.rollback()

        print(
            f"[RabbitMQ] Error procesando mensaje: {e}",
            flush=True
        )

        ch.basic_nack(
            delivery_tag=method.delivery_tag,
            requeue=True
        )

    finally:
        db.close()


def iniciar_consumer():

    credentials = pika.PlainCredentials(
        RABBITMQ_USERNAME,
        RABBITMQ_PASSWORD
    )

    parameters = pika.ConnectionParameters(
        host=RABBITMQ_HOST,
        port=RABBITMQ_PORT,
        credentials=credentials
    )

    while True:
        try:
            print(
                "[RabbitMQ] Intentando conectar...",
                flush=True
            )

            connection = pika.BlockingConnection(
                parameters
            )

            print(
                "[RabbitMQ] Conexión establecida correctamente",
                flush=True
            )

            break

        except Exception as e:
            print(
                f"[RabbitMQ] No se pudo conectar: {e}. "
                "Reintentando en 5 segundos...",
                flush=True
            )

            time.sleep(5)

    channel = connection.channel()

    # Declarar Exchange
    channel.exchange_declare(
        exchange=EXCHANGE,
        exchange_type="direct",
        durable=True
    )

    # Declarar Queue
    channel.queue_declare(
        queue=QUEUE,
        durable=True
    )

    # Vincular Queue con Exchange
    channel.queue_bind(
        exchange=EXCHANGE,
        queue=QUEUE,
        routing_key=ROUTING_KEY
    )

    # Procesar un mensaje a la vez
    channel.basic_qos(
        prefetch_count=1
    )

    # Registrar Consumer
    channel.basic_consume(
        queue=QUEUE,
        on_message_callback=procesar_mensaje,
        auto_ack=False
    )

    print(
        "[RabbitMQ] Consumer iniciado",
        flush=True
    )

    print(
        f"[RabbitMQ] Exchange: {EXCHANGE}",
        flush=True
    )

    print(
        f"[RabbitMQ] Queue: {QUEUE}",
        flush=True
    )

    print(
        f"[RabbitMQ] Routing Key: {ROUTING_KEY}",
        flush=True
    )

    print(
        "[RabbitMQ] Esperando mensajes...",
        flush=True
    )

    channel.start_consuming()