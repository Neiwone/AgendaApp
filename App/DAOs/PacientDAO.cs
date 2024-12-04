using App.models;
using App.utils;
using App.validate;
using Microsoft.Data.Sqlite;

namespace App.DAOs
{
    public class PacienteDAO
    {
        private SqliteConnection Connection { get; set; }

        public PacienteDAO()
        {
            this.Connection = DatabaseConnection.GetInstance().Connection;
        }

        public void AddPacient(Pacient p)
        {
            string addquery = "INSERT INTO Pacients (CPF, Name, BirthDate) VALUES (@CPF, @Name, @BirthDate)";

            using var cmd = new SqliteCommand(addquery, Connection);

            cmd.Parameters.AddWithValue("@CPF", p.CPF);
            cmd.Parameters.AddWithValue("@Nome", p.Name);
            cmd.Parameters.AddWithValue("@DataNascimento", p.BirthDate);

            cmd.ExecuteNonQuery();
        }

        public List<Pacient> GetAllPacients()
        {
            string getquery = "SELECT * FROM Pacients";

            using var cmd = new SqliteCommand(getquery, Connection);

            using var reader = cmd.ExecuteReader();

            List<Pacient> pacientes = new();
            while (reader.Read())
            {
                Result<Pacient, CreatePacientError> result = Pacient.Create(
                    reader["CPF"].ToString(),
                    reader["Nome"].ToString(), 
                    reader["DataNascimento"].ToString()
                    );

                if (result.IsSuccess)
                {
                    pacientes.Add(
                        result.Value
                    );
                }
                else
                {
                    throw new Exception("Cannot get pacient from db");
                }

            }
            return pacientes;
        }

        public Pacient GetPacient(string cpf)
        {
            string getquery = "SELECT * FROM Pacientes WHERE CPF = @CPF";
            using var cmd = new SqliteCommand(getquery, Connection);

            cmd.Parameters.AddWithValue("@CPF", cpf);

            using var reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                Result<Pacient, CreatePacientError> result = Pacient.Create(
                    reader["CPF"].ToString(),
                    reader["Nome"].ToString(),
                    reader["DataNascimento"].ToString()
                    );

                if (result.IsSuccess)
                {
                    return result.Value;
                }
                else
                {
                    throw new Exception("Cannot get pacient from db");
                }
            }

            throw new Exception("Pacient not found");
        }

        public void RemovePacient(string cpf)
        {
            string deletequery = "DELETE FROM Pacients WHERE CPF = @CPF";
            using var cmd = new SqliteCommand(deletequery, Connection);

            cmd.Parameters.AddWithValue("@CPF", cpf);

            int affectedRows = cmd.ExecuteNonQuery();

            if (affectedRows == 0)
                throw new Exception("Cannot remove pacient from db");
        }
    }
}