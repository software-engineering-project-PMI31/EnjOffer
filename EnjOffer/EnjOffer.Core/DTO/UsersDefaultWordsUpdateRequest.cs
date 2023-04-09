using EnjOffer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class UsersDefaultWordsUpdateRequest : IValidatableObject
    {
        [Required(ErrorMessage = $"{nameof(UserId)} can't be blank")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = $"{nameof(DefaultWordId)} can't be blank")]
        public Guid DefaultWordId { get; set; }

        public DateTime? LastTimeEntered { get; set; }

        [Required(ErrorMessage = $"{nameof(CorrectEnteredCount)} can't be blank")]
        [Range(0, double.MaxValue, ErrorMessage = "CorrectEnteredCountIncrease can't be less than 0")]
        public int? CorrectEnteredCount { get; set; }

        [Required(ErrorMessage = $"{nameof(IncorrectEnteredCount)} can't be blank")]
        [Range(0, double.MaxValue, ErrorMessage = "IncorrectEnteredCountIncrease can't be less than 0")]
        public int? IncorrectEnteredCount { get; set; }

        [Required(ErrorMessage = $"{nameof(IsIncreaseCorrectEnteredCount)} can't be blank")]
        public bool IsIncreaseCorrectEnteredCount { get; set; }

        [Required(ErrorMessage = $"{nameof(IsIncreaseIncorrectEnteredCount)} can't be blank")]
        public bool IsIncreaseIncorrectEnteredCount { get; set; }

        public UsersDefaultWords ToUserDefaultWords()
        {
            return new UsersDefaultWords()
            {
                UserId = UserId,
                DefaultWordId = DefaultWordId,
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
                    new[] { nameof(IsIncreaseCorrectEnteredCount) });
            }
        }
    }
}
