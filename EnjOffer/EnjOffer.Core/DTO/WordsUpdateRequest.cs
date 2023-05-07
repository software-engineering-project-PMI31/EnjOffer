using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.ServiceContracts;

namespace EnjOffer.Core.DTO
{
    public class WordsUpdateRequest : IValidatableObject
    {
        public Guid? UserId { get; set; }
        public Guid? UserWordId { get; set; }
        public Guid? DefaultWordId { get; set; }
        public string? Word { get; set; }
        public string? WordTranslation { get; set; }

        public DateTime? LastTimeEntered { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "CorrectEnteredCountIncrease can't be less than 0")]
        public int? CorrectEnteredCount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "IncorrectEnteredCountIncrease can't be less than 0")]
        public int? IncorrectEnteredCount { get; set; }

        public bool IsIncreaseCorrectEnteredCount { get; set; }

        public bool IsIncreaseIncorrectEnteredCount { get; set; }

        public WordsResponse ToWords()
        {
            return new WordsResponse()
            {
                UserId = UserId,
                DefaultWordId = DefaultWordId,
                Word = Word,
                WordTranslation = WordTranslation,
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
