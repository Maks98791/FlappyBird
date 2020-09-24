using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FlappyBird.Web.Models
{
    public class GameManager : INotifyPropertyChanged
    {
        private readonly int _gravity = 2;
        private int _speed { get; set; } = 2;

        public event PropertyChangedEventHandler PropertyChanged;

        public BirdModel Bird { get; private set; }
        public ObservableCollection<PipeModel> Pipes { get; private set; }
        public bool IsRunning { get; private set; } = false;

        public GameManager()
        {
            Bird = new BirdModel();
            Pipes = new ObservableCollection<PipeModel>();
            Pipes.CollectionChanged += (o, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pipes)));
        }

        public async void MainLoop()
        {
            IsRunning = true;

            while (IsRunning)
            {
                Bird.Fall(_gravity);
                
                foreach(var pipe in Pipes)
                {
                    pipe.Move(_speed);
                }

                var centeredPipe = GetCenteredPipe();
                if(centeredPipe != null)
                {
                    var min = 300 - 150 + centeredPipe.DistanceFromBottom;
                    var max = 430 - 150 + centeredPipe.DistanceFromBottom - 45;

                    if (Bird.DistanceFromBottom < min || Bird.DistanceFromBottom > max)
                    {
                        GameOver();
                    }
                }

                if (!Pipes.Any() || Pipes.Last().DistanceFromLeft < 250)
                {
                    GeneratePipe();
                }

                if (Pipes.First().DistanceFromLeft < -60)
                {
                    Pipes.Remove(Pipes.First());
                }

                await Task.Delay(20); // wait 20ms
            }
        }

        public void GameOver()
        {
            IsRunning = false;
        }

        private void GeneratePipe()
        {
            Pipes.Add(new PipeModel());
        }

        private PipeModel GetCenteredPipe()
        {
            return Pipes.FirstOrDefault(p => p.IsCentered());
        }
    }
}
