package com.example.microventas.dto;

import java.time.LocalDate;

public class VentaCreadaEvent {

    private Long ventaId;
    private Long productoId;
    private Integer cantidad;
    private LocalDate fecha;

    public VentaCreadaEvent() {
    }

    public VentaCreadaEvent(
            Long ventaId,
            Long productoId,
            Integer cantidad,
            LocalDate fecha) {

        this.ventaId = ventaId;
        this.productoId = productoId;
        this.cantidad = cantidad;
        this.fecha = fecha;
    }

    public Long getVentaId() {
        return ventaId;
    }

    public void setVentaId(Long ventaId) {
        this.ventaId = ventaId;
    }

    public Long getProductoId() {
        return productoId;
    }

    public void setProductoId(Long productoId) {
        this.productoId = productoId;
    }

    public Integer getCantidad() {
        return cantidad;
    }

    public void setCantidad(Integer cantidad) {
        this.cantidad = cantidad;
    }

    public LocalDate getFecha() {
        return fecha;
    }

    public void setFecha(LocalDate fecha) {
        this.fecha = fecha;
    }
}