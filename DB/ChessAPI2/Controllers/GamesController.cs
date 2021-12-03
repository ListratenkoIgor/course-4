using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ChessAPI2.Models;

namespace ChessAPI2.Controllers
{
    public class GamesController : ApiController
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: api/Games
 /*       public Game GetGames()
        {

            Logic logic = new Logic();
            Game game = logic.GetCurrentGame();
            return game;
        }
*/
        public IQueryable<Game> GetGames() {

            Logic logic = new Logic();
            return logic.GetActiveGames();
           
        }
        // GET: api/Games/id
        [ResponseType(typeof(Game))]
        public Game GetGame(int id)
        {
            Logic logic = new Logic();
            Game game = logic.GetGame(id);
            return game;
            
        }
    
        // GET: api/Games/user/id
        public Game GetGame(string user,int id)
        {
            Logic logic = new Logic();
            Game game = logic.GetGame(id,user);
            return game;

        }
        public Game GetMove(int id, string move) {

            Logic logic = new Logic();
            Game game = logic.MakeMove(id,move);
            
            return game;
        }
        public Game GetMove(string user, int id, string move)
        {

            Logic logic = new Logic();
            Game game = logic.MakeMove(user,id, move);

            return game;
        }
        // PUT: api/Games/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGame(int id, Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != game.GameID)
            {
                return BadRequest();
            }

            db.Entry(game).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        

        // DELETE: api/Games/5

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GameExists(int id)
        {
            return db.Games.Count(e => e.GameID == id) > 0;
        }
    }
}