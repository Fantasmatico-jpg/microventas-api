from sqlalchemy.orm import Session

from app.models import Producto
from app.schemas import ProductoCreate, ProductoUpdate


class ProductoRepository:

    @staticmethod
    def obtener_todos(db: Session):
        return db.query(Producto).all()

    @staticmethod
    def obtener_por_id(db: Session, producto_id: int):
        return (
            db.query(Producto)
            .filter(Producto.id == producto_id)
            .first()
        )

    @staticmethod
    def crear(db: Session, datos: ProductoCreate):
        producto = Producto(**datos.model_dump())

        db.add(producto)
        db.commit()
        db.refresh(producto)

        return producto

    @staticmethod
    def actualizar(
        db: Session,
        producto: Producto,
        datos: ProductoUpdate
    ):
        cambios = datos.model_dump(exclude_unset=True)

        for campo, valor in cambios.items():
            setattr(producto, campo, valor)

        db.commit()
        db.refresh(producto)

        return producto

    @staticmethod
    def eliminar(db: Session, producto: Producto):
        db.delete(producto)
        db.commit()