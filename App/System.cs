using App.models;
using App.ui;
using App.utils;
using App.validate;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App
{
    // TODO: remove console writing of this class
    public class System
    {
        private readonly Database database;
        private readonly Menu Menu_Screens;
        public System()
        {
            this.database = Database.GetInstance();
            this.Menu_Screens = new();
        }

        public void Run()
        {
            

            while (true)
            {
                Console.Clear();
                switch (Menu_Screens.ShowMainMenu())
                {
                    case Menu.MainMENUOptions.REGISTER:
                        var repeatREGISTERMenu = true;
                        while (repeatREGISTERMenu)
                        {
                            Console.Clear();

                            // Temporary variables
                            string cpf;
                            Pacient p;

                            switch (Menu_Screens.ShowRegisterMenu())
                            {
                                
                                case Menu.RegisterMENUOptions.ADD:
                                    p = CreatePacient() ?? throw new Exception("Cannot create pacient");
                                    AddPacient(p);
                                    PacientForm.SuccessfulCreation();
                                    break;
                                case Menu.RegisterMENUOptions.REMOVE:
                                    cpf = Menu_Screens.AskCPF();
                                    p = FindPacientByCPF(cpf) ?? throw new Exception("Cannot find pacient");
                                    RemovePacient(p);
                                    break;
                                case Menu.RegisterMENUOptions.LISTBYCPF:
                                    ListarPacientes(BYCPF: true);
                                    break;
                                case Menu.RegisterMENUOptions.LISTBYNAME:
                                    ListarPacientes();
                                    break;
                                case Menu.RegisterMENUOptions.RETURN:
                                    repeatREGISTERMenu = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case Menu.MainMENUOptions.AGENDA:
                        var repeatAGENDAMenu = true;
                        while (repeatAGENDAMenu)
                        {
                            Console.Clear();

                            switch (Menu_Screens.ShowAgendaMenu())
                            {
                                case Menu.AgendaMENUOptions.SCHEDULE:
                                    AddSchedule(
                                        CreateSchedule()
                                        );
                                    break;
                                case Menu.AgendaMENUOptions.UNSCHEDULE:
                                    UnschedulePacient(
                                        Menu_Screens.AskUnschedule()
                                        );
                                    break;
                                case Menu.AgendaMENUOptions.LIST:
                                    Menu_Screens.ListAgenda(
                                        database.Appointments,
                                        database.Pacients,
                                        Menu_Screens.ReadAgendaCriteria()
                                        );
                                    break;
                                case Menu.AgendaMENUOptions.RETURN:
                                    repeatAGENDAMenu = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case Menu.MainMENUOptions.END:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
  
            }
        }

        private void UnschedulePacient(Tuple<string, DateOnly, TimeOnly>? inputdata)
        {
            if (inputdata == null)
            {
                Console.WriteLine("Não foi possível cancelar o agendamento!");
                Thread.Sleep(1000);
                return;
            }
            var agendamento = database.Appointments.Where(a => a.CPF == inputdata.Item1 && a.Date == inputdata.Item2 && a.StartTime == inputdata.Item3).FirstOrDefault();

            if (agendamento == null)
            {
                Console.WriteLine("Não foi possível cancelar o agendamento!");
                Thread.Sleep(1000);
            }
            else
            {
                database.Appointments.Remove(agendamento);

                Console.WriteLine("Agendamento cancelado com sucesso!");
                Thread.Sleep(1000);
            }

        }

        private void AddSchedule(Appointment a)
        {
            if (a != null)
            {
                database.Appointments.Add(a);

                Console.WriteLine("Paciente cadastrado com sucesso!");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("Não foi possível cadastrar o paciente!");
                Thread.Sleep(1000);
            }
        }

        private Appointment CreateSchedule()
        {
            Tuple<string, DateOnly, TimeOnly, TimeOnly> appointmentfields = Appointment.ReadAppointmentFields();

            return new Appointment(appointmentfields.Item1, appointmentfields.Item2, appointmentfields.Item3, appointmentfields.Item4);
        }

        public void ListarPacientes(bool BYCPF = false)
        {
            Func<Pacient, string> filter;
            if (BYCPF)
            {
                filter = p => p.CPF;
            }
            else
            {
                filter = p => p.Name;
            }
                

            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("CPF          Nome                            Dt.Nasc. Idade");
            Console.WriteLine("-----------------------------------------------------------");

            foreach (var p in database.Pacients.OrderBy(filter))
            {
                int age = DateTime.Now.Year - p.BirthDate.Year;

                if (DateTime.Now.Month < p.BirthDate.Month || (DateTime.Now.Month == p.BirthDate.Month && DateTime.Now.Day < p.BirthDate.Day))
                    age--;

                Console.WriteLine("{0,-11} {1,-32} {2,-10} {3,-5}",
                    p.CPF,
                    p.Name,
                    p.BirthDate.ToShortDateString(),
                    age);

                var pacientAppointments = database.Appointments.Where(a => a.CPF == p.CPF);

                if (pacientAppointments != null)
                {
                    foreach (var a in pacientAppointments)
                    {
                        Console.WriteLine($"   Agendado para: {a.Date:dd/MM/yyyy}");
                        Console.WriteLine($"   {a.StartTime:hh\\:mm} às {a.EndTime:hh\\:mm}");
                    }
                }
                
            }

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("\nPressione qualquer botão para retornar ao menu...");
            Console.ReadKey();
        }

        
        private Pacient? CreatePacient()
        {
            
            var cpf = PacientForm.AskForCPF();
            var name = PacientForm.AskForName();
            var birthDate = PacientForm.AskForBirthDate();

            // Double validation and there is nothing I can do to change that :D
            Result<Pacient, CreatePacientError> result = Pacient.Create(cpf, name, birthDate);

            if (result.IsSuccess)
            {
                return result.Value;
            }
            else
            {
                return null;
            }
        }

        private Pacient? FindPacientByCPF(string cpf)
        {
            var pacient = database.Pacients.Find(p => p.CPF == cpf);

            if (pacient != null)
            {
                return pacient;
            }
            else
            {
                Console.WriteLine("Paciente não encontrado!");
                Thread.Sleep(1000);

                return null;
            }


        }

        private void AddPacient(Pacient p)
        {
            database.Pacients.Add(p);
            database.CPFs.Add(p.CPF);
        }

        private void RemovePacient(Pacient? p)
        {
            if (p != null)
            {
                database.Pacients.Remove(p);
                database.CPFs.Remove(p.CPF);

                Console.WriteLine("Paciente excluído com sucesso!");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("Não foi possível excluir o paciente!");
                Thread.Sleep(1000);
            }

        }
    }
}