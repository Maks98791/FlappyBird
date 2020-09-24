using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FlappyBird.Web.Models
{
    public class PipeModel : INotifyPropertyChanged
    {
        private int _distanceFromLeft = 500;
        public int DistanceFromBottom { get; private set; } = new Random().Next(1, 60);

        public int DistanceFromLeft
        {
            get { return _distanceFromLeft; }
            set
            {
                _distanceFromLeft = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DistanceFromLeft)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Move(int speed)
        {
            DistanceFromLeft -= speed;
        }

        public bool IsCentered()
        {
            var centerMin = 500 / 2 - 60;
            var centerMax = (500 + 60) / 2;

            return (DistanceFromLeft > centerMin && DistanceFromLeft < centerMax);
        }
    }
}
