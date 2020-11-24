﻿using System.Collections.Generic;
using ChessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChessWeb.Domain.Maps
{
    public class GameMap : IEntityBuilder<Game>
    {
        public GameMap(EntityTypeBuilder<Game> builder) => BuildEntity(builder);

        public void BuildEntity(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Fen).HasMaxLength(100);
            builder.HasData(
                new
                {
                    Id = 1L,
                    Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
                });
            builder.OwnsOne(e => e.ChessGameInfo).HasData(
                new
                {
                    HasWhiteKingMoved = false,
                    HasBlackKingMoved = false,
                    HasWhiteQueensideRookMoved = false,
                    HasWhiteKingsideRookMoved = false,
                    HasBlackQueensideRookMoved = false,
                    HasBlackKingsideRookMoved = false,
                    WhiteKingSquare = "e1",
                    BlackKingSquare = "e8",
                    GameId = 1L
                });
        }
    }
}