using System.ComponentModel;
using System.Globalization;

namespace App.validate
{
    public enum CreatePacientError
    {
        [Description("\nErro: o CPF deve ser válido\n")]
        INVALID_CPF = 0,

        [Description("\nErro: o nome deve ter pelo menos 5 caracteres.\n")]
        INVALID_NAME = 1,

        [Description("\nErro: CPF já cadastrado\n")]
        CPF_ALREADY_EXISTS = 2,

        [Description("\nErro: data no formato incorreto.\n")]
        WRONG_DATE_FORMAT = 3,

        [Description("\nErro: paciente deve ter pelo menos 13 anos.\n")]
        LESS_THAN_13 = 4
    }


    public static class EnumExtensions
    {
        public static string ToDescriptionString(this CreatePacientError val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this RemovePacientError val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }

    public class CreatePacientValidator : IValidator<CreatePacientError>
    {
        private Database Database { get => Database.GetInstance(); }
        public List<CreatePacientError> Errors { get; set; }

        public bool IsValid()
        {
            return Errors.Count == 0;
        }

        public CreatePacientValidator()
        {
            this.Errors = new();
        }

        public bool ValidateName(string name)
        {
            if (name.Length < 5)
            {
                Errors.Add(CreatePacientError.INVALID_NAME);
                return false;
            }
            return true;
        }
        public bool ValidateCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            {
                Errors.Add(CreatePacientError.INVALID_CPF);
                return false;
            }

            bool IsValidCPF(string cpf)
            {
                int CalculateDigit(string baseCpf, int[] weight)
                {
                    int sum = baseCpf.Zip(weight, (digit, factor) => (digit - '0') * factor).Sum();
                    int remainder = sum % 11;
                    return remainder < 2 ? 0 : 11 - remainder;
                }

                var firstWeight = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                var secondWeight = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                string baseCpf = cpf.Substring(0, 9);
                int firstDigit = CalculateDigit(baseCpf, firstWeight);
                int secondDigit = CalculateDigit(baseCpf + firstDigit, secondWeight);

                return cpf.EndsWith($"{firstDigit}{secondDigit}");
            }

            if (!IsValidCPF(cpf))
            {
                Errors.Add(CreatePacientError.INVALID_CPF);
                return false;
            }

            if (Database.CPFs.Contains(cpf))
            {
                Errors.Add(CreatePacientError.CPF_ALREADY_EXISTS);
                return false;
            }

            return true;
        }


        public bool ValidateBirthDate(string birthDate)
        {
            if (!DateTime.TryParseExact(birthDate, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime BirthDate))
            {
                Errors.Add(CreatePacientError.WRONG_DATE_FORMAT);
                return false;
            }

            if (BirthDate >= DateTime.Now.AddYears(-13))
            {
                Errors.Add(CreatePacientError.LESS_THAN_13);
                return false;
            }
            return true;
        }

        public List<CreatePacientError> Validate(string cpf, string name, string birthDate)
        {
            ValidateCPF(cpf);
            ValidateName(name);
            ValidateBirthDate(birthDate);

            return Errors;
        }

        public void Clear()
        {
            Errors.Clear();
        }
    }

    public enum RemovePacientError
    {
        [Description("\nErro: o CPF deve ser válido\n")]
        INVALID_CPF = 0,

        [Description("\nErro: paciente não cadastrado\n")]
        NON_EXISTENT_CPF = 1,

        [Description("\nErro: paciente está agendado.\n")]
        SCHEDULED_PACIENT = 2,
    }
    // TODO: check if the pacient is scheduled
    public class RemovePacientValidator : IValidator<RemovePacientError>
    {
        private Database Database { get => Database.GetInstance(); }
        public List<RemovePacientError> Errors { get; set; }

        public bool ValidateCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            {
                Errors.Add(RemovePacientError.INVALID_CPF);
                return false;
            }

            bool IsValidCPF(string cpf)
            {
                int CalculateDigit(string baseCpf, int[] weight)
                {
                    int sum = baseCpf.Zip(weight, (digit, factor) => (digit - '0') * factor).Sum();
                    int remainder = sum % 11;
                    return remainder < 2 ? 0 : 11 - remainder;
                }

                var firstWeight = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                var secondWeight = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                string baseCpf = cpf.Substring(0, 9);
                int firstDigit = CalculateDigit(baseCpf, firstWeight);
                int secondDigit = CalculateDigit(baseCpf + firstDigit, secondWeight);

                return cpf.EndsWith($"{firstDigit}{secondDigit}");
            }

            if (!IsValidCPF(cpf))
            {
                Errors.Add(RemovePacientError.INVALID_CPF);
                return false;
            }

            if (!Database.CPFs.Contains(cpf))
            {
                Errors.Add(RemovePacientError.NON_EXISTENT_CPF);
                return false;
            }

            

            return true;
        }
    }
}
