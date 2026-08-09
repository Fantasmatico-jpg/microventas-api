package com.example.microventas.repository;

import com.example.microventas.entity.Venta;
import org.springframework.data.jpa.repository.JpaRepository;

import java.time.LocalDate;
import java.util.List;

public interface VentaRepository extends JpaRepository<Venta, Long> {

    List<Venta> findByClienteContainingIgnoreCase(String cliente);

    List<Venta> findByEstado(Boolean estado);

    List<Venta> findByFecha(LocalDate fecha);
}