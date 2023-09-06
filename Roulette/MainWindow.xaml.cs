using Roulette.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Roulette.Models.Bet;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace Roulette {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public int[] blackNums = {
                2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35
            };
        List<int> numOrder = new List<int>();

        MyViewModel ViewModel = new MyViewModel();

        public MainWindow() {
            InitializeComponent();
            Initialize();
            //ShowWinningBets(GetWinningBets(3, blackNums, ViewModel),
            //ViewModel.WinningNumberColor,
            //ViewModel.WinningBetColor,
            //notification_panel);
            //Uncomment this to test without a client
        }
        private void Initialize() {
            CreateNumbers();
            CreateAllBets();
            ViewModel = new MyViewModel();
            DataContext = ViewModel;
            StartListening();
        }
        private void CreateAllBets() {
            CreateDozenBets();
            CreateBottomBets();
            CreateRowBets();
        }
        private void CreateNumbers() {    
            int index = 0;
            List<string> props = new List<string>() { };

            for (int row = 0; row < 3; row++) {
                for (int col = 1; col <= 12; col++) {
                    numOrder.Add( col * 3 - row);
                    index++;
                }
            }

            for (int i = 0; i < 36; i++) {
                if (blackNums.Contains(numOrder[i])) NumbersGrid.Children.Add(new RouletteNumber.BlackNum(numOrder[i]).grid);
                else NumbersGrid.Children.Add(new RouletteNumber.RedNum(numOrder[i]).grid);
            }
        }
        private void CreateDozenBets() {
            string[] bets = {"1st 12", "2nd 12", "3rd 12"};
            foreach (string bet in bets) DozenBets.Children.Add(new Bet(bet).Border);
        }
        private void CreateBottomBets() {
            string[] bets = {"1 to 18", "Even", "Black", "Red", "Odd", "19 to 36" };
            foreach (string bet in bets) {

                if (bet == "Red" || bet == "Black") BottomBets.Children.Add(new ColorBet(bet).Border);
                else BottomBets.Children.Add(new Bet(bet).Border);
            }
        }
        private void CreateRowBets() {
            for (int i = 0; i < 3; i++)  RowBets.Children.Add(new RowBet("2 to 1", i).Border);

        }
        private void ShowWinningBets(
            ObservableCollection<string> winningBets,
            SolidColorBrush winningNumberColor,
            SolidColorBrush winningBetColor,
            object notificationPanel) {
            
            ViewModel.UpdateWinningBets(winningBets, ViewModel.WinningNumberColor, winningBetColor);
            Animations.FadeAnim(notificationPanel, 10);

            int num = int.Parse(ViewModel.WinningBets[0]);
            if (num == 0) {
                Animations.AnimateZero(Zero, ViewModel.WinningBetColor.Color, 10);

                return;
            }

            int i = numOrder.IndexOf(num);
            ShowBet(NumbersGrid.Children[i], ViewModel);

            List<UIElement> bets = new List<UIElement>();

            AddBets(winningBets, NumbersGrid, bets);
            AddBets(winningBets, BottomBets, bets);
            AddBets(winningBets, DozenBets, bets);
            AddBets(winningBets, RowBets, bets);

            ShowBets(bets, ViewModel);
        }
        private void AddBets(
            ObservableCollection<string> winningBets,
            object parent,
            List<UIElement> bets) {

            foreach (string bet in winningBets) {
                UIElement element = FindElementByName((Panel)parent, bet);

                if (element != null) {
                    // If the found element is a Grid, look for the Border child inside it
                    if (element is Grid grid) {
                        Border border = null;
                        foreach (UIElement child in grid.Children) {
                            if (child is Border) {
                                border = child as Border;
                                break;
                            }
                        }

                        if (border != null) {
                            element = border;
                        }
                    }
                    bets.Add(element);
                }
            }
        }
        private UIElement FindElementByName(Panel parent, string name) {
            foreach (UIElement element in parent.Children) {
                if (element is FrameworkElement frameworkElement
                    && frameworkElement.Name == "_" + name.Replace(" ", "_")) {
                    return frameworkElement;
                }
            }

            return null;
        }

        //== TCP ==
        private async void StartListening() {
            TcpListener listener = null;
            try {
                int port = 4948;
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                Byte[] bytes = new Byte[256];
                String data = null;

                while (true) {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = await listener.AcceptTcpClientAsync();

                    await Task.Run(() => HandleClient(client)); 
                }
            }
            catch (SocketException e) {
                
            }
            finally {
                // Stop listening for new clients.
                listener.Stop();
            }


        }
        private async Task HandleClient(TcpClient client) {
            using (NetworkStream stream = client.GetStream()) {
                byte[] buffer = new byte[1024];
                int bytesRead;

                // Read the incoming data
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                // Convert the data to a JSON string
                string jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                JsonResponse response = JsonSerializer.Deserialize<JsonResponse>(jsonString);

                if (response.Qualifier == "showWinningNumber") {
                    int winningNumber = response.Data["showWinningNumber"];
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ShowWinningBets(GetWinningBets
                            (winningNumber,
                            blackNums,
                            ViewModel),
                                ViewModel.WinningNumberColor,
                                ViewModel.WinningBetColor,
                                notification_panel);                       
                    });
                }
            }

            client.Close();
        }
    }
}


