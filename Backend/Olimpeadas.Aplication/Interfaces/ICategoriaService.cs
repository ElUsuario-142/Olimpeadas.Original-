using Olimpeadas.Aplication.DTOs.Categoria;

namespace Olimpeadas.Aplication.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDTO>> GetAllAsync();
        Task<CategoriaDTO?> GetByIdAsync(int id);
        Task<CategoriaDTO> CrearAsync(CrearCategoriaDTO dto);
    }
}
