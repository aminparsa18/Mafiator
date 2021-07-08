using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MafiatorApp.ViewModels.Base;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MafiatorApp.Validations
{
    public class ValidatableObject<T> : ObservableObject, IValidity
    {
        private readonly List<IValidationRule<T>> _validations;
        private List<string> _errors;
        private T _value;
        private bool _isValid;

        public List<IValidationRule<T>> Validations => _validations;

        public List<string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid,value);
        }

        public ValidatableObject()
        {
            _isValid = true;
            _errors = new List<string>();
            _validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            Errors.Clear();
            var errors = _validations.Where(v => !v.Check(Value))
                                                     .Select(v => v.ValidationMessage);
            foreach (var error in errors)
            {
                Errors.Add(error);
            }

            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}
