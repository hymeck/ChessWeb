﻿// <auto-generated />
using System;
using ChessWeb.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChessWeb.Persistence.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20201126220654_Game_withOne-to-One")]
    partial class Game_withOnetoOne
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ChessWeb.Domain.Entities.ChessGameInfo", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("BlackKingSquare")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<bool>("HasBlackKingMoved")
                        .HasColumnType("bit");

                    b.Property<bool>("HasBlackKingsideRookMoved")
                        .HasColumnType("bit");

                    b.Property<bool>("HasBlackQueensideRookMoved")
                        .HasColumnType("bit");

                    b.Property<bool>("HasWhiteKingMoved")
                        .HasColumnType("bit");

                    b.Property<bool>("HasWhiteKingsideRookMoved")
                        .HasColumnType("bit");

                    b.Property<bool>("HasWhiteQueensideRookMoved")
                        .HasColumnType("bit");

                    b.Property<string>("WhiteKingSquare")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("Id");

                    b.ToTable("ChessGameInfo");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            BlackKingSquare = "e8",
                            HasBlackKingMoved = false,
                            HasBlackKingsideRookMoved = false,
                            HasBlackQueensideRookMoved = false,
                            HasWhiteKingMoved = false,
                            HasWhiteKingsideRookMoved = false,
                            HasWhiteQueensideRookMoved = false,
                            WhiteKingSquare = "e1"
                        });
                });

            modelBuilder.Entity("ChessWeb.Domain.Entities.Color", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<bool>("ColorType")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ColorType")
                        .IsUnique();

                    b.ToTable("Colors");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            ColorType = true
                        },
                        new
                        {
                            Id = 2L,
                            ColorType = false
                        });
                });

            modelBuilder.Entity("ChessWeb.Domain.Entities.Game", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Fen")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Games");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
                        });
                });

            modelBuilder.Entity("ChessWeb.Domain.Entities.Move", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Fen")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<long?>("GameId")
                        .HasColumnType("bigint");

                    b.Property<string>("MoveNext")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<long?>("PlayerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Moves");
                });

            modelBuilder.Entity("ChessWeb.Domain.Entities.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Nickname")
                        .IsUnique()
                        .HasFilter("[Nickname] IS NOT NULL");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Email = "noonimf@gmail.com",
                            Nickname = "Hymeck",
                            Password = "hymeckpass"
                        },
                        new
                        {
                            Id = 2L,
                            Email = "mr.yatson@gmail.com",
                            Nickname = "Racoty",
                            Password = "racotypass"
                        },
                        new
                        {
                            Id = 3L,
                            Email = "vadimyaren@yandex.by",
                            Nickname = "Yaren",
                            Password = "yarenpass"
                        },
                        new
                        {
                            Id = 4L,
                            Email = "some_email@gmail.com",
                            Nickname = "Someone",
                            Password = "someonepass"
                        });
                });

            modelBuilder.Entity("ChessWeb.Domain.Entities.Side", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long?>("ColorId")
                        .HasColumnType("bigint");

                    b.Property<long?>("GameId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ColorId");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Sides");
                });

            modelBuilder.Entity("ChessWeb.Domain.Entities.ChessGameInfo", b =>
                {
                    b.HasOne("ChessWeb.Domain.Entities.Game", "Game")
                        .WithOne("ChessGameInfo")
                        .HasForeignKey("ChessWeb.Domain.Entities.ChessGameInfo", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("ChessWeb.Domain.Entities.Move", b =>
                {
                    b.HasOne("ChessWeb.Domain.Entities.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("ChessWeb.Domain.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("ChessWeb.Domain.Entities.Side", b =>
                {
                    b.HasOne("ChessWeb.Domain.Entities.Color", "Color")
                        .WithMany()
                        .HasForeignKey("ColorId");

                    b.HasOne("ChessWeb.Domain.Entities.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("ChessWeb.Domain.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Color");

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("ChessWeb.Domain.Entities.Game", b =>
                {
                    b.Navigation("ChessGameInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
