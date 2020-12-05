﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ChessWeb.Application.ViewModels.Game;
using ChessWeb.Domain.Entities;
using ChessWeb.Domain.Interfaces.Repositories;
using ChessWeb.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChessWeb.Application.Controllers
{
    public class GamesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IGameService _gameService;
        private readonly ISideRepository _sideRepository;
        
        public GamesController(UserManager<User> userManager, IGameService gameService, ISideRepository sideRepository)
        {
            _userManager = userManager;
            _gameService = gameService;
            _sideRepository = sideRepository;
        }

        public async Task<IActionResult> Index() =>
            View(await _gameService.GetAllAsync());

        [Authorize]
        public async Task<IActionResult> Create()
        {
            await _gameService.CreateGameAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Delete()
        {
            throw new NotImplementedException(nameof(Delete));
        }

        public async Task<IActionResult> GameSides(long id)
        {
            var game = await _gameService.GetAsync(id);
            if (game == null)
                return NotFound();
            var sides = (await _sideRepository.GetGameSides(game)).ToList();
            return View(new GameViewModel(game, sides));
        }

        public IActionResult Play(string userName)
        {
            throw new NotImplementedException(nameof(Play));
        }
        
        [Authorize]
        public async Task<IActionResult> Join(long sideId)
        {
            var userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var side = await _sideRepository.FindAsync(sideId);
            await _gameService.JoinAsync(user, side);
            return RedirectToAction("Index");
        }
    }
}