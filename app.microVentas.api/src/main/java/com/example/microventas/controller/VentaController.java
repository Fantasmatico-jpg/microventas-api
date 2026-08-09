package com.example.microventas.controller;

import com.example.microventas.dto.VentaRequestDTO;
import com.example.microventas.dto.VentaResponseDTO;
import com.example.microventas.service.IVentaService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.util.List;

@RestController
@RequestMapping("/api/ventas")
public class VentaController {

    private final IVentaService ventaService;

    public VentaController(IVentaService ventaService) {
        this.ventaService = ventaService;
    }

    @GetMapping
    public ResponseEntity<List<VentaResponseDTO>> obtenerVentas(
            @RequestParam(required = false) String cliente,
            @RequestParam(required = false) Boolean estado,
            @RequestParam(required = false) LocalDate fecha) {

        if (cliente != null) {
            return ResponseEntity.ok(ventaService.buscarPorCliente(cliente));
        }

        if (estado != null) {
            return ResponseEntity.ok(ventaService.buscarPorEstado(estado));
        }

        if (fecha != null) {
            return ResponseEntity.ok(ventaService.buscarPorFecha(fecha));
        }

        return ResponseEntity.ok(ventaService.obtenerTodas());
    }

    @GetMapping("/{id}")
    public ResponseEntity<VentaResponseDTO> obtenerPorId(@PathVariable Long id) {
        return ResponseEntity.ok(ventaService.obtenerPorId(id));
    }

    @PostMapping
    public ResponseEntity<VentaResponseDTO> crear(
            @RequestBody VentaRequestDTO request) {

        VentaResponseDTO respuesta = ventaService.crear(request);

        return ResponseEntity
                .status(HttpStatus.CREATED)
                .body(respuesta);
    }

    @PutMapping("/{id}")
    public ResponseEntity<VentaResponseDTO> actualizar(
            @PathVariable Long id,
            @RequestBody VentaRequestDTO request) {

        return ResponseEntity.ok(
                ventaService.actualizar(id, request)
        );
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> eliminar(@PathVariable Long id) {

        ventaService.eliminar(id);

        return ResponseEntity.noContent().build();
    }
}