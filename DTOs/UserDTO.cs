using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public record UserDTO(int UserId, 
        [property: Required(ErrorMessage = "אימייל הוא שדה חובה")]
        [property: EmailAddress(ErrorMessage = "פורמט אימייל לא תקין")]string UserEmail,
        [property: Required(ErrorMessage = "שם פרטי הוא שדה חובה")]
        [property: StringLength(50, MinimumLength = 2, ErrorMessage = "שם פרטי חייב להיות בין 2 ל-12 תווים")]string UserFirstName,
        [property: Required(ErrorMessage = "שם משפחה הוא שדה חובה")]
        [property: StringLength(50, MinimumLength = 2, ErrorMessage = "שם משפחה חייב להיות בין 2 ל-12 תווים")]string UserLastName);
}
