using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_TestApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// https://www.youtube.com/watch?v=wWkw0HSWI-A&t=543s
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
		}

		private MediaState _mediaState = MediaState.Stoped;
		internal MediaState MediaState
		{
			get { return _mediaState; }
			set
			{
				_mediaState = value;
				FirePropertyChanged("PlayButtonName");
			}
		}

		public string PlayButtonName
		{
			get
			{
				return (MediaState == MediaState.Playing) ? "Pause" : "Play";
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void FirePropertyChanged(string name)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
		}

		#region Обработчик событий кнопок
		private void btnOpen_Click(object sender, RoutedEventArgs e)
		{
			var _dlg = new OpenFileDialog()
			{
				CheckFileExists = true
			};

			if (_dlg.ShowDialog(this) == true)
			{
				var _filename = _dlg.FileName;
				if (MediaState == MediaState.Paused || MediaState == MediaState.Playing)
					Stop();
				media.Source = new Uri(_filename);
				media.Play();
			}
		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			if (media.Source == null)
				return;

			if (MediaState == MediaState.Playing && media.CanPause)
				Pause();
			else if (MediaState == MediaState.Paused || MediaState == MediaState.Stoped)
				Play();
		}

		private void btnStop_Click(object sender, RoutedEventArgs e)
		{
			if (MediaState == MediaState.Playing && media.CanPause)
				Stop();
		}
		#endregion Обработчик событий кнопок

		private void slVideo_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			//Pause();
			media.Pause();
			media.Position = TimeSpan.FromSeconds(slVideo.Value);
			//Play();
			media.Play();
		}

		#region Методы работы с потоком видео
		private void Media_Loaded(object sender, RoutedEventArgs e)
		{
			media.MediaOpened += new RoutedEventHandler(media_MediaOpened);
			media.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(media_MediaFailed);
			media.MediaEnded += new RoutedEventHandler(media_MediaEnded);

			DispatcherTimer _timer = new DispatcherTimer();
			_timer.Interval = new TimeSpan(0, 0, 1);
			_timer.Tick += new EventHandler(timer_Tick);
			_timer.Start();
		}

		void timer_Tick(object sender, EventArgs e)
		{
			if (MediaState == MediaState.Playing)
			{
				slVideo.Value = media.Position.TotalSeconds; // Исправить на корректное формат времени: часы-минуты-секунды
			}
		}

		void media_MediaOpened(object sender, RoutedEventArgs e)
		{
			slVideo.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
			Play();
		}

		void media_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			MessageBox.Show("Loading video error", "Error", MessageBoxButton.OK);
		}

		void media_MediaEnded(object sender, RoutedEventArgs e)
		{
			Stop();
		}

		void Stop()
		{
			media.Stop();
			MediaState = MediaState.Stoped;
		}

		void Play()
		{
			media.Play();
			MediaState = MediaState.Playing;
		}

		void Pause()
		{
			media.Pause();
			MediaState = MediaState.Paused;
		}
		#endregion Методы работы с потоком видео
	}
}
