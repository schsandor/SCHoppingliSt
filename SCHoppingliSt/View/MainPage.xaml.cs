namespace SCHoppingliSt.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            Trace.WriteLine("Onappearing");
            MainViewModel viewModel = (MainViewModel)BindingContext;
            Task.Run(async () =>
            {
                await viewModel.GetShops();
            });

        }

        //protected override void OnNavigatedTo(NavigatedToEventArgs args)
        //{
        //    base.OnNavigatedTo(args);
        //    Trace.WriteLine("Onnavigatedto");
        //    MainViewModel viewModel = (MainViewModel)BindingContext;
        //    Task.Run(async () =>
        //    {
        //        await viewModel.GetShops();
        //    });
        //}
    }

}
