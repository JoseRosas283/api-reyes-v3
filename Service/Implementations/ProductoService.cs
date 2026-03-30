using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

namespace ReyesAR.Service.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;
        private readonly IImagenService _imagenService;

        public ProductoService(IProductoRepository repository, IImagenService imagenService)
        {
            _repository = repository;
            _imagenService = imagenService;
        }

        public async Task<IEnumerable<ProductoEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ProductoEntity?> GetByClaveAsync(string clave)
        {
            return await _repository.GetByIdAsync(clave);
        }

        public async Task<ProductoEntity> CreateAsync(ProductoDTO producto, IFormFile imagen)
        {
            // 1. Validaciones básicas de negocio (Capa de Aplicación)
            if (string.IsNullOrWhiteSpace(producto.nombre))
                throw new ArgumentException("El nombre del producto es obligatorio.");

            if (producto.precio <= 0)
                throw new ArgumentException("El costo del producto debe ser mayor a cero.");

            // 2. Gestión de Imagen
            if (imagen == null || imagen.Length == 0)
                throw new ArgumentException("La imagen del producto es obligatoria.");

            var urlImagen = await _imagenService.GuardarImagenAsync(imagen);
            producto.ImagenUrl = urlImagen;

            // 3. Validaciones de coherencia de Proveedor (Reforzando el SP)
            ValidarProveedor(producto);

            // 4. Delegar al repositorio
            // Nota: No calculamos el precio de venta aquí, dejamos que el SP lo haga con su DEFAULT NULL
            return await _repository.CreateAsync(producto);
        }

        public async Task<ProductoEntity> UpdateAsync(string clave, ProductoDTO producto, IFormFile? nuevaImagen)
        {
            if (string.IsNullOrWhiteSpace(clave))
                throw new ArgumentException("La clave es obligatoria para actualizar.");

            var productoExistente = await _repository.GetByIdAsync(clave);
            if (productoExistente == null)
                throw new Exception($"El producto con clave {clave} no existe.");

            // Lógica de imagen: si no hay nueva, mantenemos la anterior
            if (nuevaImagen != null && nuevaImagen.Length > 0)
            {
                producto.ImagenUrl = await _imagenService.GuardarImagenAsync(nuevaImagen);
            }
            else
            {
                producto.ImagenUrl = productoExistente.Imagen;
            }

            // Validaciones de negocio para actualización
            if (string.IsNullOrWhiteSpace(producto.nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");

            ValidarProveedor(producto);

            await _repository.UpdateAsync(clave, producto);

            return await _repository.GetByIdAsync(clave)
                   ?? throw new Exception("Error al recuperar el producto actualizado.");
        }

        public async Task DeleteAsync(string clave)
        {
            if (string.IsNullOrEmpty(clave))
                throw new ArgumentException("La clave del producto es obligatoria.");

            await _repository.DeleteAsync(clave);
        }

        // Método privado para evitar repetir lógica de validación de proveedores
        private void ValidarProveedor(ProductoDTO p)
        {
            if (p.tipo_producto == "Individual" && !string.IsNullOrWhiteSpace(p.claveProveedor))
            {
                throw new ArgumentException("Un producto 'Individual' no puede tener un proveedor asignado.");
            }

            if (p.tipo_producto != "Individual" && string.IsNullOrWhiteSpace(p.claveProveedor))
            {
                throw new ArgumentException($"El tipo '{p.tipo_producto}' requiere un proveedor obligatorio.");
            }
        }
    }
}
