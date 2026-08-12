using app.microCliente.common.DTOs;

namespace app.microCliente.services.Interfaces
{
    public interface IHistorialCompraService
    {
        Task<BaseResponse<HistorialCompraDTO>> Insertar(
            HistorialCompraDTO historialDTO);

        Task<BaseResponse<HistorialCompraDTO>> SeleccionarUno(
            int id);

        Task<BaseResponse<List<HistorialCompraDTO>>> SeleccionarTodos();

        Task<BaseResponse<List<HistorialCompraDTO>>> SeleccionarPorCliente(
            int clienteId);

        Task<BaseResponse<HistorialCompraDTO>> Actualizar(
            int id,
            HistorialCompraDTO historialDTO);

        Task<BaseResponse<string>> Eliminar(int id);
    }
}