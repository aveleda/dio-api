using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.Id, dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }

        [Route("{id:int}")]
        [HttpGet]
        public ActionResult ObterInfectado(int id)
        {
            var filter = Builders<Infectado>.Filter.Eq("Id", id);
            
            var infectado = _infectadosCollection.Find(filter).ToList();
            
            return Ok(infectado);
        }

        [HttpPut("{id}")]
        public ActionResult AtualizarInfectados(int id, [FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.Id, dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            var filter = Builders<Infectado>.Filter.Eq("Id", id);
            
            _infectadosCollection.ReplaceOne(filter, infectado);
            
            return Ok("Atualizado com sucesso");
        }

        [HttpDelete("{id}")]
        public ActionResult RemoverInfectado(int id)
        {
            var filter = Builders<Infectado>.Filter.Eq("Id", id);
            
            _infectadosCollection.DeleteOne(filter);

            return Ok("Removido com sucesso");
        }
    }
}