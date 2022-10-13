using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProjetoEscola_API.Data;
using ProjetoEscola_API.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProjetoEscola_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AlunoController : ControllerBase
    {
        private readonly EscolaContext _context;
        public AlunoController(EscolaContext context)
        {
            // construtor
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Aluno>> GetAll()
        {
            return _context.Aluno.ToList();
        }

        [HttpGet("{AlunoId}")]
        public ActionResult<List<Aluno>> Get(int AlunoId)
        {
            try
            {
                var result = _context.Aluno.Find(AlunoId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> post(Aluno model)
        {
            System.Console.WriteLine("\n\n\nENTROU NO POST\n\n\n");
            try
            {
                _context.Aluno.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created($"/api/aluno/{model.ra}", model);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("message:" + e.Message, e.StackTrace);
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        // fix this put function
        [HttpPut("{AlunoRA}")]
        public async Task<IActionResult> put(int AlunoId, Aluno dadosAlunoAlt)
        {
            try
            {
                //verifica se existe aluno a ser alterado
                var result = await _context.Aluno.FindAsync(AlunoId);
                if (AlunoId != result.id)
                {
                    return BadRequest();
                }
                result.ra = dadosAlunoAlt.ra;
                result.nome = dadosAlunoAlt.nome;
                result.codCurso = dadosAlunoAlt.codCurso;
                await _context.SaveChangesAsync();
                return Created($"/api/aluno/{dadosAlunoAlt.ra}", dadosAlunoAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
        [HttpDelete("{AlunoRA}")]
        public async Task<ActionResult> delete(int AlunoId)
        {
            try
            {
                 System.Console.WriteLine("\n\n\n\nEntrou no delete:" + AlunoId);
                //verifica se existe aluno a ser excluído
                var aluno = _context.Aluno.FindAsync(AlunoId);
                if (aluno == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(aluno);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }
    }
}