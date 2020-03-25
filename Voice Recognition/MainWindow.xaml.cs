using Microsoft.Speech.Recognition;
using System.Collections.Generic;
using System.Windows;

namespace Voice_Recognition
{
    public partial class MainWindow : Window
    {
        private SpeechRecognitionEngine recognizer = null;
        private Repository repository = null;
        private List<GrammarWord> grammarWords;
        public MainWindow()
        {
            repository = new Repository();
            repository.OpenConnection();
            InitializeComponent();
            InitWordListBinding();
        }    

        private void BtnVoiceEnable_Click(object sender, RoutedEventArgs e)
        {          
            StartRecognize();
            BtnVoiceEnable.IsEnabled = false;
            BtnVoiceDisable.IsEnabled = true;
        }


        private void BtnVoiceDisable_Click(object sender, RoutedEventArgs e)
        {
            BtnVoiceEnable.IsEnabled = true;
            BtnVoiceDisable.IsEnabled = false;
            recognizer.Dispose();
        }

        private void StartRecognize()
        {
            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("pl-PL"));
            LoadWordsToGrammar();
            recognizer.SpeechRecognized += _recognizer_SpeechRecognized; 
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple); 

        }

        private void LoadWordsToGrammar()
        {
            for(int i = 0; i < grammarWords.Count; i++)
            {
                recognizer.LoadGrammar(new Grammar(new GrammarBuilder(grammarWords[i].Name)));
            }
        }

         private void _recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
         {
            for(int i = 0; i < grammarWords.Count; i++)
            {
                if (e.Result.Text == grammarWords[i].Name)
                {
                    grammarWords[i].Counter++;
                    repository.UpdateWordInDb(grammarWords[i]);
                    break;
                }
            }
            InitWordListBinding();
         }
      
        private void InitWordListBinding()
        {
            grammarWords = repository.GetAllWordsFromDb();
            lstWords.ItemsSource = grammarWords;
        }

        private void BtnAddWordToDb_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNewWord.Text))
            {
                repository.InsertWordToDb(new GrammarWord()
                {
                    Id = 0,
                    Name = txtNewWord.Text,
                    Counter=0
                });
            }
            InitWordListBinding();
        }
    }
}
