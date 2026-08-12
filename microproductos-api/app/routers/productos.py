from typing import Optional

from fastapi import APIRouter, Depends, HTTPException, Query, status
from sqlalchemy.orm import Session

from app.database import get_db
from app.models import Producto
from app.repositories import ProductoRepository
from app.schemas import (
    ProductoCreate,
    ProductoResponse,
    ProductoUpdate
)

router = APIRouter(
    prefix="/api/productos",
    tags=["Productos"]
)


@router.get(
    "",
    response_model=list[ProductoResponse]
)
def obtener_productos(
    nombre: Optional[str] = Query(default=None),
    categoria: Optional[str] = Query(default=None),
    estado: Optional[bool] = Query(default=None),
    db: Session = Depends(get_db)
):
    consulta = db.query(Producto) 
    if nombre:
        consulta = consulta.filter(
            Producto.nombre.contains(nombre)
        )

    if categoria:
        consulta = consulta.filter(
            Producto.categoria == categoria
        )

    if estado is not None:
        consulta = consulta.filter(
            Producto.estado == estado
        )

    return consulta.all()


@router.get(
    "/{producto_id}",
    response_model=ProductoResponse
)
def obtener_producto(
    producto_id: int,
    db: Session = Depends(get_db)
):
    producto = ProductoRepository.obtener_por_id(db, producto_id)

    if not producto:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Producto no encontrado"
        )

    return producto


@router.post(
    "",
    response_model=ProductoResponse,
    status_code=status.HTTP_201_CREATED
)
def crear_producto(
    datos: ProductoCreate,
    db: Session = Depends(get_db)
):
    return ProductoRepository.crear(db, datos)


@router.put(
    "/{producto_id}",
    response_model=ProductoResponse
)
def actualizar_producto(
    producto_id: int,
    datos: ProductoUpdate,
    db: Session = Depends(get_db)
):
    producto = ProductoRepository.obtener_por_id(db, producto_id)

    if not producto:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Producto no encontrado"
        )

    return ProductoRepository.actualizar(
        db,
        producto,
        datos
    )


@router.delete(
    "/{producto_id}",
    status_code=status.HTTP_204_NO_CONTENT
)
def eliminar_producto(
    producto_id: int,
    db: Session = Depends(get_db)
):
    producto = ProductoRepository.obtener_por_id(db, producto_id)

    if not producto:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Producto no encontrado"
        )

    ProductoRepository.eliminar(db, producto)