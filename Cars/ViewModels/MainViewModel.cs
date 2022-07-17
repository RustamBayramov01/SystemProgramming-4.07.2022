using Cars.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Cars.ViewModels
{
    public class MainViewModel:ViewModelBase
    {

        CancellationTokenSource cancellationToken = new CancellationTokenSource();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        Stopwatch stopWatch = new Stopwatch();

        public Thread thread;
        string Time = string.Empty;
        private bool singler;
        private string Timerr;

        public ObservableCollection<Car> Cars { get; set; }= new ObservableCollection<Car>();


        public RelayCommand CancelCommand
        {
            get => new RelayCommand(
            () =>
            {
                if (Single) { thread.Abort(); }
                else { cancellationToken.Cancel(); }
            });
        }

        public string Timer
        {
            get => Timerr; set { Timerr = value; RaisePropertyChanged(); }
        }

        public bool Single { get => singler; set 
            { 
                singler = value; 
                RaisePropertyChanged();
            } }



      
        void Tick(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan timeSpan = stopWatch.Elapsed;
                Time = String.Format("{0:00}:{0:00}:{1:00}",
                timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
                Timer = Time;
            }
        }

 
        public void Read()
        {
            stopWatch.Start();
            dispatcherTimer.Start();

            var JsonS1 = File.ReadAllText("Mercedes.json");
            foreach (var car in JsonConvert.DeserializeObject<ObservableCollection<Car>>(JsonS1)) { dispatcher.Invoke(() => Cars.Add(car)); }

            var JsonS2 = File.ReadAllText("LandRover.json");
            foreach (var car in JsonConvert.DeserializeObject<ObservableCollection<Car>>(JsonS2)) { dispatcher.Invoke(() => Cars.Add(car)); }
            
            var JsonS3 = File.ReadAllText("Ferrari.json");
            foreach (var car in JsonConvert.DeserializeObject<ObservableCollection<Car>>(JsonS3)) { dispatcher.Invoke(() => Cars.Add(car)); }

            var JsonS4 = File.ReadAllText("Jaguar.json");
            foreach (var car in JsonConvert.DeserializeObject<ObservableCollection<Car>>(JsonS4)) { dispatcher.Invoke(() => Cars.Add(car)); }
            
            
            if (stopWatch.IsRunning)
            {
                stopWatch.Stop();
                dispatcherTimer.Stop();
            }

        }

        public RelayCommand StartCommand
        {
            get => new RelayCommand(
            () =>
            { 
                if (Single == true)
                {                    
                    thread = new Thread(Read);                    
                    thread.Start();
                }
                else
                {                     
                    var threadPool = ThreadPool.QueueUserWorkItem((e) =>
                    {                        
                        lock (Cars)
                        {
                            stopWatch.Start();
                            dispatcherTimer.Start();

                            var jsonStr = File.ReadAllText("Mercedes.json");
                            foreach (var car in JsonConvert.DeserializeObject<ObservableCollection<Car>>(jsonStr)) { dispatcher.Invoke(() => Cars.Add(car)); if (cancellationToken.IsCancellationRequested) { return; } }
                        }
                    });
                    var threadPool2 = ThreadPool.QueueUserWorkItem((e) =>
                    {
                        lock (Cars)
                        {
                            stopWatch.Start();
                            dispatcherTimer.Start();
                            var jsonStr = File.ReadAllText("LandRover.json");
                            foreach (var car in JsonConvert.DeserializeObject<ObservableCollection<Car>>(jsonStr)) { dispatcher.Invoke(() => Cars.Add(car)); if (cancellationToken.IsCancellationRequested) { return; } }
                        }
                    });
                    var threadPool3 = ThreadPool.QueueUserWorkItem((e) =>
                    {
                        lock (Cars)
                        {
                            stopWatch.Start();
                            dispatcherTimer.Start();
                            var jsonStr = File.ReadAllText("Ferrari.json");
                            foreach (var car in JsonConvert.DeserializeObject<ObservableCollection<Car>>(jsonStr)) { dispatcher.Invoke(() => Cars.Add(car)); if (cancellationToken.IsCancellationRequested) { return; } }
                        }
                    });
                    var threadPool4 = ThreadPool.QueueUserWorkItem((e) =>
                    {
                        lock (Cars)
                        {
                            stopWatch.Start();
                            dispatcherTimer.Start();
                            var jsonStr = File.ReadAllText("Jaguar.json");
                            foreach (var car in JsonConvert.DeserializeObject<ObservableCollection<Car>>(jsonStr)) { dispatcher.Invoke(() => Cars.Add(car)); if (cancellationToken.IsCancellationRequested) { return; } }
                        }
                    });
                }
            });
        }
        
    }
}
