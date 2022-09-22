using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Music_Player
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog openFileDialog;
        private readonly MediaPlayer mediaPlayer;
        private bool isDraging = false;
        DispatcherTimer _tiker;

        public MainWindow()
        {
            _tiker = new DispatcherTimer();
            _tiker.Tick += Ticker_Tick;
            mediaPlayer = new MediaPlayer();
        }

        private void Ticker_Tick(object sender, EventArgs e)
        {
            if (!isDraging)
            {
                TimerSlider.Value = mediaPlayer.Position.TotalSeconds;
                lblCurrenttime.Text = $"{mediaPlayer.Position.Minutes}:{mediaPlayer.Position.Seconds}";

            }
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Text documents (.mp3)|*.mp3";

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            if (openFileDialog.ShowDialog() is true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));

                if (mediaPlayer.NaturalDuration.HasTimeSpan)
                    lblMusiclength.Text = $"{mediaPlayer.NaturalDuration.TimeSpan.Minutes}:{mediaPlayer.NaturalDuration.TimeSpan.Seconds}";
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (btnPlay.Visibility == Visibility.Visible)
            {
                btnStop.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Collapsed;
                mediaPlayer.Play();
                lblSongname.Text = mediaPlayer.Source.ToString().Split('/')[7];
                TimerSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                _tiker.Start();
            }
            else
            {
                mediaPlayer.Pause();
                btnStop.Visibility = Visibility.Collapsed;
                btnPlay.Visibility = Visibility.Visible;
            }
        }

        private void btnPNext_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(openFileDialog.InitialDirectory);
            var files = directoryInfo.GetFiles().Where(i => i.Extension == ".mp3").OrderBy(f => f.Name).ToArray();
            for (int i = 0;i < files.Length;i++)
            {
                if (files[i].FullName == mediaPlayer.Source.LocalPath)
                {
                    if (i < files.Length - 1)
                    {
                        mediaPlayer.Close();
                        mediaPlayer.Open(new Uri(files[i+1].FullName));
                        lblSongname.Text = files[i + 1].Name;
                        if (mediaPlayer.NaturalDuration.HasTimeSpan)
                            TimerSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        mediaPlayer.Play();
                        if (mediaPlayer.NaturalDuration.HasTimeSpan)
                            lblMusiclength.Text = $"{mediaPlayer.NaturalDuration.TimeSpan.Minutes}:{mediaPlayer.NaturalDuration.TimeSpan.Seconds}";
                        break;
                    }
                    else
                    {
                        mediaPlayer.Close();
                        mediaPlayer.Open(new Uri(files[0].FullName));
                        lblSongname.Text = mediaPlayer.Source.ToString().Split('/')[7];
                        if (mediaPlayer.NaturalDuration.HasTimeSpan)
                            TimerSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        mediaPlayer.Play();
                        if (mediaPlayer.NaturalDuration.HasTimeSpan)
                            lblMusiclength.Text = $"{mediaPlayer.NaturalDuration.TimeSpan.Minutes}:{mediaPlayer.NaturalDuration.TimeSpan.Seconds}";
                        break;
                    }
                }
            }
        }

        private void TimerSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDraging = true;
        }

        private void TimerSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDraging = false;
            mediaPlayer.Position = new TimeSpan(0, 0, 0, (int)TimerSlider.Value);
        }

        private void btnPRewind_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(openFileDialog.InitialDirectory);
            var files = directoryInfo.GetFiles().Where(i => i.Extension == ".mp3").OrderBy(f => f.Name).ToArray();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].FullName == mediaPlayer.Source.LocalPath)
                {
                    if (i > 0)
                    {
                        mediaPlayer.Close();
                        mediaPlayer.Open(new Uri(files[i - 1].FullName));
                        lblSongname.Text = files[i - 1].Name;
                        if (mediaPlayer.NaturalDuration.HasTimeSpan)
                            TimerSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        mediaPlayer.Play();
                        if (mediaPlayer.NaturalDuration.HasTimeSpan)
                            lblMusiclength.Text = $"{mediaPlayer.NaturalDuration.TimeSpan.Minutes}:{mediaPlayer.NaturalDuration.TimeSpan.Seconds}";
                        break;
                    }
                    else
                    {
                        mediaPlayer.Close();
                        mediaPlayer.Open(new Uri(files[files.Length - 1].FullName));
                        lblSongname.Text = mediaPlayer.Source.ToString().Split('/')[7];
                        if (mediaPlayer.NaturalDuration.HasTimeSpan)
                            TimerSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        mediaPlayer.Play();
                        if (mediaPlayer.NaturalDuration.HasTimeSpan)
                            lblMusiclength.Text = $"{mediaPlayer.NaturalDuration.TimeSpan.Minutes}:{mediaPlayer.NaturalDuration.TimeSpan.Seconds}";
                        break;
                    }
                }
            }
        }
    }
}
