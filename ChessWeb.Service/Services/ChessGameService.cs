﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ChessEngine;
using ChessEngine.Console;
using ChessEngine.Domain;
using ChessEngine.Exceptions;
using ChessWeb.Domain.Entities;
using ChessWeb.Domain.Interfaces.Repositories;
using ChessWeb.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Color = ChessWeb.Domain.Entities.Color;

namespace ChessWeb.Service.Services
{
    public class ChessGameService : IChessGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ISideRepository _sideRepository;
        private readonly IMoveRepository _moveRepository;
        private readonly IColorRepository _colorRepository;
        private readonly UserManager<User> _userManager;

        public ChessGameService(
            IGameRepository gameRepository, 
            ISideRepository sideRepository, 
            IMoveRepository moveRepository, 
            IColorRepository colorRepository,
            UserManager<User> userManager)
        {
            _gameRepository = gameRepository;
            _sideRepository = sideRepository;
            _moveRepository = moveRepository;
            _colorRepository = colorRepository;
            _userManager = userManager;
        }

        // public Game MakeMove(Game game, Move move, Side side)
        // {
        //     // todo: uncomment this checks
        //     // if (side.User.Id != move.User.Id)
        //     //     return game;
        //     // if (game.Id != move.Game.Id || game.Id != side.Game.Id || move.Game.Id != side.Game.Id)
        //     //     return game;
        //     // if (move.Fen != game.Fen)
        //     //     return game;
        //     var fen = game.Fen;
        //     var chessGame = new ChessGame(fen);
        //     if (side.Color.ToChar() != (char) chessGame.ActiveColor)
        //         return game;
        //
        //     var moveNext = move.MoveNext;
        //     try
        //     {
        //         var squares = MoveInputParser.ParseMove(moveNext);
        //         var nextChessGame = chessGame.Move(squares);
        //
        //         game.Fen = nextChessGame.ToString();
        //
        //         _unitOfWork.Games.Update(game);
        //         _unitOfWork.Complete();
        //         return game;
        //     }
        //
        //     catch (FormatException e)
        //     {
        //         Debug.WriteLine(e.Message + "\n" +e.StackTrace);
        //         return game;
        //     }
        //
        //     catch (ChessGameException e)
        //     {
        //         Debug.WriteLine(e.Message + "\n" +e.StackTrace);
        //         return game;
        //     }
        // }
        //
        // public void AddToGame(User user, Game game, Color color)
        // {
        //     var gameSides = _unitOfWork.Sides.GetAll().Where(x => x.Game == game);
        //     
        //     if (gameSides.Count() >= 2)
        //         return;
        //
        //     var sideUser = gameSides.FirstOrDefault(x => x.User.UserName == user.UserName);
        //     if (sideUser != null)
        //         return;
        //     
        //     var colorSide = gameSides.FirstOrDefault(x => x.Color == color);
        //     if (colorSide != null) 
        //         return;
        //     
        //     var side = new Side
        //     {
        //         Color = color,
        //         Game = game,
        //         User = user
        //     };
        //     _unitOfWork.Sides.Add(side);
        //     _unitOfWork.Complete();
        // }
        public async Task<Game> MakeMove(long gameId, string username, string move)
        {
            var game = await _gameRepository.GetAsync(gameId);
            if (game == null)
                return game;

            if (game.IsDone || game.IsWaiting)
                return game;
            
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return game;

            var side = (await _sideRepository.GetGameSides(game))
                .FirstOrDefault(x => x.UserId == user.Id);

            if (side == null)
                return game;
            
            // // todo: uncomment this checks
            // if (side.UserId != move.UserId)
            //     return game;
            // if (game.Id != move.GameId || 
            //     game.Id != side.GameId || 
            //     move.GameId != side.GameId)
            //     return game;
            // if (move.Fen != game.Fen)
            //     return game;
            
            var fen = game.Fen;
            var chessGame = new ChessGame(fen);
            var color = await _colorRepository.FindAsync(side.ColorId);
            if (color.ToChar() != (char) chessGame.ActiveColor)
                return game;
            
            var moveEntry = new Move
            {
                Fen = game.Fen,
                Game = game,
                MoveNext = move,
                User = user
            };
            
            var moveNext = moveEntry.MoveNext;
            try
            {
                var squares = MoveInputParser.ParseMove(moveNext);
                var nextChessGame = chessGame.Move(squares);

                ChangeGameState(nextChessGame, ref game, moveEntry, color);

                await _moveRepository.AddAsync(moveEntry);
                await _gameRepository.UpdateAsync(game);
                await _gameRepository.SaveChangesAsync();
                return game;
            }

            catch (FormatException e)
            {
                Debug.WriteLine(e.Message + "\n" +e.StackTrace);
                return game;
            }
            
            catch (ChessGameException e)
            {
                Debug.WriteLine(e.Message + "\n" +e.StackTrace);
                return game;
            }
        }

        private void ChangeGameState(ChessGame chessGame, ref Game game, Move move, Color color)
        {
            game.Fen = chessGame.ToString();
            game.ActiveColor = chessGame.ActiveColor.IsWhite();
            game.LastMove = move.MoveNext;
            if (chessGame.IsCheckmate)
            {
                game.Winner = move.User.UserName;
                game.Status = FinalStatusFromColor(color);
            }
            else if (chessGame.IsStalemate)
            {
                game.Winner = "stalemate";
                game.Status = 2;
            }
        }

        private byte FinalStatusFromColor(Color color) => 
            color.ColorType ? 3 : 4;
        // public async Task AddToGame(User user, Game game, Color color)
        // {
        //     throw new NotImplementedException();
        // }
    }
}