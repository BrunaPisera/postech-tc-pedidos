﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pedidos.Infrastructure.Data;

#nullable disable

namespace Pedidos.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20250203143720_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Pedidos.Core.Entities.ClienteAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Nome")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("Pedidos.Core.Entities.ItemPedido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Customizacao")
                        .HasColumnType("text");

                    b.Property<Guid?>("PedidoAggregateId")
                        .HasColumnType("uuid");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("integer");

                    b.Property<short>("Quantidade")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("PedidoAggregateId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ItemPedido");
                });

            modelBuilder.Entity("Pedidos.Core.Entities.PedidoAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ClienteId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("HorarioRecebimento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("PagamentoConfirmado")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("Pedidos.Core.Entities.ProdutoAggregate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Categoria")
                        .HasColumnType("integer");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Categoria")
                        .HasDatabaseName("IX_Categoria");

                    b.ToTable("Produto");
                });

            modelBuilder.Entity("Pedidos.Core.Entities.ClienteAggregate", b =>
                {
                    b.OwnsOne("Pedidos.Core.Entities.ValueObjects.CPF", "CPF", b1 =>
                        {
                            b1.Property<Guid>("ClienteAggregateId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("CPF");

                            b1.HasKey("ClienteAggregateId");

                            b1.ToTable("Cliente");

                            b1.WithOwner()
                                .HasForeignKey("ClienteAggregateId");
                        });

                    b.OwnsOne("Pedidos.Core.Entities.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("ClienteAggregateId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Endereco")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Email");

                            b1.HasKey("ClienteAggregateId");

                            b1.ToTable("Cliente");

                            b1.WithOwner()
                                .HasForeignKey("ClienteAggregateId");
                        });

                    b.Navigation("CPF");

                    b.Navigation("Email");
                });

            modelBuilder.Entity("Pedidos.Core.Entities.ItemPedido", b =>
                {
                    b.HasOne("Pedidos.Core.Entities.PedidoAggregate", null)
                        .WithMany("Itens")
                        .HasForeignKey("PedidoAggregateId");

                    b.HasOne("Pedidos.Core.Entities.ProdutoAggregate", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("Pedidos.Core.Entities.PedidoAggregate", b =>
                {
                    b.HasOne("Pedidos.Core.Entities.ClienteAggregate", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId");

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("Pedidos.Core.Entities.ProdutoAggregate", b =>
                {
                    b.OwnsOne("Pedidos.Core.Entities.ValueObjects.Preco", "Preco", b1 =>
                        {
                            b1.Property<int>("ProdutoAggregateId")
                                .HasColumnType("integer");

                            b1.Property<double>("Value")
                                .HasColumnType("double precision")
                                .HasColumnName("Preco");

                            b1.HasKey("ProdutoAggregateId");

                            b1.ToTable("Produto");

                            b1.WithOwner()
                                .HasForeignKey("ProdutoAggregateId");
                        });

                    b.Navigation("Preco")
                        .IsRequired();
                });

            modelBuilder.Entity("Pedidos.Core.Entities.PedidoAggregate", b =>
                {
                    b.Navigation("Itens");
                });
#pragma warning restore 612, 618
        }
    }
}
