using Microsoft.EntityFrameworkCore;
using ReyesAR.Models;
using System;

namespace ReyesAR.BasedeDatos
{
    public class ReyesARContext : DbContext
    {
        public ReyesARContext(DbContextOptions<ReyesARContext> options) : base(options) { }
        public virtual DbSet<UnidadMedidaEntity> UnidadMedidas { get; set; }
        public virtual DbSet<UnidadCompraEntity> UnidadCompras { get; set; }
        public virtual DbSet<UnidadVentaEntity> UnidadVentas { get; set; }
        public virtual DbSet<DepartamentoEntity> Departamentos { get; set; }
        public virtual DbSet<CategoriaEntity> Categorias { get; set; }
        public virtual DbSet<ProveedorEntity> Proveedores { get; set; }
        public virtual DbSet<ProductoEntity> Productos { get; set; }
        public virtual DbSet<EmpleadoEntity> Empleados { get; set; }
        public virtual DbSet<RolEntity> Roles { get; set; }
        public virtual DbSet<EmpleadoRolEntity> EmpleadoRol { get; set; }
        public virtual DbSet<UsuarioEntity> Usuarios { get; set; }
        public virtual DbSet<EquivalenciaUnidadEntity> EquivalenciasUnidad { get; set; }
        public virtual DbSet<EquivalenciaUnidadVentaEntity> EquivalenciasUnidadVentas { get; set; }
        public virtual DbSet<PedidoEntity> Pedidos { get; set; }
        public virtual DbSet<DetallePedidoEntity> DetallePedido { get; set; }
        public virtual DbSet<RepresentanteEntity> Representantes { get; set; }
        public virtual DbSet<EntregaEntity> Entregas { get; set; }
        public virtual DbSet<CompraEntity> Compras { get; set; }
        public virtual DbSet<DetalleEntregaEntity> DetalleEntrega { get; set; }
        public virtual DbSet<DetalleCompraDirectaEntity> DetalleCompraDirecta {  get; set; }
        public virtual DbSet<EntradaEntity> Entradas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UnidadMedidaEntity>(entity =>
            {
                entity.ToTable("UnidadMedidas");
                entity.HasKey(u => u.claveUnidad).HasName("PK_ClaveUnidad");

                entity.Property(u => u.claveUnidad)
                    .HasColumnType("varchar(18)")
                    .HasDefaultValueSql("generar_clave_unidad()")
                    .ValueGeneratedOnAdd();

                // Mapeo de Enums de Postgres
                // Usamos HasPostgresEnum o simplemente definimos el nombre del tipo en la base de datos
                entity.Property(u => u.nombreUnidad)
                    .HasColumnType("nombre_unidad_enum")
                    .IsRequired();

                entity.Property(u => u.abreviatura)
                    .HasColumnType("abreviatura_unidad_enum")
                    .IsRequired();

                entity.Property(u => u.tipoStock)
                    .HasColumnType("tipo_stock_enum")
                    .IsRequired();

                entity.Property(u => u.estado)
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

                // Relación 1:N con Productos
                entity.HasMany(u => u.Productos)
                    .WithOne(p => p.UnidadMedida)
                    .HasForeignKey(p => p.claveUnidadMedida);
            });

            modelBuilder.Entity<UnidadVentaEntity>(entity =>
            {
                entity.ToTable("UnidadVentas"); // Coincide con tu CREATE TABLE
                entity.HasKey(u => u.claveUnidadVenta).HasName("PK_ClaveUnidadVenta");

                entity.Property(u => u.claveUnidadVenta)
                    .HasColumnType("varchar(18)")
                    .HasDefaultValueSql("generar_clave_unidadVenta()")
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.nombreUnidad)
                    .HasColumnType("nombre_unidad_venta_enum") // Enum específico de ventas
                    .IsRequired();

                entity.Property(u => u.abreviatura)
                    .HasColumnType("abreviatura_unidad_venta_enum") // Enum específico de ventas
                    .IsRequired();

                entity.Property(u => u.tipoValor)
                    .HasColumnType("tipo_valor_enum") // Enum general de valores
                    .IsRequired();

                entity.Property(u => u.estado)
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

                // Relación 1:N con Productos
                entity.HasMany(u => u.Productos)
                    .WithOne(p => p.UnidadVenta) // Asumiendo que en ProductoEntity se llama UnidadVenta
                    .HasForeignKey(p => p.claveUnidadVenta);
            });


            modelBuilder.Entity<UnidadCompraEntity>(entity =>
            {
                // 1. Nombre exacto de la tabla en tu script
                entity.ToTable("UnidadCompras");
                entity.HasKey(u => u.claveUnidadCompra).HasName("PK_ClaveunidadCompra");

                // 2. Configuración de la Clave con función de Postgres
                entity.Property(u => u.claveUnidadCompra)
                    .HasColumnType("varchar(18)")
                    .HasDefaultValueSql("generar_clave_unidadCompra()")
                    .ValueGeneratedOnAdd(); // Importante para que EF no intente mandarlo nulo

                // 3. Mapeo de ENUMS (Cambiamos varchar por el nombre del tipo en Postgres)
                entity.Property(u => u.nombre_unidad)
                    .HasColumnType("tipo_unidad_nuevo")
                    .IsRequired();

                entity.Property(u => u.abreviatura)
                    .HasColumnType("abreviatura_unidad_nuevo")
                    .IsRequired();

                entity.Property(u => u.tipo_stock)
                    .HasColumnType("tipo_valor_enum")
                    .IsRequired();

                entity.Property(u => u.estado)
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

            });

            modelBuilder.Entity<DepartamentoEntity>(entity =>
            {
                entity.ToTable("Departamentos");
                entity.HasKey(d => d.claveDepartamento).HasName("PK_ClaveDepartamento");

                entity.Property(d => d.claveDepartamento)
                    .HasColumnType("varchar(18)")
                    .HasDefaultValueSql("generar_clave_departamento()")
                    .ValueGeneratedOnAdd();

                entity.Property(d => d.nombre)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(d => d.descripcion)
                    .HasColumnType("varchar(255)")
                    .IsRequired();

                entity.Property(d => d.estado)
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

                entity.Property(d => d.fecha_creacion)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<CategoriaEntity>(entity =>
            {
                entity.ToTable("Categorias"); // Respetando las mayúsculas de tu script
                entity.HasKey(c => c.claveCategoria).HasName("PK_ClaveCategoria");

                entity.Property(c => c.claveCategoria)
                    .HasColumnType("varchar(18)")
                    .HasDefaultValueSql("generar_clave_Categoria()") // Usamos tu función de Postgres
                    .ValueGeneratedOnAdd(); // Le avisamos a EF que la DB genera este valor

                entity.Property(c => c.nombre)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(c => c.descripcion)
                    .HasColumnType("varchar(255)")
                    .IsRequired(); // Según tu script, es NOT NULL

                entity.Property(c => c.estado)
                 .HasColumnName("estado")
                .HasColumnType("boolean")
                   .IsRequired(); // ← nuevo campo para estado

                // Relación 1:N con la colección que definiste en el modelo
                entity.HasMany(c => c.Productos)
                    .WithOne(p => p.Categoria)
                    .HasForeignKey(p => p.claveCategoria);

                // Relación N:1 con Departamento
                entity.HasOne(c => c.Departamento)
                    .WithMany(d => d.Categorias) // colección en DepartamentoEntity
                    .HasForeignKey(c => c.claveDepartamento)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProveedorEntity>(entity =>
            {
                entity.ToTable("Proveedores");
                entity.HasKey(p => p.claveProveedor).HasName("PK_ClaveProveedor");

                entity.Property(p => p.claveProveedor)
                    .HasColumnName("claveProveedor")   // ← AGREGAR ESTO
                    .HasColumnType("varchar(18)");
                entity.Property(p => p.direccion)
                    .HasColumnType("varchar(255)");
                entity.Property(p => p.telefono)
                    .HasColumnType("varchar(15)");
                entity.Property(p => p.tipo_proveedor)
                    .HasColumnType("varchar(20)");

                // Campo de fecha con default en la base
                entity.Property(p => p.FechaCreacion)
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("NOW()");
            });

            // Configuración de Empresa (Extensión de Proveedor)
            modelBuilder.Entity<EmpresaEntity>(entity =>
            {
                entity.ToTable("Empresas");
                // La clave primaria es también la foránea hacia Proveedor
                entity.HasKey(e => e.claveProveedor);
                entity.Property(e => e.claveProveedor)
                    .HasColumnName("claveProveedor");
                entity.Property(e => e.razonSocial).HasColumnType("varchar(150)").IsRequired();
                entity.Property(e => e.rfc).HasColumnType("varchar(13)").IsRequired();

                 entity.HasOne(e => e.Proveedor)              // lado dependiente: Empresa apunta a Proveedor
                 .WithOne(p => p.Empresa)               // lado principal: Proveedor tiene una Empresa
                .HasForeignKey<EmpresaEntity>(e => e.claveProveedor)
                     .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Independiente (Extensión de Proveedor)
            modelBuilder.Entity<IndependienteEntity>(entity =>
            {
                entity.ToTable("Independientes");
                entity.HasKey(i => i.claveProveedor);
                entity.Property(e => e.claveProveedor)
                    .HasColumnName("claveProveedor");
                entity.Property(i => i.nombre).HasColumnType("varchar(100)").IsRequired();
                entity.Property(i => i.apellidoPaterno).HasColumnType("varchar(100)");
                entity.Property(i => i.apellidoMaterno).HasColumnType("varchar(100)");
                entity.Property(i => i.tipoServicio).HasColumnType("varchar(50)");

                   entity.HasOne(i => i.Proveedor)              // lado dependiente: Independiente apunta a Proveedor
                  .WithOne(p => p.Independiente)         // lado principal: Proveedor tiene un Independiente
               .HasForeignKey<IndependienteEntity>(i => i.claveProveedor)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductoEntity>(entity =>
            {
                entity.ToTable("Productos");
                entity.HasKey(p => p.claveProducto).HasName("PK_ClaveProducto");

                entity.Property(p => p.claveProducto)
                    .HasColumnName("claveproducto")
                    .HasColumnType("varchar(18)")
                    .HasDefaultValueSql("generar_clave_producto()");

                entity.Property(p => p.nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(p => p.descripcion)
                    .HasColumnName("descripcion")
                    .HasColumnType("varchar(255)")
                    .IsRequired();

                entity.Property(p => p.precio)
                    .HasColumnName("precio")
                    .HasColumnType("numeric(10,2)")
                    .IsRequired();

                entity.Property(p => p.margen_ganancia)
                    .HasColumnName("margen_ganancia")
                    .HasColumnType("numeric(5,2)")
                    .HasDefaultValue(1.30m);

                entity.Property(p => p.precio_venta)
                    .HasColumnName("precio_venta")
                    .HasColumnType("numeric(10,2)");

                entity.Property(p => p.codigo_barras)
                    .HasColumnName("codigo_barras")
                    .HasColumnType("varchar(13)");

                entity.Property(p => p.tipo_producto)
                    .HasColumnName("tipo_producto")
                    .HasColumnType("varchar(15)")
                    .IsRequired();

                entity.Property(p => p.cantidad)
                    .HasColumnName("cantidad")
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.caducidad)
                    .HasColumnName("caducidad")
                    .HasColumnType("boolean");

                entity.Property(p => p.estado)
                    .HasColumnName("estado")
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

                entity.Property(p => p.Imagen)
                    .HasColumnName("imagen")
                    .HasColumnType("varchar");

                // FK: Categoría
                entity.Property(p => p.claveCategoria)
                    .HasColumnName("clavecategoria");

                entity.HasOne(p => p.Categoria)
                    .WithMany(c => c.Productos)
                    .HasForeignKey(p => p.claveCategoria);

                // FK: Unidad de Medida
                entity.Property(p => p.claveUnidadMedida)
                    .HasColumnName("claveunidadmedida");

                entity.HasOne(p => p.UnidadMedida)
                    .WithMany(u => u.Productos)
                    .HasForeignKey(p => p.claveUnidadMedida);

                // FK: Unidad de Venta
                entity.Property(p => p.claveUnidadVenta)
                    .HasColumnName("claveunidadventa");

                entity.HasOne(p => p.UnidadVenta)
                    .WithMany(u => u.Productos)
                    .HasForeignKey(p => p.claveUnidadVenta);

                // FK: Proveedor
                entity.Property(p => p.claveProveedor)
                    .HasColumnName("claveproveedor");

                entity.HasOne(p => p.Proveedor)
                    .WithMany(pr => pr.Productos)
                    .HasForeignKey(p => p.claveProveedor);
            });

            modelBuilder.Entity<EmpleadoEntity>(entity =>
            {
                entity.ToTable("Empleados"); // Nombre exacto de la tabla física

                // Clave primaria
                entity.HasKey(e => e.claveEmpleado);

                // Mapeo de columnas
                entity.Property(e => e.claveEmpleado)
                    .HasColumnName("claveEmpleado")
                    .HasColumnType("varchar(18)")
                 .HasDefaultValueSql("generar_clave_empleado()");

                entity.Property(e => e.nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(e => e.apellido_paterno)
                    .HasColumnName("apellido_paterno")
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(e => e.apellido_materno)
                    .HasColumnName("apellido_materno")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(15)")
                    .IsRequired();

                entity.Property(e => e.curp)
                    .HasColumnName("curp")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.HasIndex(e => e.curp)
                    .IsUnique(); // CURP debe ser único

                entity.Property(e => e.estado)
                    .HasColumnName("estado")
                    .HasColumnType("boolean") // Booleano en SQL Server
                    .IsRequired();
            });

            modelBuilder.Entity<RolEntity>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r => r.ClaveRol);
                entity.Property(r => r.ClaveRol)
                    .HasColumnName("claveRol")
                    .HasColumnType("varchar(18)");

                // ← SIN .HasDefaultValueSql("generar_clave_rol()")
                entity.Property(r => r.nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(r => r.descripcion)
                    .HasColumnName("descripcion")
                    .HasColumnType("varchar(150)");

                entity.Property(r => r.activo)
                    .HasColumnName("activo")
                    .HasColumnType("boolean")
                    .HasDefaultValue(true)
                    .IsRequired();

                entity.Property(r => r.fecha_creacion)
                    .HasColumnName("fecha_creacion")
                    .HasColumnType("date")
                    .HasDefaultValueSql("CURRENT_DATE")
                    .IsRequired();
            });

            modelBuilder.Entity<EmpleadoRolEntity>(entity =>
            {
                entity.ToTable("EmpleadoRol"); // Nombre exacto de la tabla física

                // Clave primaria compuesta por 3 campos (indispensable para el historial)
                entity.HasKey(er => new { er.claveEmpleado, er.claveRol, er.fecha_inicio });

                // Mapeo de columnas
                entity.Property(er => er.claveEmpleado)
                    .HasColumnName("claveEmpleado")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(er => er.claveRol)
                    .HasColumnName("claveRol")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(er => er.fecha_inicio)
                    .HasColumnName("fecha_inicio")
                    .HasColumnType("date") // DATE en PostgreSQL
                    .HasDefaultValueSql("CURRENT_DATE")
                    .IsRequired();

                entity.Property(er => er.fecha_fin)
                    .HasColumnName("fecha_fin")
                    .HasColumnType("date"); // NULL por defecto según tu SQL

                // Relaciones
                entity.HasOne(er => er.Empleado)
                    .WithMany(e => e.RolesHistorial) // Un empleado tiene muchos registros en su historial
                    .HasForeignKey(er => er.claveEmpleado)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(er => er.Rol)
                    .WithMany(r => r.EmpleadosAsignados) // Un rol puede estar en muchos registros de historial
                    .HasForeignKey(er => er.claveRol)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UsuarioEntity>(entity =>
            {
                entity.ToTable("Usuarios"); // Nombre exacto en Postgres

                entity.HasKey(u => u.claveUsuario);

                // Mapeo de columnas y valor por defecto del SP/Función de Postgres
                entity.Property(u => u.claveUsuario)
                    .HasColumnName("claveUsuario")
                    .HasColumnType("varchar(18)")
                    .HasDefaultValueSql("generar_clave_usuario()");

                entity.Property(u => u.nombre_usuario)
                    .HasColumnName("nombre_usuario")
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                // Definición de restricción UNIQUE para el login
                entity.HasIndex(u => u.nombre_usuario)
                    .IsUnique()
                    .HasDatabaseName("uq_nombre_usuario");

                entity.Property(u => u.contrasena)
                    .HasColumnName("contrasena")
                    .HasColumnType("varchar(250)")
                    .IsRequired();

                entity.Property(u => u.estado)
                    .HasColumnName("estado")
                    .HasDefaultValue(true);

                entity.Property(u => u.claveEmpleado)
                    .HasColumnName("claveEmpleado")
                    .HasColumnType("varchar(18)");

                // Configuración de la relación 1:1
                // Un Usuario tiene un Empleado, y un Empleado tiene un Usuario
                entity.HasOne(u => u.Empleado)
                    .WithOne(e => e.Usuario)
                    .HasForeignKey<UsuarioEntity>(u => u.claveEmpleado)
                    .OnDelete(DeleteBehavior.SetNull); // O Cascade, según prefieras
            });

            modelBuilder.Entity<EquivalenciaUnidadEntity>(entity =>
            {
                entity.ToTable("EquivalenciasUnidad");

                // 1. Clave Primaria Compuesta (PK de dos columnas)
                entity.HasKey(e => new { e.claveProducto, e.claveUnidadCompra });

                // 2. Mapeo de Atributos con tipos de datos SQL exactos
                entity.Property(e => e.claveProducto)
                    .HasColumnName("claveProducto")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(e => e.claveUnidadCompra)
                    .HasColumnName("claveUnidadCompra")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(e => e.factor_conversion)
                    .HasColumnName("factor_conversion")
                    .HasColumnType("numeric") // Respetamos NUMERIC
                    .IsRequired();

                entity.Property(e => e.unidad_base)
                    .HasColumnName("unidad_base")
                    .HasColumnType("nombre_unidad_enum") // Tu ENUM de Postgres
                    .IsRequired();

                // 3. Configuración de Relaciones N:N (Foreign Keys)
                entity.HasOne(e => e.Producto)
                    .WithMany(p => p.Equivalencias) // Requiere la colección en ProductoEntity
                    .HasForeignKey(e => e.claveProducto)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.UnidadCompra)
                    .WithMany(u => u.EquivalenciasUnidad) // Requiere la colección en UnidadCompraEntity
                    .HasForeignKey(e => e.claveUnidadCompra)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EquivalenciaUnidadVentaEntity>(entity =>
            {
                entity.ToTable("EquivalenciasUnidadVentas");

                // 1. Clave Primaria Compuesta (PK de dos columnas)
                entity.HasKey(e => new { e.claveProducto, e.claveUnidadVenta });

                // 2. Mapeo de Atributos con tipos de datos SQL exactos
                entity.Property(e => e.claveProducto)
                    .HasColumnName("claveProducto")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(e => e.claveUnidadVenta)
                    .HasColumnName("claveUnidadVenta")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(e => e.factor_conversion)
                    .HasColumnName("factor_conversion")
                    .HasColumnType("numeric") // Respetamos NUMERIC
                    .IsRequired();

                entity.Property(e => e.unidad_base)
                    .HasColumnName("unidad_base")
                    .HasColumnType("nombre_unidad_enum") // Tu ENUM de Postgres
                    .IsRequired();

                // 3. Configuración de Relaciones N:N (Foreign Keys)
                entity.HasOne(e => e.Producto)
                    .WithMany(p => p.EquivalenciaProductoVenta) // Requiere la colección en ProductoEntity
                    .HasForeignKey(e => e.claveProducto)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.UnidadVenta)
                    .WithMany(u => u.EquivalenciasUnidadVenta) // Requiere la colección en UnidadCompraEntity
                    .HasForeignKey(e => e.claveUnidadVenta)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PedidoEntity>(entity =>
            {
                entity.ToTable("Pedidos"); // Nombre exacto en Postgres

                // Clave primaria
                entity.HasKey(p => p.clavePedido);

                // Mapeo de columnas (Case-sensitive para Postgres)
                entity.Property(p => p.clavePedido)
                    .HasColumnName("clavepedido")
                    .HasColumnType("varchar(18)")
                    .HasDefaultValueSql("generar_clave_pedido()");

                entity.Property(p => p.fecha_pedido)
                    .HasColumnName("fecha_pedido")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(p => p.estado)
                    .HasColumnName("estado")
                    .HasColumnType("varchar(20)")
                    .IsRequired();

                entity.Property(p => p.observaciones)
                    .HasColumnName("observaciones")
                    .HasColumnType("varchar(150)")
                    .IsRequired();

                entity.Property(p => p.claveUsuario)
                    .HasColumnName("claveusuario")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(p => p.tipo_pedido)
                    .HasColumnName("tipo_pedido")
                    .HasColumnType("varchar(20)")
                    .IsRequired();

                entity.Property(p => p.claveProveedor)
                    .HasColumnName("claveproveedor")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(p => p.total)
                    .HasColumnName("total")
                    .HasColumnType("numeric")
                    .HasDefaultValue(0)
                    .IsRequired();

                // --- RELACIONES ACTIVADAS ---

                // Relación con Usuarios(1:N)
                entity.HasOne(p => p.Usuario)
                    .WithMany(u => u.Pedidos) // <--- Aquí conectamos con la colección de UsuarioEntity
                    .HasForeignKey(p => p.claveUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación con Proveedores (1:N)
                entity.HasOne(p => p.Proveedor)
                    .WithMany(pr => pr.Pedidos) // <--- Aquí conectamos con la colección de ProveedorEntity
                    .HasForeignKey(p => p.claveProveedor)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DetallePedidoEntity>(entity =>
            {
                entity.ToTable("DetallePedido"); // Nombre exacto de la tabla física

                // Clave primaria compuesta
                entity.HasKey(d => new { d.Clavepedido, d.Claveproducto });

                // Mapeo de columnas
                entity.Property(d => d.Clavepedido)
                    .HasColumnName("Clavepedido")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(d => d.Claveproducto)
                    .HasColumnName("Claveproducto")
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(d => d.cantidad)
                    .HasColumnName("cantidad")
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.Property(d => d.descripcion)
                    .HasColumnName("descripcion")
                    .HasColumnType("varchar(255)");

                entity.Property(d => d.tipo_detalle)
                    .HasColumnName("tipo_detalle")
                    .HasColumnType("varchar(20)")
                    .IsRequired();

                // Relaciones
                entity.HasOne(d => d.Pedido)
                    .WithMany(p => p.Detalles) // Un pedido puede tener muchos detalles
                    .HasForeignKey(d => d.Clavepedido)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Producto)
                    .WithMany(pr => pr.Detalles) // Un producto puede aparecer en muchos detalles
                    .HasForeignKey(d => d.Claveproducto)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RepresentanteEntity>(entity =>
            {
                entity.ToTable("Representantes");

                // Llave Primaria
                entity.HasKey(e => e.claveRepresentante);

                // 2. Mapeo detallado de campos con tipos de datos de Postgres
                entity.Property(e => e.claveRepresentante)
                      .HasColumnName("claveRepresentante")
                      .HasColumnType("varchar(18)") // Coincide con tu DEFAULT generar_clave_representante()
                      .HasDefaultValueSql("generar_clave_representante()");

                entity.Property(e => e.nombre)
                      .IsRequired()
                      .HasColumnName("nombre")
                      .HasColumnType("varchar(50)");

                entity.Property(e => e.apellido_paterno)
                      .IsRequired()
                      .HasColumnName("apellido_paterno")
                      .HasColumnType("varchar(50)");

                entity.Property(e => e.apellido_materno)
                      .HasColumnName("apellido_materno")
                      .HasColumnType("varchar(50)");

                entity.Property(e => e.telefono)
                      .IsRequired()
                      .HasColumnName("telefono")
                      .HasColumnType("varchar(15)");

                entity.Property(e => e.estado)
                      .HasColumnName("estado")
                      .HasColumnType("boolean")
                      .HasDefaultValue(true);

                entity.Property(e => e.claveProveedor)
                      .IsRequired()
                      .HasColumnName("claveproveedor")
                      .HasColumnType("varchar(18)");

                // 3. Mapeo del ENUM (Crucial para que no lo mande como texto simple)
                entity.Property(e => e.tipo_representante)
                      .IsRequired()
                      .HasColumnName("tipo_representante")
                      .HasColumnType("tipo_representante_enum");

                // --- RELACIÓN ---
                entity.HasOne(d => d.Proveedor)
                      .WithMany(p => p.Representantes)
                      .HasForeignKey(d => d.claveProveedor)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EntregaEntity>(entity =>
            {
                // 1. Tabla y Llave Primaria
                entity.ToTable("Entregas");
                entity.HasKey(e => e.claveEntrega);
                entity.Property(e => e.claveEntrega)
                      .HasColumnName("claveEntrega")
                      .HasColumnType("varchar(18)")
                      .HasDefaultValueSql("generar_clave_entrega()");

                // 2. Configuración de Columnas
                entity.Property(e => e.fecha_entrega)
                      .HasColumnName("fecha_entrega")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.estado)
                      .IsRequired()
                      .HasColumnName("estado")
                      .HasColumnType("varchar(20)");
                entity.Property(e => e.total)
                      .IsRequired()
                      .HasColumnName("total")
                      .HasColumnType("numeric(10,2)");
                entity.Property(e => e.descripcion)
                      .IsRequired()
                      .HasColumnName("descripcion")
                      .HasColumnType("varchar(255)");
                entity.Property(e => e.tipo_entrega)
                      .IsRequired()
                      .HasColumnName("tipo_entrega")
                      .HasColumnType("varchar(50)");

                // ✅ Declarar claveRepresentante ANTES de usarla en la relación
                entity.Property(e => e.claveRepresentante)
                      .HasColumnName("claveRepresentante")
                      .HasColumnType("varchar(18)")
                      .IsRequired(false);

                // 3. MAPEO DE RELACIONES
                // --- RELACIÓN 1:1 (Pedidos - Entregas) ---
                // Un pedido solo tiene una entrega. La clavePedido debe ser única en la tabla Entregas.
                entity.HasIndex(e => e.clavePedido).IsUnique();
                entity.HasOne(e => e.Pedido)
                      .WithOne(p => p.Entrega) // En PedidoEntity: public virtual EntregaEntity Entrega { get; set; }
                      .HasForeignKey<EntregaEntity>(e => e.clavePedido)
                      .OnDelete(DeleteBehavior.Restrict);

                // --- RELACIÓN 1:N (Proveedores - Entregas) ---
                // Un proveedor puede tener muchas entregas asociadas.
                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Entregas) // En ProveedorEntity: public virtual ICollection<EntregaEntity> Entregas { get; set; }
                      .HasForeignKey(e => e.claveProveedor)
                      .OnDelete(DeleteBehavior.Restrict);

                // --- RELACIÓN 1:N (Usuarios - Entregas) ---
                // Un usuario (empleado/almacenista) puede registrar muchas entregas.
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Entregas) // En UsuarioEntity: public virtual ICollection<EntregaEntity> Entregas { get; set; }
                      .HasForeignKey(e => e.claveUsuario)
                      .OnDelete(DeleteBehavior.Restrict);

                // --- RELACIÓN 1:N (Representantes - Entregas) ---
                // Opcional: Un representante puede estar en varias entregas.
                entity.HasOne(e => e.Representante)
                      .WithMany(r => r.Entregas)
                      .HasForeignKey(e => e.claveRepresentante)
                      .HasConstraintName("FK_Entregas_Representantes")
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // 1. Entidad Padre: Compras
            modelBuilder.Entity<CompraEntity>(entity =>
            {
                entity.ToTable("Compras");
                entity.HasKey(c => c.ClaveCompra).HasName("PK_ClaveCompra");
                entity.Property(c => c.ClaveCompra).HasColumnType("varchar(18)").HasColumnName("ClaveCompra");
                entity.Property(c => c.fecha_compra).HasColumnType("timestamp").HasColumnName("fecha_compra").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(c => c.tipo_compra).HasColumnType("varchar(20)").HasColumnName("tipo_compra").IsRequired();
                entity.Property(c => c.total).HasColumnType("numeric(10,2)").HasColumnName("total").IsRequired();
                entity.Property(c => c.forma_pago).HasColumnType("varchar(10)").HasColumnName("forma_pago");
                entity.Property(c => c.ClaveUsuario)
                        .HasColumnType("varchar(18)")
                        .HasColumnName("ClaveUsuario");
                entity.HasOne(c => c.Usuario)
                        .WithMany(u => u.Compras)
                        .HasForeignKey(c => c.ClaveUsuario)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            // 2. Subentidad: CompraPedido
            modelBuilder.Entity<CompraPedidoEntity>(entity =>
            {
                entity.ToTable("Comprapedido");
                entity.HasKey(cp => cp.claveCompra).HasName("PK_CompraPedido");
                entity.Property(cp => cp.claveCompra).HasColumnType("varchar(18)").HasColumnName("claveCompra");
                entity.Property(cp => cp.folio).HasColumnType("varchar(18)").HasColumnName("folio");
                entity.Property(cp => cp.factura_proveedor).HasColumnType("varchar").HasColumnName("factura_proveedor");
                entity.Property(cp => cp.claveEntrega).HasColumnType("varchar(18)").HasColumnName("claveEntrega").IsRequired();
                entity.Property(cp => cp.estado_cumplimiento).HasColumnType("varchar(20)").HasColumnName("estado_cumplimiento").IsRequired();
                entity.Property(cp => cp.estado_operativo).HasColumnType("varchar(20)").HasColumnName("estado_operativo").IsRequired();
                entity.HasOne(cp => cp.Compra)
                        .WithOne(c => c.CompraPedido)
                        .HasForeignKey<CompraPedidoEntity>(cp => cp.claveCompra)
                        .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(cp => cp.Entrega)
                        .WithMany()
                        .HasForeignKey(cp => cp.claveEntrega)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            // 3. Subentidad: CompraDirecta
            modelBuilder.Entity<CompraDirectaEntity>(entity =>
            {
                entity.ToTable("Compradirecta");
                entity.HasKey(cd => cd.claveCompra).HasName("PK_CompraDirecta");
                entity.Property(cd => cd.claveCompra).HasColumnType("varchar(18)").HasColumnName("claveCompra");
                entity.Property(cd => cd.referencia_pago).HasColumnType("varchar(20)").HasColumnName("referencia_pago");
                entity.Property(cd => cd.documento_referencia).HasColumnType("varchar").HasColumnName("documento_referencia");
                entity.Property(cd => cd.origen).HasColumnType("varchar(20)").HasColumnName("origen").IsRequired();
                entity.Property(cd => cd.estado).HasColumnType("varchar(20)").HasColumnName("estado").IsRequired();
                entity.HasOne(cd => cd.Compra)
                        .WithOne(c => c.CompraDirecta)
                        .HasForeignKey<CompraDirectaEntity>(cd => cd.claveCompra)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DetalleEntregaEntity>(entity =>
            {
                // Nombre de la tabla
                entity.ToTable("DetalleEntrega");

                // Llave Primaria Compuesta con Nombre Personalizado
                entity.HasKey(e => new { e.claveEntrega, e.claveProducto, e.tipo_detalle, e.extra })
                      .HasName("PK_DetalleEntrega");

                // Configuración de Propiedades (Columnas)
                entity.Property(e => e.claveEntrega)
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(e => e.claveProducto)
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(e => e.claveCompra)
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(e => e.claveUnidadCompra)
                    .HasColumnType("varchar(18)")
                    .IsRequired();

                entity.Property(e => e.cantidad)
                    .HasColumnType("numeric")
                    .IsRequired();

                entity.Property(e => e.precioUnitario)
                    .HasColumnType("numeric(10,2)")
                    .IsRequired();

                entity.Property(e => e.subtotal)
                    .HasColumnType("numeric(12,2)")
                    .IsRequired();

                entity.Property(e => e.observaciones)
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.tipo_detalle)
                    .HasColumnType("varchar(20)")
                    .IsRequired();

                entity.Property(e => e.extra)
                    .HasColumnType("boolean")
                    .HasDefaultValue(false)
                    .IsRequired();

                // ==========================================
                // RELACIONES (LLAVES FORÁNEAS)
                // ==========================================

                // Relación con Entregas
                entity.HasOne(d => d.Entrega)
                    .WithMany(e => e.DetalleEntregaProducto)
                    .HasForeignKey(d => d.claveEntrega)
                    .HasConstraintName("fk_detalle_entrega_padre");

                // Relación con CompraPedido
                entity.HasOne(d => d.CompraPedido)
                    .WithMany(c => c.DetalleEntregaProducto)
                    .HasForeignKey(d => d.claveCompra)
                    .HasConstraintName("fk_detalle_entrega_compra");

                // Candado Producto-Unidad (EquivalenciasUnidad)
                entity.HasOne(d => d.EquivalenciaUnidad)
                    .WithMany(e => e.DetalleEntregaProducto) // No requiere colección en Equivalencia
                    .HasForeignKey(d => new { d.claveProducto, d.claveUnidadCompra })
                    .HasConstraintName("fk_entrega_unidad_equivalencia");
            });

            // Relación con Producto (Unidireccional desde Producto)
            modelBuilder.Entity<ProductoEntity>()
                .HasMany(p => p.DetalleEntregaProducto)
                .WithOne(d => d.Producto)
                .HasForeignKey(d => d.claveProducto);

            // CONFIGURACIÓN: DetalleCompraDirecta
            modelBuilder.Entity<DetalleCompraDirectaEntity>(entity =>
            {
                entity.ToTable("DetalleCompraDirecta");

                // 1. Definición de la Llave Primaria Compuesta
                entity.HasKey(e => new { e.claveCompra, e.claveProducto });

                // 2. Mapeo de Columnas y Tipos de Datos
                entity.Property(e => e.claveCompra)
                    .HasMaxLength(18)
                    .IsRequired();

                entity.Property(e => e.claveProducto)
                    .HasMaxLength(18)
                    .IsRequired();

                entity.Property(e => e.cantidad)
                    .HasColumnType("numeric(10,2)")
                    .IsRequired();

                entity.Property(e => e.precioUnitario)
                    .HasColumnType("double precision")
                    .IsRequired();

                entity.Property(e => e.impuestos)
                    .HasMaxLength(10);

                // 3. Columna Calculada (IMPORTANTE: Se marca como calculada en BD)
                entity.Property(d => d.subtotal)
                    .HasColumnType("numeric(10,2)")
                    .HasComputedColumnSql("\"cantidad\" * \"precioUnitario\"", stored: true)
                    .ValueGeneratedOnAddOrUpdate(); // Se genera tanto al insertar como al editar

                entity.Property(e => e.claveUnidadCompra)
                    .HasMaxLength(18)
                    .IsRequired();

                // 4. Relaciones (Foreign Keys)

                // Relación con CompraDirecta
                entity.HasOne(d => d.CompraDirecta)
                    .WithMany() // O .WithMany(p => p.Detalles) si tienes la colección en la cabecera
                    .HasForeignKey(d => d.claveCompra)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con EquivalenciasUnidad (Usa los dos campos de la FK compuesta)
                entity.HasOne(d => d.EquivalenciaUnidad)
                    .WithMany()
                    .HasForeignKey(d => new { d.claveProducto, d.claveUnidadCompra })
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación lógica con Producto
                entity.HasOne(d => d.Producto)
                    .WithMany()
                    .HasForeignKey(d => d.claveProducto)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EntradaEntity>(entity =>
            {
                // 1. Nombre de la tabla y Llave Primaria
                entity.ToTable("Entradas");
                entity.HasKey(e => e.claveEntrada);

                // 2. Configuración de la Clave con Función de Postgres
                entity.Property(e => e.claveEntrada)
                    .HasColumnType("varchar(18)")
                    .HasDefaultValueSql("generar_clave_entrada()")
                    .ValueGeneratedOnAdd();

                // 3. Mapeo de campos obligatorios y tipos
                entity.Property(e => e.cantidad_recibida)
                    .HasColumnType("numeric(10,2)")
                    .IsRequired();

                entity.Property(e => e.fecha)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.tipo_entrada)
                    .HasMaxLength(18)
                    .IsRequired();

                entity.Property(e => e.fecha_caducidad)
                    .HasColumnType("date");

                entity.Property(e => e.observaciones)
                    .HasMaxLength(150);

                // Campo extra: BOOLEAN DEFAULT NULL
                entity.Property(e => e.extra)
                    .HasDefaultValue(null);

                // --- 4. CONFIGURACIÓN DE RELACIONES (COLECCIONES) ---

                // Relación con Compra (Una Compra -> Muchas Entradas)
                entity.HasOne(d => d.Compra)
                    .WithMany(p => p.Entradas) // p.Entradas es la ICollection en CompraEntity
                    .HasForeignKey(d => d.claveCompra)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                // Relación con Producto (Un Producto -> Muchas Entradas)
                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.Entradas) // p.Entradas es la ICollection en ProductoEntity
                    .HasForeignKey(d => d.claveProducto)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                // Relación con Usuario (Un Usuario -> Muchas Entradas registradas)
                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Entradas) // p.Entradas es la ICollection en UsuarioEntity
                    .HasForeignKey(d => d.claveUsuario)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });
        }
    }
}
