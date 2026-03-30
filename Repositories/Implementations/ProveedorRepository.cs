using Npgsql;
using ReyesAR.DTO;
using ReyesAR.Repositories.Interface;
using ReyesAR.BasedeDatos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace ReyesAR.Repositories.Implementations
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly ReyesARContext _context;

        public ProveedorRepository(ReyesARContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProveedorBaseDTO>> GetAllAsync()
        {
            var proveedores = await _context.Proveedores.ToListAsync();

            return proveedores.Select(p => new ProveedorBaseDTO
            {
                ClaveProveedor = p.claveProveedor,
                Direccion = p.direccion,
                Telefono = p.telefono,
                Estado = p.estado,
                TipoProveedor = p.tipo_proveedor
            }).ToList();
        }

        public async Task<ProveedorBaseDTO?> GetByIdAsync(string idProveedor)
        {
            var p = await _context.Proveedores
                .Include(x => x.Empresa)
                .Include(x => x.Independiente)
                .FirstOrDefaultAsync(x => x.claveProveedor == idProveedor);

            if (p == null) return null;

            return p.tipo_proveedor == "EMPRESA" && p.Empresa != null
                ? new ProveedorEmpresaDTO
                {
                    ClaveProveedor = p.claveProveedor,
                    Direccion = p.direccion,
                    Telefono = p.telefono,
                    Estado = p.estado,
                    TipoProveedor = p.tipo_proveedor,
                    RazonSocial = p.Empresa.razonSocial,
                    Rfc = p.Empresa.rfc
                }
                : new ProveedorIndependienteDTO
                {
                    ClaveProveedor = p.claveProveedor,
                    Direccion = p.direccion,
                    Telefono = p.telefono,
                    Estado = p.estado,
                    TipoProveedor = p.tipo_proveedor,
                    Nombre = p.Independiente?.nombre,
                    ApellidoPaterno = p.Independiente?.apellidoPaterno,
                    ApellidoMaterno = p.Independiente?.apellidoMaterno,
                    TipoServicio = p.Independiente?.tipoServicio
                };
        }

        public async Task<ProveedorBaseDTO> CreateAsync(ProveedorCreateDTO dto)
        {
            // Declaramos el parámetro de entrada/salida
            var parameterClave = new NpgsqlParameter("p_nueva_clave", NpgsqlTypes.NpgsqlDbType.Varchar, 18)
            {
                Direction = ParameterDirection.InputOutput,
                Value = DBNull.Value // se pasa como NULL para que el procedimiento genere la clave
            };

            // Creamos los parámetros de entrada con tipo explícito
            var pDireccion = new NpgsqlParameter("p0", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.direccion ?? DBNull.Value };
            var pTipo = new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.tipo_proveedor ?? DBNull.Value };
            var pTel = new NpgsqlParameter("p2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.telefono ?? DBNull.Value };
            var pRazon = new NpgsqlParameter("p3", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.razonSocial ?? DBNull.Value };
            var pRfc = new NpgsqlParameter("p4", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.rfc ?? DBNull.Value };
            var pNom = new NpgsqlParameter("p5", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.nombre ?? DBNull.Value };
            var pPat = new NpgsqlParameter("p6", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.apellidoPaterno ?? DBNull.Value };
            var pMat = new NpgsqlParameter("p7", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.apellidoMaterno ?? DBNull.Value };
            var pServ = new NpgsqlParameter("p8", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.tipoServicio ?? DBNull.Value };
            var pEst = new NpgsqlParameter("p9", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = dto.estado };

            // Llamada al procedimiento: incluye el INOUT como primer argumento
            await _context.Database.ExecuteSqlRawAsync(
                "CALL \"insertar_proveedor\"(@p_nueva_clave, @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)",
                parameterClave, pDireccion, pTipo, pTel, pRazon, pRfc, pNom, pPat, pMat, pServ, pEst
            );

            // Recuperamos la clave generada por el procedimiento
            var clave = parameterClave.Value?.ToString();

            // Armamos el DTO de retorno según el tipo de proveedor
            return dto.tipo_proveedor?.ToUpper() == "EMPRESA"
                ? new ProveedorEmpresaDTO
                {
                    ClaveProveedor = clave,
                    Direccion = dto.direccion,
                    Telefono = dto.telefono,
                    Estado = dto.estado,
                    TipoProveedor = dto.tipo_proveedor,
                    RazonSocial = dto.razonSocial,
                    Rfc = dto.rfc
                }
                : new ProveedorIndependienteDTO
                {
                    ClaveProveedor = clave,
                    Direccion = dto.direccion,
                    Telefono = dto.telefono,
                    Estado = dto.estado,
                    TipoProveedor = dto.tipo_proveedor,
                    Nombre = dto.nombre,
                    ApellidoPaterno = dto.apellidoPaterno,
                    ApellidoMaterno = dto.apellidoMaterno,
                    TipoServicio = dto.tipoServicio
                };
        }



        public async Task UpdateAsync(string claveUrl, ProveedorCreateDTO dto)
        {
            var parameters = new[]
            {
            new NpgsqlParameter("p0", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = claveUrl },
            new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.direccion ?? DBNull.Value },
            new NpgsqlParameter("p2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.telefono ?? DBNull.Value },
            new NpgsqlParameter("p3", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = dto.estado },
            new NpgsqlParameter("p4", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.tipo_proveedor ?? DBNull.Value },
            new NpgsqlParameter("p5", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.razonSocial ?? DBNull.Value },
            new NpgsqlParameter("p6", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.rfc ?? DBNull.Value },
            new NpgsqlParameter("p7", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.nombre ?? DBNull.Value },
            new NpgsqlParameter("p8", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.apellidoPaterno ?? DBNull.Value },
            new NpgsqlParameter("p9", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.apellidoMaterno ?? DBNull.Value },
            new NpgsqlParameter("p10", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)dto.tipoServicio ?? DBNull.Value }
        };

            await _context.Database.ExecuteSqlRawAsync(
                "CALL actualizar_proveedor(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)",
                parameters
            );
        }

        public async Task DeleteAsync(string idProveedor)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL eliminar_proveedor({0})",
                idProveedor
            );
        }
    }

}
