using System.ComponentModel.DataAnnotations;

namespace FacebookAPI.App_Code.CustomAnnotation
{
    public class IdMustBeGreaterThanZero : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is int number)
            {
                if (number > 0)
                {
                    return true;
                }
                else
                {
                    ErrorMessage = "Id must be greater than 0";
                    return false;
                }
            }
            else
            {
                throw new ArgumentException("Value must be an integer");
            }
        }
    }
}
