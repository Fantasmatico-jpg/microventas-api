using app.microCliente.dataAccess.context;
using app.microCliente.entities.models;

namespace app.microCliente.dataAccess.repositories
{
    public class HistorialCompraRepository
        : CrudGenericService<HistorialCompra>, IHistorialCompraRepository
    {
        public HistorialCompraRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<HistorialCompra> Insertar(HistorialCompra historial)
        {
            return await InsertEntity(historial);
        }

        public async Task<HistorialCompra> SeleccionarUno(int id)
        {
            return await SelectEntity(id);
        }

        public async Task<List<HistorialCompra>> SeleccionarTodos()
        {
            return await SelectEntitiesAll();
        }

        public async Task<List<HistorialCompra>> SeleccionarPorCliente(int clienteId)
        {
            var historial = await SeleccionarTodos();

            return historial
                .Where(h => h.ClienteId == clienteId)
                .ToList();
        }

        public async Task Actualizar(HistorialCompra historial)
        {
            await UpdateEntity(historial);
        }

        public async Task Eliminar(int id)
        {
            await DeleteEntity(id);
        }
    }
}