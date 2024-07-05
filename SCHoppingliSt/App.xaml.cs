namespace SCHoppingliSt
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();

            AppShellViewModel viewModel = new();
            MainPage = new AppShell(viewModel);
        }
    }
}
