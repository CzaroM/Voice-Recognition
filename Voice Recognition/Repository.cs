using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Voice_Recognition
{
    public class Repository
    {
        SQLiteConnection connection;
        public Repository()
        {
        }

        public SQLiteConnection OpenConnection()
        {
            connection = new SQLiteConnection("Data Source=database.s3db; Version = 3; New = True; Compress = True; ");
            try
            {
                connection.Open();
            }
            catch(SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                connection = null;
            }
            return connection;
           
        }

        public bool InsertWordToDb(GrammarWord grammarWord)
        {
            bool result = true;

            using (SQLiteCommand sQLiteCommand = connection.CreateCommand())
            {
                try
                {
                    sQLiteCommand.CommandText = "INSERT INTO GrammarWords (Name, PRONUNCIATION_COUNTER) VALUES (@name,@pronunciationCounter)";
                    sQLiteCommand.Parameters.AddWithValue("@name", grammarWord.Name);
                    sQLiteCommand.Parameters.AddWithValue("@pronunciationCounter", grammarWord.Counter);
                    sQLiteCommand.ExecuteNonQuery();
                }
                catch(SQLiteException ex)
                {
                    result = false;
                    MessageBox.Show(ex.Message);
                }
               
            }

            return result;
        }

        public bool UpdateWordInDb(GrammarWord grammarWord)
        {
            bool result = true;

            using (SQLiteCommand sQLiteCommand = connection.CreateCommand())
            {
                try
                {
                    sQLiteCommand.CommandText = "UPDATE GrammarWords SET Name=@name, PRONUNCIATION_COUNTER=@pronunciationCounter WHERE Id=@id";
                    sQLiteCommand.Parameters.AddWithValue("@id", grammarWord.Id);
                    sQLiteCommand.Parameters.AddWithValue("@name", grammarWord.Name);
                    sQLiteCommand.Parameters.AddWithValue("@pronunciationCounter", grammarWord.Counter);
                    sQLiteCommand.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    result = false;
                    MessageBox.Show(ex.Message);
                }             
            }
            return result;
        }

        public List<GrammarWord> GetAllWordsFromDb()
        {
            List<GrammarWord> grammarWords = new List<GrammarWord>();

            using (SQLiteCommand sQLiteCommand = connection.CreateCommand())
            {
                try
                {
                    sQLiteCommand.CommandText = "SELECT * FROM GrammarWords";
                    SQLiteDataReader sqlite_datareader = sQLiteCommand.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        grammarWords.Add(new GrammarWord() {
                        Id = sqlite_datareader.GetInt32(0),
                        Name = sqlite_datareader.GetString(1),
                        Counter = sqlite_datareader.GetInt32(2)
                        });
                    }

                }
                catch (SQLiteException ex)
                {
                    grammarWords = null;
                    MessageBox.Show(ex.Message);
                }
            }
            return grammarWords;
        }
    }
}
