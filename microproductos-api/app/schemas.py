from datetime import datetime
from decimal import Decimal
from typing import Optional

from pydantic import BaseModel, ConfigDict, Field


class ProductoBase(BaseModel):
    nombre: str = Field(min_length=2, max_length=100)
    descripcion: Optional[str] = Field(default=None, max_length=250)
    precio: Decimal = Field(gt=0)
    stock: int = Field(ge=0)
    categoria: str = Field(min_length=2, max_length=80)
    estado: bool = True


class ProductoCreate(ProductoBase):
    pass


class ProductoUpdate(BaseModel):
    nombre: Optional[str] = Field(default=None, min_length=2, max_length=100)
    descripcion: Optional[str] = Field(default=None, max_length=250)
    precio: Optional[Decimal] = Field(default=None, gt=0)
    stock: Optional[int] = Field(default=None, ge=0)
    categoria: Optional[str] = Field(default=None, min_length=2, max_length=80)
    estado: Optional[bool] = None


class ProductoResponse(ProductoBase):
    model_config = ConfigDict(from_attributes=True)

    id: int
    fecha_registro: datetime
    fecha_actualizacion: datetime