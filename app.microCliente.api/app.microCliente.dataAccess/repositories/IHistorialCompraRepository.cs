using app.microCliente.entities.models;

namespace app.microCliente.dataAccess.repositories
{
    public interface IHistorialCompraRepository
    {
        Task<HistorialCompra> Insertar(HistorialCompra historial);

        Task<HistorialCompra> SeleccionarUno(int id);

        Task<List<HistorialCompra>> SeleccionarTodos();

        Task<List<HistorialCompra>> SeleccionarPorCliente(int clienteId);

        Task Actualizar(HistorialCompra historial);

        Task Eliminar(int id);
    }
}