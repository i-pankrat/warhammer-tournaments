using System.ComponentModel.DataAnnotations;

namespace WarhammerTournaments.Models;

public class EditTournamentViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название турнира")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Введите описание турнира")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Расскажите о правилах турнира")]
    public string Rules { get; set; }

    [Required(ErrorMessage = "Укажите местопроведения")]
    public string Address { get; set; }

    public string OwnerId { get; set; }
    public string OwnerUserName { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Введите положительное число")]
    public int AvailableParticipant { get; set; }

    [Required(ErrorMessage = "Введите дату проведения турнира")]
    public DateTime Date { get; set; }

    public IFormFile? Image { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Введите неотрицательное число")]
    [Required(ErrorMessage = "Укажите входной взнос")]
    public int EntranceFee { get; set; }
}