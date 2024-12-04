using App.validate;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace App.models
{
    public class Appointment
    {
        private static readonly AgendaValidator validator = new();

        [Key]
        public int ID { get; private set; }

        [Required]
        public string CPF { get; private set; }
        [Required]
        public DateOnly Date { get; private set; }
        [Required]
        public TimeOnly StartTime { get; private set; }
        [Required]
        public TimeOnly EndTime { get; private set; }

        public Appointment(string cpf, DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            this.CPF = cpf;
            this.Date = date;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public static Tuple<string, DateOnly, TimeOnly, TimeOnly> ReadAppointmentFields()
        {
            string cpf;
            do
            {
                Console.Write("CPF: ");
                cpf = Console.ReadLine();
                validator.ValidateCPF(cpf);
            } while (!validator.IsValid());

            // TODO: add agenda validator methods
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

            string endTimeInput;
            TimeOnly endTime;
            do
            {
                Console.Write("Hora final: ");
                endTimeInput = Console.ReadLine();
            } while (!TimeOnly.TryParseExact(endTimeInput, "hhmm", null, DateTimeStyles.None, out endTime));

            return new Tuple<string, DateOnly, TimeOnly, TimeOnly>(cpf, date, startTime, endTime);
        }



    }
}
