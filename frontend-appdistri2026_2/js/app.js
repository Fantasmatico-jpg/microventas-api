const API = "http://localhost:8000";

const sections = document.querySelectorAll(".section-view");
const tabs = document.querySelectorAll(".tab");
const toast = document.getElementById("toast");

function showSection(name) {
  sections.forEach(section => {
    section.classList.toggle("active", section.id === name);
  });

  tabs.forEach(tab => {
    tab.classList.toggle("active", tab.dataset.section === name);
  });
}

tabs.forEach(tab => {
  tab.addEventListener("click", () => showSection(tab.dataset.section));
});

function showToast(message) {
  toast.textContent = message;
  toast.classList.add("show");
  setTimeout(() => toast.classList.remove("show"), 2200);
}

async function apiGet(path) {
  const response = await fetch(`${API}${path}`);
  if (!response.ok) {
    throw new Error(`${response.status} ${response.statusText}`);
  }
  return response.json();
}

function normalizeClients(data) {
  return Array.isArray(data) ? data : (data.result || []);
}

async function loadClientes() {
  const body = document.getElementById("clientesBody");
  body.innerHTML = `<tr><td colspan="6" class="empty">Cargando...</td></tr>`;

  try {
    const data = await apiGet("/clientes/obtener-todos");
    const clients = normalizeClients(data);
    document.getElementById("clientCount").textContent = clients.length;

    body.innerHTML = clients.length
      ? clients.map(c => `
          <tr>
            <td>${c.id ?? ""}</td>
            <td>${c.nombre ?? ""}</td>
            <td>${c.apellido ?? ""}</td>
            <td>${c.email ?? ""}</td>
            <td>${c.telefono ?? ""}</td>
            <td>${c.estado ? "Activo" : "Inactivo"}</td>
          </tr>
        `).join("")
      : `<tr><td colspan="6" class="empty">No hay clientes.</td></tr>`;

    showToast("Clientes actualizados");
  } catch (error) {
    body.innerHTML = `<tr><td colspan="6" class="empty">Error al cargar clientes.</td></tr>`;
    showToast(`Error: ${error.message}`);
  }
}

async function loadProductos() {
  const body = document.getElementById("productosBody");
  body.innerHTML = `<tr><td colspan="6" class="empty">Cargando...</td></tr>`;

  try {
    const data = await apiGet("/productos");
    const products = Array.isArray(data) ? data : [];
    document.getElementById("productCount").textContent = products.length;

    body.innerHTML = products.length
      ? products.map(p => `
          <tr>
            <td>${p.id ?? ""}</td>
            <td>${p.nombre ?? ""}</td>
            <td>${p.categoria ?? ""}</td>
            <td>$${Number(p.precio ?? 0).toFixed(2)}</td>
            <td>${p.stock ?? 0}</td>
            <td>${p.estado ? "Activo" : "Inactivo"}</td>
          </tr>
        `).join("")
      : `<tr><td colspan="6" class="empty">No hay productos.</td></tr>`;

    showToast("Productos actualizados");
  } catch (error) {
    body.innerHTML = `<tr><td colspan="6" class="empty">Error al cargar productos.</td></tr>`;
    showToast(`Error: ${error.message}`);
  }
}

async function loadVentas() {
  const body = document.getElementById("ventasBody");
  body.innerHTML = `<tr><td colspan="7" class="empty">Cargando...</td></tr>`;

  try {
    const data = await apiGet("/ventas");
    const sales = Array.isArray(data) ? data : [];
    document.getElementById("saleCount").textContent = sales.length;

    body.innerHTML = sales.length
      ? sales.map(v => `
          <tr>
            <td>${v.id ?? ""}</td>
            <td>${v.numeroVenta ?? ""}</td>
            <td>${v.cliente ?? ""}</td>
            <td>${v.productoId ?? ""}</td>
            <td>${v.cantidad ?? 0}</td>
            <td>$${Number(v.total ?? 0).toFixed(2)}</td>
            <td>${v.estado ? "Activa" : "Inactiva"}</td>
          </tr>
        `).join("")
      : `<tr><td colspan="7" class="empty">No hay ventas.</td></tr>`;

    showToast("Ventas actualizadas");
  } catch (error) {
    body.innerHTML = `<tr><td colspan="7" class="empty">Error al cargar ventas.</td></tr>`;
    showToast(`Error: ${error.message}`);
  }
}

async function checkApi() {
  const status = document.getElementById("apiStatus");

  try {
    await apiGet("/productos");
    status.textContent = "API: conectada";
    status.classList.remove("offline");
    status.classList.add("online");
  } catch {
    status.textContent = "API: sin conexión";
    status.classList.remove("online");
    status.classList.add("offline");
  }
}

async function refreshAll() {
  await Promise.all([loadClientes(), loadProductos(), loadVentas(), checkApi()]);
  showSection("resumen");
}

document.getElementById("refreshAll").addEventListener("click", refreshAll);

window.showSection = showSection;
window.loadClientes = loadClientes;
window.loadProductos = loadProductos;
window.loadVentas = loadVentas;

refreshAll();
