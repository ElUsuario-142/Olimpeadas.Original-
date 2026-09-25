using Olimpeadas.Aplication.DTOs.Categoria;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;

namespace Olimpeadas.Aplication.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaDTO>> GetAllAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            return categorias.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nombre = c.Nombre
            });
        }

        public async Task<CategoriaDTO?> GetByIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            return categoria == null ? null : new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

        public async Task<CategoriaDTO> CrearAsync(CrearCategoriaDTO dto)
        {
            var categoriaExistente = await _categoriaRepository.GetByNombreAsync(dto.Nombre);
            if (categoriaExistente != null)
                throw new InvalidOperationException($"La categoría '{dto.Nombre}' ya existe.");

            var categoria = new Categoria
            {
                Nombre = dto.Nombre
            };

            await _categoriaRepository.AddAsync(categoria);
            await _categoriaRepository.SaveChangesAsync();

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }
    }
}
