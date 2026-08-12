from sqlalchemy import Boolean, Column, DateTime, Integer, Numeric, String
from sqlalchemy.sql import func

from app.database import Base


class Producto(Base):
    __tablename__ = "productos"

    id = Column(Integer, primary_key=True, index=True)
    nombre = Column(String(100), nullable=False)
    descripcion = Column(String(250), nullable=True)
    precio = Column(Numeric(10, 2), nullable=False)
    stock = Column(Integer, nullable=False, default=0)
    categoria = Column(String(80), nullable=False)
    estado = Column(Boolean, nullable=False, default=True)

    fecha_registro = Column(
        DateTime,
        nullable=False,
        server_default=func.now()
    )

    fecha_actualizacion = Column(
        DateTime,
        nullable=False,
        server_default=func.now(),
        onupdate=func.now()
    )