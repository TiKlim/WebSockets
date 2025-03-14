using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AvaloniaApplication2.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApplication2.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ClientWebSocketHandler clientWebSocketHandler = new();
        private string currentMessage = String.Empty;

        public string CurrentMessage
        {
            get { return currentMessage;}
            set
            {
                currentMessage = value;
                OnPropertyChanged(nameof(CurrentMessage));
            }
        }

        public ObservableCollection<string> Messages { get; set; } = new();

        public MainWindowViewModel()
        {
            clientWebSocketHandler.OnReceiveMessage += DisplayMessage;
        }

        public async Task SendMessages(string message)
        {
            Messages.Add(message);
            await clientWebSocketHandler.SendMessages(message);
            CurrentMessage = String.Empty;
        }

        public void DisplayMessage(string message)
        {
            Messages.Add(message);
        }
    }
}
