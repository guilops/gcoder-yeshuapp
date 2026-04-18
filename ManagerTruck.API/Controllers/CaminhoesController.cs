using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagerTruck.Context;
using ManagerTruck.Dtos;
using ManagerTruck.Entities;

namespace ManagerTruck.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CaminhoesController : ControllerBase
    {
        private readonly ILogger<CaminhoesController> _logger;
        private readonly AppDbContext _context;

        public CaminhoesController(ILogger<CaminhoesController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("/caminhoes")]
        public async Task<IActionResult> ListarCaminhoes()
        {
            var caminhoesEntity = await _context.Caminhoes.ToListAsync();

            if (!caminhoesEntity.Any()) return NotFound("caminhoes nao localizadas");

            var lista = await _context.Caminhoes
                .Include(c => c.Montadora)
                .ToListAsync();

            var result = lista.Select(c => new CaminhaoDto
            {
                Id = c.Id,
                Placa = c.Placa,
                Modelo = c.Modelo,
                AnoFabricacao = c.AnoFabricacao,
                AnoModelo = c.AnoModelo,
                FotoUrl = c.FotoUrl,
                KmAtual = c.KmAtual,
                KmUltimaManutencao = c.KmUltimaManutencao,
                MontadoraId = c.MontadoraId,
                Montadora = c.Montadora.Nome,
                PrecisaManutencao = c.KmUltimaManutencao == 0 ? false : (c.KmAtual - c.KmUltimaManutencao) >= 10000,
                ManutencaoProxima = c.KmUltimaManutencao == 0 ? false : (c.KmAtual - c.KmUltimaManutencao) >= 9000
            }).ToList();

            return Ok(result);
        }

        [HttpGet("/caminhoes/{id:int}")]
        public async Task<IActionResult> ListarCaminhaoPorId(int id)
        {
            var caminhaoEntity = await _context.Caminhoes
                                        .Include(c => c.Montadora)
                                        .FirstOrDefaultAsync(x => x.Id == id);

            if (caminhaoEntity is null) return NotFound("Caminhao nao localizado");

            return Ok(new CaminhaoDto
            {
                Id = caminhaoEntity.Id,
                AnoFabricacao = caminhaoEntity.AnoFabricacao,
                AnoModelo = caminhaoEntity.AnoModelo,
                Placa = caminhaoEntity.Placa,
                Modelo = caminhaoEntity.Modelo,
                FotoUrl = caminhaoEntity.FotoUrl,
                KmAtual = caminhaoEntity.KmAtual,
                KmUltimaManutencao = caminhaoEntity.KmUltimaManutencao,
                MontadoraId = caminhaoEntity.Montadora.Id,
                Montadora = caminhaoEntity.Montadora.Nome,
                PrecisaManutencao = caminhaoEntity.KmUltimaManutencao == 0 ? false : (caminhaoEntity.KmAtual - caminhaoEntity.KmUltimaManutencao) >= 10000,
                ManutencaoProxima = caminhaoEntity.KmUltimaManutencao == 0 ? false : (caminhaoEntity.KmAtual - caminhaoEntity.KmUltimaManutencao) >= 9000
            });
        }

        [HttpPost("/caminhoes")]
        public async Task<IActionResult> SalvarCaminhao(CaminhaoDto caminhaoDto)
        {
            var caminhaoEntity = new CaminhaoEntity
            {
                Placa = caminhaoDto.Placa,
                DataCriacao = DateTime.UtcNow,
                Historico = caminhaoDto.Historico.Select(x => new CaminhaoHistoricoEntity
                {
                    Data = DateTime.SpecifyKind(x.Data, DateTimeKind.Utc),
                    Descricao = x.Descricao,
                    KmRodado = x.KmRodado,
                    Tipo = x.Tipo,
                    Valor = x.Valor
                }).ToList(),
                FotoUrl = caminhaoDto.FotoUrl,
                AnoFabricacao = caminhaoDto.AnoFabricacao,
                AnoModelo = caminhaoDto.AnoModelo,
                Modelo = caminhaoDto.Modelo,
                KmAtual = caminhaoDto.KmAtual,
                MontadoraId = caminhaoDto.MontadoraId
            };
            _context.Caminhoes.Add(caminhaoEntity);
            await _context.SaveChangesAsync();

            return Created($"/caminhoes/{caminhaoEntity.Id}", caminhaoEntity);
        }

        [HttpPut("/caminhoes/{id}")]
        public async Task<IActionResult> AtualizarCaminhao(int id, CaminhaoDto caminhaoDto)
        {
            var caminhaoEntity = await _context.Caminhoes.FirstOrDefaultAsync(x => x.Id == id);

            if (caminhaoEntity is null) return NotFound("Caminhao nao localizado");

            caminhaoEntity.Placa = caminhaoDto.Placa;
            caminhaoEntity.DataCriacao = DateTime.UtcNow;
            caminhaoEntity.Historico = caminhaoDto.Historico.Select(x => new CaminhaoHistoricoEntity
            {
                Data = DateTime.SpecifyKind(x.Data, DateTimeKind.Utc),
                Descricao = x.Descricao,
                KmRodado = x.KmRodado,
                Tipo = x.Tipo,
                Valor = x.Valor
            }).ToList();
            caminhaoEntity.FotoUrl = caminhaoDto.FotoUrl;
            caminhaoEntity.AnoFabricacao = caminhaoDto.AnoFabricacao;
            caminhaoEntity.AnoModelo = caminhaoDto.AnoModelo;
            caminhaoEntity.Modelo = caminhaoDto.Modelo;
            caminhaoEntity.KmAtual = caminhaoDto.KmAtual;
            caminhaoEntity.MontadoraId = caminhaoDto.MontadoraId;

            _context.Caminhoes.Update(caminhaoEntity);
            await _context.SaveChangesAsync();

            return Ok(caminhaoEntity);
        }


        [HttpDelete("/caminhoes/{id}")]
        public async Task<IActionResult> ExcluirCaminhao(int id)
        {
            var caminhaoEntity = await _context.Caminhoes.FirstOrDefaultAsync(x => x.Id == id);

            if (caminhaoEntity is null) return NotFound("Caminhao nao localizado");

            _context.Caminhoes.Remove(caminhaoEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
