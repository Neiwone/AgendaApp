using App.utils;
using App.validate;
using System.ComponentModel.DataAnnotations;

namespace App.models
{
    public class Pacient
    {
        private static readonly CreatePacientValidator validator = new();
        [Required]
        public string Name { get; private set; }
        [Key]
        public string CPF { get; private set; }
        [Required]
        public DateOnly BirthDate { get; private set; }

        private Pacient(string CPF, string Name, DateOnly BirthDate)
        {
            this.Name = Name;
            this.CPF = CPF;
            this.BirthDate = BirthDate;
        }

        public static Result<Pacient, CreatePacientError> Create(string cpf, string name, string birthDate)
        {
            List<CreatePacientError> errors = validator.Validate(cpf, name, birthDate);

            if (errors.Count != 0)
            {
                return Result<Pacient, CreatePacientError>.OnFailure(errors);
            }
            else
            {
                string _name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                DateOnly _birthDate = DateOnly.Parse(birthDate);

                return Result<Pacient, CreatePacientError>.OnSuccess(new Pacient(cpf, _name, _birthDate));
            }
            
        }
    }

}
