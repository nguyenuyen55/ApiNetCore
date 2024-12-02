using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SupperHeroController : ControllerBase
    {
        private readonly DataContext _context;
        public SupperHeroController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllHeroes()
        {
            var heroes = await _context.SuperHeroes.ToListAsync();
            return Ok(heroes);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHero(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
          if(hero == null)
            {
                return NotFound("Hero not found.");
            }
            return Ok(hero);
        }
        [HttpPost]
        public async Task<IActionResult> AddHero([FromBody] SupperHero hero)
        {
           _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(await _context.SuperHeroes.ToListAsync());
        }
        [HttpPut]
        public async Task<ActionResult<List<SupperHero>>> UpdateHero([FromBody] SupperHero Updatehero)
        {
            var dbHero = await _context.SuperHeroes.FindAsync(Updatehero.id);
            if (dbHero == null)
            {
                return NotFound("Hero not found.");
            }
            dbHero.Name= Updatehero.Name;
            dbHero.FirstName= Updatehero.FirstName;
            dbHero.LastName= Updatehero.LastName;
            dbHero.Place= Updatehero.Place;

            await _context.SaveChangesAsync();
            return Ok(await _context.SuperHeroes.ToListAsync());
        }
        [HttpDelete]
        public async Task<ActionResult<List<SupperHero>>> UpdateHero(int id)
        {
            var dbHero = await _context.SuperHeroes.FindAsync(id);
            if (dbHero == null)
            {
                return NotFound("Hero not found.");
            }
            _context.SuperHeroes.Remove(dbHero);
            await _context.SaveChangesAsync();
            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }
}
