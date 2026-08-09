package com.example.microventas.service;

import com.example.microventas.dto.VentaRequestDTO;
import com.example.microventas.dto.VentaResponseDTO;

import java.time.LocalDate;
import java.util.List;

public interface IVentaService {

    List<VentaResponseDTO> obtenerTodas();

    VentaResponseDTO obtenerPorId(Long id);

    VentaResponseDTO crear(VentaRequestDTO request);

    VentaResponseDTO actualizar(Long id, VentaRequestDTO request);

    void eliminar(Long id);

    List<VentaResponseDTO> buscarPorCliente(String cliente);

    List<VentaResponseDTO> buscarPorEstado(Boolean estado);

    List<VentaResponseDTO> buscarPorFecha(LocalDate fecha);
}