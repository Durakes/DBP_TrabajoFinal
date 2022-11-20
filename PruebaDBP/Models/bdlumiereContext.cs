using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PruebaDBP.Models
{
    public partial class bdlumiereContext : DbContext
    {
        public bdlumiereContext()
        {
        }

        public bdlumiereContext(DbContextOptions<bdlumiereContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categorium> Categoria { get; set; } = null!;
        public virtual DbSet<Comentario> Comentarios { get; set; } = null!;
        public virtual DbSet<Director> Directors { get; set; } = null!;
        public virtual DbSet<Estanterium> Estanteria { get; set; } = null!;
        public virtual DbSet<Idioma> Idiomas { get; set; } = null!;
        public virtual DbSet<Pelicula> Peliculas { get; set; } = null!;
        public virtual DbSet<PeliculaCategorium> PeliculaCategoria { get; set; } = null!;
        public virtual DbSet<PeliculaDirector> PeliculaDirectors { get; set; } = null!;
        public virtual DbSet<PeliculaEstanterium> PeliculaEstanteria { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<ValoracionUsuario> ValoracionUsuarios { get; set; } = null!;

     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PRIMARY");

                entity.ToTable("categoria");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IdCategoria)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_categoria");

                entity.Property(e => e.IdTmdbCategoria)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_tmdbCat");

                entity.Property(e => e.NomCategoria)
                    .HasMaxLength(100)
                    .HasColumnName("nom_categoria");
            });

            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.IdComentario)
                    .HasName("PRIMARY");

                entity.ToTable("comentario");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdPelicula, "id_pelicula");

                entity.HasIndex(e => e.IdUsuario, "id_usuario");

                entity.Property(e => e.IdComentario)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_comentario");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.FechaPublicacion)
                    .HasMaxLength(15)
                    .HasColumnName("fecha_publicacion");

                entity.Property(e => e.IdPelicula)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_pelicula");

                entity.Property(e => e.IdUsuario)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_usuario");

                entity.Property(e => e.Texto)
                    .HasMaxLength(500)
                    .HasColumnName("texto");
            });

            modelBuilder.Entity<Director>(entity =>
            {
                entity.HasKey(e => e.IdDirector)
                    .HasName("PRIMARY");

                entity.ToTable("director");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IdDirector)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_director");

                entity.Property(e => e.IdDirTmdb)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_dir_tmdb");

                entity.Property(e => e.BioDirector)
                    .HasMaxLength(2500)
                    .HasColumnName("bio_director");

                entity.Property(e => e.NomDirector)
                    .HasMaxLength(50)
                    .HasColumnName("nom_director");

                entity.Property(e => e.UrlFoto)
                    .HasMaxLength(100)
                    .HasColumnName("url_foto");
            });

            modelBuilder.Entity<Estanterium>(entity =>
            {
                entity.HasKey(e => e.IdEstanteria)
                    .HasName("PRIMARY");

                entity.ToTable("estanteria");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdUsuario, "id_usuario");

                entity.Property(e => e.IdEstanteria)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_estanteria");

                entity.Property(e => e.EsEditable).HasColumnName("es_editable");

                entity.Property(e => e.FechaCreacion)
                    .HasMaxLength(15)    
                    .HasColumnName("fecha_creacion");

                entity.Property(e => e.IdUsuario)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_usuario");

                entity.Property(e => e.NomEstanteria)
                    .HasMaxLength(100)
                    .HasColumnName("nom_estanteria");
            });

            modelBuilder.Entity<Idioma>(entity =>
            {
                entity.HasKey(e => e.IdIdioma)
                    .HasName("PRIMARY");

                entity.ToTable("idioma");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IdIdioma)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_idioma");

                entity.Property(e => e.NomIdioma)
                    .HasMaxLength(50)
                    .HasColumnName("nom_idioma");

                entity.Property(e => e.Abreviacion)
                    .HasMaxLength(10)
                    .HasColumnName("abreviacion");
            });

            modelBuilder.Entity<Pelicula>(entity =>
            {
                entity.HasKey(e => e.IdPelicula)
                    .HasName("PRIMARY");

                entity.ToTable("pelicula");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdIdioma, "id_idioma");

                entity.Property(e => e.IdTmdb)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_tmdb");

                entity.Property(e => e.IdPelicula)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_pelicula");

                entity.Property(e => e.Valoracion)
                    .HasPrecision(4,2)
                    .HasColumnName("valoracion");

                entity.Property(e => e.FechaEstreno)
                    .HasMaxLength(15)
                    .HasColumnName("fecha_estreno");

                entity.Property(e => e.IdIdioma)
                    .HasColumnType("int(15)")
                    .HasColumnName("id_idioma");

                entity.Property(e => e.NomPelicula)
                    .HasMaxLength(100)
                    .HasColumnName("nom_pelicula");

                entity.Property(e => e.Sumilla)
                    .HasMaxLength(2500)
                    .HasColumnName("sumilla");

                entity.Property(e => e.UrlFoto)
                    .HasMaxLength(500)
                    .HasColumnName("url_foto");
            });

            modelBuilder.Entity<PeliculaCategorium>(entity =>
            {
                entity.HasKey(e => new { e.IdPelicula, e.IdCategoria })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("pelicula_categoria");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdCategoria, "id_categoria");

                entity.Property(e => e.IdPelicula)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_pelicula");

                entity.Property(e => e.IdCategoria)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_categoria");
            });

            modelBuilder.Entity<PeliculaDirector>(entity =>
            {
                entity.HasKey(e => new { e.IdPelicula, e.IdDirector })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("pelicula_director");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdDirector, "id_director");

                entity.Property(e => e.IdPelicula)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_pelicula");

                entity.Property(e => e.IdDirector)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_director");
            });

            modelBuilder.Entity<PeliculaEstanterium>(entity =>
            {
                entity.HasKey(e => new { e.IdPelicula, e.IdEstanteria })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("pelicula_estanteria");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdEstanteria, "id_estanteria");

                entity.Property(e => e.IdPelicula)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_pelicula");

                entity.Property(e => e.IdEstanteria)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_estanteria");

                entity.Property(e => e.FechaAgregacion)
                    .HasMaxLength(15)
                    .HasColumnName("fecha_agregacion");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PRIMARY");

                entity.ToTable("usuario");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IdUsuario)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_usuario");

                entity.Property(e => e.ApeUsuario)
                    .HasMaxLength(100)
                    .HasColumnName("ape_usuario");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(100)
                    .HasColumnName("contraseña");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaCreacion)
                    .HasMaxLength(15)
                    .HasColumnName("fecha_creacion");

                entity.Property(e => e.FechaNacimiento)
                    .HasMaxLength(15)    
                    .HasColumnName("fecha_nacimiento");

                entity.Property(e => e.NomUsuario)
                    .HasMaxLength(100)
                    .HasColumnName("nom_usuario");

                entity.Property(e => e.UrlFoto)
                    .HasMaxLength(500)
                    .HasColumnName("url_foto");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<ValoracionUsuario>(entity =>
            {
                entity.HasKey(e => new { e.IdPelicula, e.IdUsuario })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("valoracion_usuario");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdUsuario, "id_usuario");

                entity.Property(e => e.IdPelicula)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_pelicula");

                entity.Property(e => e.IdUsuario)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_usuario");

                entity.Property(e => e.FechaValoracion)
                    .HasMaxLength(15)
                    .HasColumnName("fecha_valoracion");

                entity.Property(e => e.Valoracion)
                    .HasColumnType("int(11)")
                    .HasColumnName("valoracion");

                entity.Property(e => e.EstaVisto)
                    .HasColumnType("int(1)")
                    .HasColumnName("estaVisto");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
