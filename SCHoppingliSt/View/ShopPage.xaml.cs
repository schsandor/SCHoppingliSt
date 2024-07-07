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
        Task.Run(async () =>
        {
            await viewModel.GetItems(viewModel.ShopOverview.ShopName);
        });
        //viewModel.Title = viewModel.ShopOverview.ShopName;
        //OnPropertyChanged(viewModel.Title);
    }

    
}