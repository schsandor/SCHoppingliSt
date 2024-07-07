namespace SCHoppingliSt.View;

public partial class ShopPage : ContentPage
{
	public ShopPage(ShopViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ShopViewModel viewModel = (ShopViewModel)BindingContext;
        //viewModel.Title = viewModel.ShopOverview.ShopName;
        //OnPropertyChanged(viewModel.Title);
    }

    //public void ApplyQueryAttributes(IDictionary<string, object> query)
    //{
    //    ShopViewModel viewModel = (ShopViewModel)BindingContext;
    //    viewModel.ShopOverview = query["ShopOverview"] as ShopOverview;
    //    OnPropertyChanged("ShopOverview");
    //}

    //protected override void OnAppearing()
    //{
    //    MainViewModel viewModel = (MainViewModel)BindingContext;
    //    base.ApplyBindings();
    //    base.OnAppearing();


    //}
}