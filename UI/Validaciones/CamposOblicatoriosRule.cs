using System.Globalization;
using System.Windows.Controls;

namespace RegistroOrdenes.UI.Validaciones {
    public class CamposOblicatoriosRule : ValidationRule {

        public CamposOblicatoriosRule() { }

        public override ValidationResult Validate(object value , CultureInfo cultureInfo) {


            try {

                if (string.IsNullOrWhiteSpace(((string) value))) {
                    return new ValidationResult(false , $"Este campo no puede estar vació");
                }

            } catch (System.Exception e) {

                return new ValidationResult(false , $"error de formato.\n" + e.Message);
            }

            return ValidationResult.ValidResult;

        }
    }
}
