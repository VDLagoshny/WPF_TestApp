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
			set
			{
				_mediaState = value;
				FirePropertyChanged("PlayButtonName");
			}
			get { return _mediaState; }
		}

		public string PlayButtonName
		{
			get
			{
				return (MediaState == MediaState.Playing) ? "Pause": "Play";
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void FirePropertyChanged(string name)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
		}

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
			if (MediaState == MediaState.Paused || MediaState == MediaState.Stoped)
				Play();

			
			switch (PlayButtonName)
			{
				case "Pause":
					Pause();
					break;
				case "Play":
					Play();
					break;
			}
		}

		private void btnStop_Click(object sender, RoutedEventArgs e)
		{
			if (MediaState == MediaState.Playing && media.CanPause)
				Stop();
		}

		private void slVideo_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

		}

		private void Media_Loaded(object sender, RoutedEventArgs e)
		{

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
	}
}
