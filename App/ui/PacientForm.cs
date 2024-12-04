using App.models;
using App.validate;
using System.Globalization;

namespace App.ui
{
    internal class PacientForm
    {
        private static readonly CreatePacientValidator validator = new();

        public static string AskForCPF()
        {

            string cpf;
            while (true)
            {
                Console.Write("CPF: ");
                cpf = Console.ReadLine();

                validator.Clear();
                validator.ValidateCPF(cpf);
                if (validator.IsValid())
                {
                    return cpf.Trim();
                }

                foreach (var error in validator.Errors)
                {
                    Console.WriteLine(error.ToDescriptionString());
                }
            }
        }

        public static string AskForName()
        {
            string name;
            while (true)
            {
                Console.Write("Nome: ");
                name = Console.ReadLine();

                validator.Clear();
                validator.ValidateName(name);
                if (validator.IsValid())
                {
                    return name.Trim();
                }

                foreach (var error in validator.Errors)
                {
                    Console.WriteLine(error.ToDescriptionString());
                }
            }
        }

        public static string AskForBirthDate()
        {

            string birthDate;
            while (true)
            {
                Console.Write("Data de Nascimento: ");
                birthDate = Console.ReadLine();

                validator.Clear();
                validator.ValidateBirthDate(birthDate);
                if (validator.IsValid())
                {
                    return birthDate.Trim();
                }

                foreach (var error in validator.Errors)
                {
                    Console.WriteLine(error.ToDescriptionString());
                }
            }
        }

        public static void SuccessfulCreation()
        {
            Console.WriteLine("Paciente cadastrado com sucesso!");
            Thread.Sleep(1000);
        }
    }
}
