namespace SCHoppingliSt.View;

public partial class EditShopPage : ContentPage
{
	public EditShopPage(EditShopViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}