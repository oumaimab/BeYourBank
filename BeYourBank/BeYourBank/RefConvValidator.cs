using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace BeYourBank
{
    public class RefConvValidator : ValidationRule
    {
        public override ValidationResult Validate
      (object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Ce champ ne peut être vide");
            else if (value.ToString().Length < 14)
            {
                return new ValidationResult
                    (false, "La référence de la convention est sur 14 positions");
            }
            return ValidationResult.ValidResult;
        }
    }
}
