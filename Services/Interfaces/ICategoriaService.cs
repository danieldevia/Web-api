using InventarioApi.Models;
using InventarioApi.Models.DTOs;

namespace InventarioApi.Services.Interfaces
{
    public interface ICategoriaService
    {
        List<CategoriaResponseDto> GetAll();
        Categoria? GetById(int id);
        void Add(CategoriaCreateDto dto);
        void Delete(int id);
    }
}