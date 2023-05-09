using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class UserWordsUpdateRequest : IValidatableObject
    {
        [Required(ErrorMessage = "UserWordId can't be blank")]
        public Guid UserWordId { get; set; }

        public DateTime? LastTimeEntered { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = $"{nameof(CorrectEnteredCount)} can't be less than 0")]
        public int? CorrectEnteredCount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = $"{nameof(IncorrectEnteredCount)} can't be less than 0")]
        public int? IncorrectEnteredCount { get; set; }

        [Required(ErrorMessage = $"{nameof(IsIncreaseCorrectEnteredCount)} can't be blank")]
        public bool IsIncreaseCorrectEnteredCount { get; set; }

        [Required(ErrorMessage = $"{nameof(IsIncreaseIncorrectEnteredCount)} can't be blank")]
        public bool IsIncreaseIncorrectEnteredCount { get; set; }

        public UserWords ToUserWords()
        {
            return new UserWords()
            {
                UserWordId = UserWordId,
                LastTimeEntered = LastTimeEntered,
                CorrectEnteredCount = CorrectEnteredCount ?? 0,
                IncorrectEnteredCount = IncorrectEnteredCount ?? 0
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsIncreaseCorrectEnteredCount && IsIncreaseIncorrectEnteredCount)
            {
                yield return new ValidationResult("Increasing both CorrectEnteredCount" +
                    " and IncorrectEnteredCount is impossible",
                    new[] {nameof(IsIncreaseCorrectEnteredCount) });
            }
        }
    }
}
