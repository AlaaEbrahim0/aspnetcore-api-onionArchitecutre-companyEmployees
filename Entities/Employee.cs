using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;
public class Employee
{
    [Column("EmployeeId")]
    public int Id { get; set; }

	[Required(ErrorMessage = "Employee name is a required field.")]
	[MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
	public string? Name { get; set; }

	[Required(ErrorMessage = "Age is a required field.")] 
	public int Age { get; set; }

	[Required(ErrorMessage = "Position is a required field.")]
	[MaxLength(50, ErrorMessage = "Maximum length for the Position is 20 characters.")]
	public string? Position { get; set; }

	[ForeignKey(nameof(Company))]
	public int CompanyId { get; set; }
	public Company? Company { get; set; }
}