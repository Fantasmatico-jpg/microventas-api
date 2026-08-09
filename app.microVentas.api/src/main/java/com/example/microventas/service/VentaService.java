package com.example.microventas.service;

import com.example.microventas.dto.VentaRequestDTO;
import com.example.microventas.dto.VentaResponseDTO;
import com.example.microventas.entity.Venta;
import com.example.microventas.repository.VentaRepository;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.List;

@Service
public class VentaService implements IVentaService {

    private final VentaRepository ventaRepository;

    public VentaService(VentaRepository ventaRepository) {
        this.ventaRepository = ventaRepository;
    }

    @Override
    public List<VentaResponseDTO> obtenerTodas() {
        return ventaRepository.findAll()
                .stream()
                .map(this::convertirAResponse)
                .toList();
    }

    @Override
    public VentaResponseDTO obtenerPorId(Long id) {
        Venta venta = ventaRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Venta no encontrada"));

        return convertirAResponse(venta);
    }

    @Override
    public VentaResponseDTO crear(VentaRequestDTO request) {
        Venta venta = convertirAEntity(request);
        Venta guardada = ventaRepository.save(venta);

        return convertirAResponse(guardada);
    }

    @Override
    public VentaResponseDTO actualizar(Long id, VentaRequestDTO request) {
        Venta venta = ventaRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Venta no encontrada"));

        venta.setNumeroVenta(request.getNumeroVenta());
        venta.setFecha(request.getFecha());
        venta.setCliente(request.getCliente());
        venta.setTotal(request.getTotal());
        venta.setEstado(request.getEstado());
        venta.setObservacion(request.getObservacion());

        Venta actualizada = ventaRepository.save(venta);

        return convertirAResponse(actualizada);
    }

    @Override
    public void eliminar(Long id) {
        if (!ventaRepository.existsById(id)) {
            throw new RuntimeException("Venta no encontrada");
        }

        ventaRepository.deleteById(id);
    }

    @Override
    public List<VentaResponseDTO> buscarPorCliente(String cliente) {
        return ventaRepository.findByClienteContainingIgnoreCase(cliente)
                .stream()
                .map(this::convertirAResponse)
                .toList();
    }

    @Override
    public List<VentaResponseDTO> buscarPorEstado(Boolean estado) {
        return ventaRepository.findByEstado(estado)
                .stream()
                .map(this::convertirAResponse)
                .toList();
    }

    @Override
    public List<VentaResponseDTO> buscarPorFecha(LocalDate fecha) {
        return ventaRepository.findByFecha(fecha)
                .stream()
                .map(this::convertirAResponse)
                .toList();
    }

    private Venta convertirAEntity(VentaRequestDTO request) {
        Venta venta = new Venta();

        venta.setNumeroVenta(request.getNumeroVenta());
        venta.setFecha(request.getFecha());
        venta.setCliente(request.getCliente());
        venta.setTotal(request.getTotal());
        venta.setEstado(request.getEstado());
        venta.setObservacion(request.getObservacion());

        return venta;
    }

    private VentaResponseDTO convertirAResponse(Venta venta) {
        return new VentaResponseDTO(
                venta.getId(),
                venta.getNumeroVenta(),
                venta.getFecha(),
                venta.getCliente(),
                venta.getTotal(),
                venta.getEstado(),
                venta.getObservacion()
        );
    }
}