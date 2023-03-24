using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultiThreadedApp
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Task _numbersTask, _lettersTask, _symbolsTask;
        private Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _numbersTask = GenerateNumbersAsync(_cancellationTokenSource.Token, (int)numbersPrioritySlider.Value);
            _lettersTask = GenerateLettersAsync(_cancellationTokenSource.Token, (int)lettersPrioritySlider.Value);
            _symbolsTask = GenerateSymbolsAsync(_cancellationTokenSource.Token, (int)symbolsPrioritySlider.Value);

            startButton.IsEnabled = false;

            stopButton.IsEnabled = true;
        }

        private async void stopButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();

            await Task.WhenAll(_numbersTask, _lettersTask, _symbolsTask);

            startButton.IsEnabled = true;

            stopButton.IsEnabled = false;
        }

        private async Task GenerateNumbersAsync(CancellationToken cancellationToken, int priority)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                int number = random.Next(100);
                string text = number.ToString();
                int delayTime = (4 - priority) * 100;
                await Task.Delay(delayTime);
                Dispatcher.Invoke(() => numbersTextBox.Text += text + " ");
            }
        }

        private async Task GenerateLettersAsync(CancellationToken cancellationToken, int priority)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                char letter = (char)random.Next('a', 'z');
                string text = letter.ToString();
                int delayTime = (4 - priority) * 100;
                await Task.Delay(delayTime);
                Dispatcher.Invoke(() => lettersTextBox.Text += text + " ");
            }
        }
        private async Task GenerateSymbolsAsync(CancellationToken token, int priority)
        {
            char[] symbols = { '#', '@', '*', '$', '%' };

            while (!token.IsCancellationRequested)
            {
                int index = random.Next(symbols.Length);
                char symbol = symbols[index];

                int delayTime = (4 - priority) * 100;
                await Task.Delay(delayTime);

                Dispatcher.Invoke(() =>
                {
                    symbolsTextBox.Text += symbol + " ";
                    symbolsTextBox.ScrollToEnd();
                });
            }
        }
    }
}


