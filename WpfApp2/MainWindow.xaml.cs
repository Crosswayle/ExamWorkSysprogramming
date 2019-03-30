using Bogus;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			base.Background.Opacity = 50;
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void startProcess(User[] users, EFcontext context)
		{
			Dispatcher.BeginInvoke(new Action(() => { addToDatabaseButton.IsEnabled = false }));

			double processPercentage = (double)addingProgressBar.Maximum / Convert.ToDouble(countOfElementsTextBox.Text);

			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();

			for (int i = 0; i < Int32.Parse(countOfElementsTextBox.Text); i++)
			{
				context.Users.Add(users[i]);
				Dispatcher.BeginInvoke(new Action(() => { addingProgressBar.Value += processPercentage; }));
				Thread.Sleep(150);

			}

			context.SaveChanges();
			stopWatch.Stop();
			Dispatcher.BeginInvoke(new Action(() => { MessageBox.Show(Math.Round(stopWatch.Elapsed.TotalSeconds, 2).ToString() + " sec", "Time"); }));
			Dispatcher.BeginInvoke(new Action(() => { addingProgressBar.Value = 0; }));

			Dispatcher.BeginInvoke(new Action(() => { addToDatabaseButton.IsEnabled = true; }));
		}

		private async void Button_Click_1(object sender, RoutedEventArgs e)
		{
			using (EFcontext context = new EFcontext())
			{
				Faker generator = new Faker("en");
				User[] users = new User[Int32.Parse(countOfElementsTextBox.Text)];
				
				for(int i = 0; i < Int32.Parse(countOfElementsTextBox.Text); i ++)
				{
					users[i] = new User { Name = generator.Name.FirstName(Bogus.DataSets.Name.Gender.Male), Surname = generator.Name.LastName(Bogus.DataSets.Name.Gender.Male), Age = generator.Random.Int(18, 45)};
				}

				//Task process = new Task());
				//process.Start();
				await Task.Run(new Action(() => startProcess(users, context)));
				MessageBox.Show("Completed");
				
			}
		}
	}
}
