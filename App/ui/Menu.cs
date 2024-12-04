using App.models;
using App.validate;
using System.Globalization;

namespace App.ui
{
    public class Menu
    {
        private static readonly CreatePacientValidator pacientvalidator = new();
        private static readonly RemovePacientValidator removevalidator = new();
        private static readonly AgendaValidator agendavalidator = new();
        public enum MainMENUOptions
        {
            REGISTER = 1,
            AGENDA = 2,
            END = 3
        }
        public MainMENUOptions ShowMainMenu()
        {
            Console.WriteLine("Menu Principal");
            Console.WriteLine("1-Cadastro de pacientes");
            Console.WriteLine("2-Agenda");
            Console.WriteLine("3-Fim");

            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    var choice = Int32.Parse(input);

                    if (choice < 1 || choice > 4)
                    {
                        Console.WriteLine("Opção inválida");
                        continue;
                    }
                    else
                    {
                        return (MainMENUOptions) choice;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Digite uma opção valida");
                    continue;
                }
            }
        }

        public enum RegisterMENUOptions
        {
            ADD = 1,
            REMOVE = 2,
            LISTBYCPF = 3,
            LISTBYNAME = 4,
            RETURN = 5
        }
        public RegisterMENUOptions ShowRegisterMenu()
        {
            Console.WriteLine("Menu do Cadastro de Pacientes");
            Console.WriteLine("1-Cadastrar novo paciente");
            Console.WriteLine("2-Excluir paciente");
            Console.WriteLine("3-Listar pacientes (ordenado por CPF)");
            Console.WriteLine("4-Listar pacientes (ordenado por nome)");
            Console.WriteLine("5-Voltar p/ menu principal");

            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    var choice = Int32.Parse(input);

                    if (choice < 1 || choice > 5)
                    {
                        Console.WriteLine("Opção inválida");
                        continue;
                    }
                    else
                    {
                        return (RegisterMENUOptions) choice;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Digite uma opção valida");
                    continue;
                }
            }
        }

        public enum AgendaMENUOptions
        {
            SCHEDULE = 1,
            UNSCHEDULE = 2,
            LIST = 3,
            RETURN = 4
        }
        public AgendaMENUOptions ShowAgendaMenu()
        {
            Console.WriteLine("Agenda");
            Console.WriteLine("1-Agendar consulta");
            Console.WriteLine("2-Cancelar agendamento");
            Console.WriteLine("3-Listar agenda");
            Console.WriteLine("4-Voltar p/ menu principal");

            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    var choice = Int32.Parse(input);

                    if (choice < 1 || choice > 4)
                    {
                        Console.WriteLine("Opção inválida");
                        continue;
                    }
                    else
                    {
                        return (AgendaMENUOptions) choice;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Digite uma opção valida");
                    continue;
                }
            }
        }

        public string AskCPF()
        {
            string cpf;
            do
            {
                Console.Write("CPF: ");
                cpf = Console.ReadLine();
            } while (!removevalidator.ValidateCPF(cpf));

            return cpf;
        }

        public Tuple<string, DateOnly, TimeOnly>? AskUnschedule()
        {

            string cpf;
            do
            {
                agendavalidator.ClearErrors();
                Console.Write("CPF: ");
                cpf = Console.ReadLine();
                agendavalidator.ValidateCPF(cpf);
            } while (!agendavalidator.IsValid());

            string consultationDate;
            DateOnly date;
            do
            {
                Console.Write("Data da consulta: ");
                consultationDate = Console.ReadLine();
            } while (!DateOnly.TryParseExact(consultationDate, "dd/MM/yyyy", null, DateTimeStyles.None, out date));

            string startTimeInput;
            TimeOnly startTime;
            do
            {
                Console.Write("Hora inicial: ");
                startTimeInput = Console.ReadLine();
            } while (!TimeOnly.TryParseExact(startTimeInput, "hhmm", null, DateTimeStyles.None, out startTime));

            var input = new Tuple<string, DateOnly, TimeOnly>(cpf, date, startTime);

            agendavalidator.ValidateRemove(input);
            if (agendavalidator.IsValid())
            {
                return input;
            }
            else
            {
                return null;
            }
        }

        public Tuple<string, DateOnly?, DateOnly?> ReadAgendaCriteria()
        {
            string option;
            do
            {
                Console.Write("Apresentar a agenda T-Toda ou P-Periodo: ");
                option = Console.ReadLine().ToUpper();
            } while (option != "T" && option != "P");

            if (option == "P")
            {
                string startDateInput;
                DateOnly startDate;
                do
                {
                    Console.Write("Data inicial: ");
                    startDateInput = Console.ReadLine();
                } while (!DateOnly.TryParseExact(startDateInput, "dd/MM/yyyy", null, DateTimeStyles.None, out startDate));

                string endDateInput;
                DateOnly endDate;
                do
                {
                    Console.Write("Data final: ");
                    endDateInput = Console.ReadLine();
                } while (!DateOnly.TryParseExact(endDateInput, "dd/MM/yyyy", null, DateTimeStyles.None, out endDate));

                return new Tuple<string, DateOnly?, DateOnly?>(option, startDate, endDate);
            }
            else
            {
                return new Tuple<string, DateOnly?, DateOnly?>(option, null, null);
            }
        }

        public void ListAgenda(List<Appointment> Agenda, List<Pacient> Pacients, Tuple<string, DateOnly?, DateOnly?> input)
        {
            if (input.Item1 == "P")
            {
                var agenda = Agenda.Where(a => a.Date >= input.Item2 && a.Date <= input.Item3).ToList();

                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("{0,-11} {1,5} {2,5} {3,5} {4,-21} {5,-11}", "Data", "H.Ini", "H.Fim", "Tempo", "Nome", "Dt.Nasc.");
                Console.WriteLine("-------------------------------------------------------------");

                List<DateOnly> datasListadas = new();

                foreach (var agendamento in agenda.OrderBy(a => a.Date).ThenBy(a => a.StartTime))
                {
                    var paciente = Pacients.FirstOrDefault(p => p.CPF == agendamento.CPF);

                    // Se tiver mais de um agendamento no mesmo dia, mostra a data apenas uma vez
                    var strData = agendamento.Date.ToString();
                    if (datasListadas.Contains(agendamento.Date))
                        strData = null;

                    Console.WriteLine("{0,-11} {1,5} {2,5} {3,5} {4,-21} {5,-11}",
                        strData,
                        agendamento.StartTime,
                        agendamento.EndTime,
                        agendamento.EndTime - agendamento.StartTime,
                        paciente.Name,
                        paciente.BirthDate);

                    datasListadas.Add(agendamento.Date);
                }

                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("\nPressione qualquer botão para retornar ao menu...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("{0,-11} {1,5} {2,5} {3,5} {4,-21} {5,-11}", "Data", "H.Ini", "H.Fim", "Tempo", "Nome", "Dt.Nasc.");
                Console.WriteLine("-------------------------------------------------------------");

                List<DateOnly> datasListadas = new();

                foreach (var agendamento in Agenda.OrderBy(a => a.Date).ThenBy(a => a.StartTime))
                {
                    var paciente = Pacients.FirstOrDefault(p => p.CPF == agendamento.CPF);

                    // Se tiver mais de um agendamento no mesmo dia, mostra a data apenas uma vez
                    var strData = agendamento.Date.ToString();
                    if (datasListadas.Contains(agendamento.Date))
                        strData = null;

                    Console.WriteLine("{0,-11} {1,5} {2,5} {3,5} {4,-21} {5,-11}",
                        strData,
                        agendamento.StartTime,
                        agendamento.EndTime,
                        agendamento.EndTime - agendamento.StartTime,
                        paciente.Name,
                        paciente.BirthDate);

                    datasListadas.Add(agendamento.Date);
                }

                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("\nPressione qualquer botão para retornar ao menu...");
                Console.ReadKey();
            }
        }
    }
}