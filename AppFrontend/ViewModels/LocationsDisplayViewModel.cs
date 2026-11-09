using AppBackend;

namespace AppFrontend.ViewModels;

public class LocationsDisplayViewModel
{
    private readonly INavigationInterface _navigation;
    private readonly Model _model;
    private readonly List<Location> _locations;
    

    public LocationsDisplayViewModel(INavigationInterface navigation, Model model, IEnumerable<Location> locations) {
        this._navigation = navigation;
        this._model = model;
        this._locations = locations.ToList();
    }
}