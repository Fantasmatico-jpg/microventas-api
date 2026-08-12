using app.microCliente.common.DTOs;
using app.microCliente.dataAccess.repositories;
using app.microCliente.entities.models;
using app.microCliente.services.Interfaces;

namespace app.microCliente.services.Implementations
{
    public class HistorialCompraService : IHistorialCompraService
    {
        private readonly IHistorialCompraRepository _repository;

        public HistorialCompraService(
            IHistorialCompraRepository repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<HistorialCompraDTO>> Insertar(
            HistorialCompraDTO dto)
        {
            var response = new BaseResponse<HistorialCompraDTO>();

            try
            {
                var entity = new HistorialCompra
                {
                    ClienteId = dto.ClienteId,
                    VentaId = dto.VentaId,
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad,
                    Total = dto.Total,
                    FechaCompra = dto.FechaCompra,
                    Estado = true,
                    Fecha = DateTime.Now
                };

                entity = await _repository.Insertar(entity);

                dto.Id = entity.Id;
                dto.Estado = entity.Estado;

                response.Result = dto;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<HistorialCompraDTO>> SeleccionarUno(
            int id)
        {
            var response = new BaseResponse<HistorialCompraDTO>();

            try
            {
                var entity = await _repository.SeleccionarUno(id);

                if (entity == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "Historial de compra no encontrado";
                    return response;
                }

                response.Result = MapearDTO(entity);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<List<HistorialCompraDTO>>> SeleccionarTodos()
        {
            var response = new BaseResponse<List<HistorialCompraDTO>>();

            try
            {
                var lista = await _repository.SeleccionarTodos();

                response.Result = lista
                    .Select(MapearDTO)
                    .ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<List<HistorialCompraDTO>>> SeleccionarPorCliente(
            int clienteId)
        {
            var response = new BaseResponse<List<HistorialCompraDTO>>();

            try
            {
                var lista = await _repository.SeleccionarPorCliente(clienteId);

                response.Result = lista
                    .Select(MapearDTO)
                    .ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<HistorialCompraDTO>> Actualizar(
            int id,
            HistorialCompraDTO dto)
        {
            var response = new BaseResponse<HistorialCompraDTO>();

            try
            {
                var entity = new HistorialCompra
                {
                    Id = id,
                    ClienteId = dto.ClienteId,
                    VentaId = dto.VentaId,
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad,
                    Total = dto.Total,
                    FechaCompra = dto.FechaCompra,
                    Estado = dto.Estado,
                    Fecha = DateTime.Now
                };

                await _repository.Actualizar(entity);

                dto.Id = id;

                response.Result = dto;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<string>> Eliminar(int id)
        {
            var response = new BaseResponse<string>();

            try
            {
                await _repository.Eliminar(id);

                response.Result = "OK";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        private static HistorialCompraDTO MapearDTO(
            HistorialCompra entity)
        {
            return new HistorialCompraDTO
            {
                Id = entity.Id,
                ClienteId = entity.ClienteId,
                VentaId = entity.VentaId,
                ProductoId = entity.ProductoId,
                Cantidad = entity.Cantidad,
                Total = entity.Total,
                FechaCompra = entity.FechaCompra,
                Estado = entity.Estado
            };
        }
    }
}