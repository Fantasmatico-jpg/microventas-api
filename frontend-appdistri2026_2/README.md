# Frontend App Distribuida 2026.2

Frontend estático y básico para visualizar la arquitectura distribuida.

## Tecnologías
- HTML5
- CSS3
- JavaScript
- Fetch API

## API Gateway
El frontend consume los microservicios mediante Kong:

- `http://localhost:8000/clientes/obtener-todos`
- `http://localhost:8000/productos`
- `http://localhost:8000/ventas`

## Ejecución
Abrir `index.html` con Live Server en VS Code.

> Nota: si el navegador bloquea las peticiones por CORS, hay que habilitar CORS en Kong para el origen de Live Server.
