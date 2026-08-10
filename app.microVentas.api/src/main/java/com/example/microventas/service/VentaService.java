package com.example.microventas.service;

import com.example.microventas.dto.VentaCreadaEvent;
import com.example.microventas.dto.VentaRequestDTO;
import com.example.microventas.dto.VentaResponseDTO;
import com.example.microventas.entity.Venta;
import com.example.microventas.repository.VentaRepository;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

import java.time.LocalDate;
import java.util.List;

@Service
public class VentaService implements IVentaService {

    private final VentaRepository ventaRepository;
    private final RabbitMQPublisher rabbitMQPublisher;

    public VentaService(
            VentaRepository ventaRepository,
            RabbitMQPublisher rabbitMQPublisher) {

        this.ventaRepository = ventaRepository;
        this.rabbitMQPublisher = rabbitMQPublisher;
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
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Venta no encontrada"
                ));

        return convertirAResponse(venta);
    }

    @Override
    public VentaResponseDTO crear(VentaRequestDTO request) {

        // Guardar la venta en PostgreSQL
        Venta venta = convertirAEntity(request);

        Venta guardada = ventaRepository.save(venta);

        // Crear evento de venta
        VentaCreadaEvent evento = new VentaCreadaEvent(
                guardada.getId(),
                guardada.getProductoId(),
                guardada.getCantidad(),
                guardada.getFecha()
        );

        // Publicar evento en RabbitMQ
        rabbitMQPublisher.publicarVentaCreada(evento);

        return convertirAResponse(guardada);
    }

    @Override
    public VentaResponseDTO actualizar(
            Long id,
            VentaRequestDTO request) {

        Venta venta = ventaRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Venta no encontrada"
                ));

        venta.setNumeroVenta(request.getNumeroVenta());
        venta.setFecha(request.getFecha());
        venta.setCliente(request.getCliente());
        venta.setProductoId(request.getProductoId());
        venta.setCantidad(request.getCantidad());
        venta.setTotal(request.getTotal());
        venta.setEstado(request.getEstado());
        venta.setObservacion(request.getObservacion());

        Venta actualizada = ventaRepository.save(venta);

        return convertirAResponse(actualizada);
    }

    @Override
    public void eliminar(Long id) {

        if (!ventaRepository.existsById(id)) {
            throw new ResponseStatusException(
                    HttpStatus.NOT_FOUND,
                    "Venta no encontrada"
            );
        }

        ventaRepository.deleteById(id);
    }

    @Override
    public List<VentaResponseDTO> buscarPorCliente(
            String cliente) {

        return ventaRepository
                .findByClienteContainingIgnoreCase(cliente)
                .stream()
                .map(this::convertirAResponse)
                .toList();
    }

    @Override
    public List<VentaResponseDTO> buscarPorEstado(
            Boolean estado) {

        return ventaRepository
                .findByEstado(estado)
                .stream()
                .map(this::convertirAResponse)
                .toList();
    }

    @Override
    public List<VentaResponseDTO> buscarPorFecha(
            LocalDate fecha) {

        return ventaRepository
                .findByFecha(fecha)
                .stream()
                .map(this::convertirAResponse)
                .toList();
    }

    private Venta convertirAEntity(VentaRequestDTO request) {

        Venta venta = new Venta();

        venta.setNumeroVenta(request.getNumeroVenta());
        venta.setFecha(request.getFecha());
        venta.setCliente(request.getCliente());
        venta.setProductoId(request.getProductoId());
        venta.setCantidad(request.getCantidad());
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
                venta.getProductoId(),
                venta.getCantidad(),
                venta.getTotal(),
                venta.getEstado(),
                venta.getObservacion()
        );
    }
}