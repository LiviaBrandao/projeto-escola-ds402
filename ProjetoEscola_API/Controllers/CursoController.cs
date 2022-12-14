using Microsoft.AspNetCore.Mvc;
using ProjetoEscola_API.Data;
using ProjetoEscola_API.Models;

namespace ProjetoEscola_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CursoController : ControllerBase
    {
        private EscolaContext _context;
        public CursoController(EscolaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Curso>> GetAll()
        {
            return _context.Curso.ToList();
        }

        //metodos mais especificos -----------------------------------

        [HttpGet("{CursoId}")]
        public ActionResult<List<Curso>> Get(int CursoId)
        {
            try
            {
                var result = _context.Curso.Find(CursoId);
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
        public async Task<ActionResult> post(Curso model)
        {
            try
            {
                _context.Curso.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created("/api/Curso/{model.codCurso}", model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        // fix this put function
        [HttpPut("{CodCurso}")]
        public async Task<IActionResult> put(int CursoId, Curso dadosCursoAlt)
        {
            try
            {
                //verifica se existe Curso a ser alterado
                var result = await _context.Curso.FindAsync(CursoId);
                if (CursoId != result.id)
                {
                    return BadRequest();
                }

                result.codCurso = dadosCursoAlt.codCurso;
                result.nomeCurso = dadosCursoAlt.nomeCurso;
                result.periodo = dadosCursoAlt.periodo;

                await _context.SaveChangesAsync();
                return Created($"/api/Curso/{dadosCursoAlt.codCurso}", dadosCursoAlt);
            }
            catch(Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("{CodCurso}")]
        public async Task<ActionResult> delete(int id)
        {
            System.Console.WriteLine("\n\n\n\nCurso:" + id);
            try
            {
                //verifica se existe Curso a ser excluído
                var Curso = await _context.Curso.FindAsync(id);
                System.Console.WriteLine("\n\n\n\nCurso:" + Curso);
                if (Curso == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(Curso);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

        
    }
}