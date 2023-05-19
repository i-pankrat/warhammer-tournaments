using System.ComponentModel.DataAnnotations;

namespace WarhammerTournaments.Models;

public class EditTournamentViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название турнира")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Введите описание турнира")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Введите количество участников")]
    [Range(1, int.MaxValue, ErrorMessage = "Введите положительное число")]
    public int ParticipantNumber { get; set; }

    [Required(ErrorMessage = "Укажите дату проведение")]
    public DateTime Date { get; set; }
    
    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "Укажите входной взнос")]
    [Range(0, int.MaxValue, ErrorMessage = "Введите неотрицательное число")]
    public int EntranceFee { get; set; }
}