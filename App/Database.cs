using App.models;
using App.ui;
using App.validate;

namespace App
{
    internal class Database
    {
        private static Database? instance;
        
        public List<Pacient> Pacients { get; private set; }
        public List<string> CPFs { get; private set; }
        public List<Appointment> Appointments { get; private set; }

        private Database()
        {
            this.Pacients = [];
            this.CPFs = [];
            this.Appointments = [];
        }

        public static Database GetInstance()
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }
    }
}
