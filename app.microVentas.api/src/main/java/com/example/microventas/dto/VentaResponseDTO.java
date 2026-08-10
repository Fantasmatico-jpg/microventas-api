package com.example.microventas.dto;

import java.math.BigDecimal;
import java.time.LocalDate;

public class VentaResponseDTO {

    private Long id;
    private String numeroVenta;
    private LocalDate fecha;
    private String cliente;
    private Long productoId;
    private Integer cantidad;
    private BigDecimal total;
    private Boolean estado;
    private String observacion;

    public VentaResponseDTO() {
    }

    public VentaResponseDTO(
            Long id,
            String numeroVenta,
            LocalDate fecha,
            String cliente,
            Long productoId,
            Integer cantidad,
            BigDecimal total,
            Boolean estado,
            String observacion) {

        this.id = id;
        this.numeroVenta = numeroVenta;
        this.fecha = fecha;
        this.cliente = cliente;
        this.productoId = productoId;
        this.cantidad = cantidad;
        this.total = total;
        this.estado = estado;
        this.observacion = observacion;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getNumeroVenta() {
        return numeroVenta;
    }

    public void setNumeroVenta(String numeroVenta) {
        this.numeroVenta = numeroVenta;
    }

    public LocalDate getFecha() {
        return fecha;
    }

    public void setFecha(LocalDate fecha) {
        this.fecha = fecha;
    }

    public String getCliente() {
        return cliente;
    }

    public void setCliente(String cliente) {
        this.cliente = cliente;
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

    public BigDecimal getTotal() {
        return total;
    }

    public void setTotal(BigDecimal total) {
        this.total = total;
    }

    public Boolean getEstado() {
        return estado;
    }

    public void setEstado(Boolean estado) {
        this.estado = estado;
    }

    public String getObservacion() {
        return observacion;
    }

    public void setObservacion(String observacion) {
        this.observacion = observacion;
    }
}