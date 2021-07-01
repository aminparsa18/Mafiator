using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MafiatorApp.ViewModels.Base;
using Xamarin.Forms;

namespace MafiatorApp.Validations
{
    public class ValidatableObject<T> : ExtendedBindableObject, IValidity
    {
        private readonly List<IValidationRule<T>> _validations;
        private List<string> _errors;
        private T _value;
        private bool _isValid;

        public List<IValidationRule<T>> Validations => _validations;

        public List<string> Errors
        {
            get => _errors;
            set
            {
                _errors = value;
                RaisePropertyChanged(() => Errors);
            }
        }

        public T Value
        {
            get => _value;

            set
            {
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        public bool IsValid
        {
            get => _isValid;

            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
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
