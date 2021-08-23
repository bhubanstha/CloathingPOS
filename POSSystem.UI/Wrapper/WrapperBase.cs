using POSSystem.UI.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace POSSystem.UI.Wrapper
{
    public class WrapperBase<T> : ViewModelBase
    {
        public T Model { get; set; }

        public WrapperBase(T obj)
        {
            Model = obj;
        }

        protected virtual TValue GetValue<TValue>([CallerMemberName] string propertyName=null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }

        protected virtual void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName=null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            ValidatePropertyInternal(propertyName, value);
            OnPropertyChanged(propertyName);
        }

        public bool IsValid()
        {
            var context = new ValidationContext(Model);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(Model, context, validationResults, true);
            foreach (ValidationResult result in validationResults)
            {
                foreach (string propertyName in result.MemberNames)
                {
                    AddError(propertyName, result.ErrorMessage);
                }                
            }

            return isValid;
        }
        private void ValidatePropertyInternal<TValue>(string propertyName, TValue value)
        {
            ClearErrors(propertyName);

            //1. Validate Data Annotations
            ValidateDataAnnotations(propertyName, value);

            //2. Validate Custom Errors
            ValidateCustomErrors(propertyName);
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        private void ValidateDataAnnotations<TValue>(string propertyName, TValue value)
        {
            var context = new ValidationContext(Model) { MemberName = propertyName };
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(value, context, results);
            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}
