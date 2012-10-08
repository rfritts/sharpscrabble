﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scrabble.Web.Models;
using Scrabble.Core;
using Scrabble.Core.Types;

namespace Scrabble.Web.Controllers
{
    public class PlayController : Controller
    {
        public ActionResult Index(String id)
        {
            Board b = Game.Instance.PlayingBoard;
            foreach (KeyValuePair<Core.Config.Coordinate, Core.Squares.Square> pair in b.OccupiedSquares())
            {
                //Do something lol
            }
            return View(new PlayModel(id, GameState()));
        }

        public ActionResult Continue(String id)
        {
            GameState().Continue();
            return Content("true");
        }

        [HttpPost]
        public ActionResult TakeTurn(String id, TurnInput input)
        {
            GameState state = GameState();
            HumanPlayer current = state.HumanPlayers.First();
            if (current == null)
                throw new Exception("Current player not found");

            try
            {
                current.TakeTurn(input.ToTurn(current));
                return Json(new TurnResponse(true, "Valid"));
            }
            catch (InvalidMoveException e)
            {
                return Json(new TurnResponse(false, e.Message));
            }
        }

        private GameState GameState()
        {
            GameState state = new SessionGameLoader().Load();
            if (state == null)
                throw new HttpException(400, "Game state not found");
            return state;
        }
    }
}
