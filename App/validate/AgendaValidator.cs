using App.models;

namespace App.validate
{
    public enum ScheduleError
    {
        
    }
    public class AgendaValidator : IValidator<ScheduleError>
    {
        private Database Database { get => Database.GetInstance(); }
        public List<string> Errors { get; set; }
        List<ScheduleError> IValidator<ScheduleError>.Errors { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AgendaValidator()
        {
            this.Errors = [];
        }

        public bool IsValid()
        {

            if (Errors.Count == 0)
            {
                return true;
            }
            else
            {
                ShowErrors();
                return false;
            }
            
        }

        public void ValidateRemove(Tuple<string, DateOnly, TimeOnly> input)
        {
            ValidateCPF(input.Item1);

            if (GetAppointment(input.Item1) == null)
            {
                Errors.Add("Erro: paciente não tem agendamento");
            }
            if (!FindAppointment(input))
            {
                Errors.Add("Erro: agendamento não encontrado");
            }
        }

        public Appointment? GetAppointment(string cpf)
        {
            return Database.Appointments.Where(a => a.CPF == cpf).FirstOrDefault();
        }

        public bool FindAppointment(Tuple<string, DateOnly, TimeOnly> input)
        {
            return Database.Appointments.Any(a => a.CPF == input.Item1 && a.Date == input.Item2 && a.StartTime == input.Item3);
        }

        public void ValidateCPF(string? cpf)
        {
            if (cpf == null)
            {
                Errors.Add("Erro: CPF nulo");
            }
            if (cpf != null && !Database.CPFs.Contains(cpf))
            {
                Errors.Add("Erro: paciente não cadastrado");
            }
        }

        private void ShowErrors()
        {
            foreach (var e in Errors)
            {
                Console.WriteLine(e);
            }
        }

        public void ClearErrors()
        {
            this.Errors.Clear();
        }
    }
}
