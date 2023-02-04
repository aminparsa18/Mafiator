using System.Globalization;
using System.Reflection;

namespace Mafiator.Game.ViewModels.Base;

public static class ViewModelLocator
{
    public static readonly BindableProperty AutoWireViewModelProperty =
        BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), false, propertyChanged: OnAutoWireViewModelChanged);

    public static bool GetAutoWireViewModel(BindableObject bindable) =>
        (bool)bindable.GetValue(AutoWireViewModelProperty);

    public static void SetAutoWireViewModel(BindableObject bindable, bool value) =>
        bindable.SetValue(AutoWireViewModelProperty, value);

    private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Element view)
        {
            return;
        }

        var viewType = view.GetType();
        var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
        var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewType.FullName?.Replace(".Views.", ".ViewModels."), viewAssemblyName);
        var viewModelType = Type.GetType(viewModelName);
        if (viewModelType == null)
            return;
        view.BindingContext = MauiProgram.Provider.GetService(viewModelType);
    }
}