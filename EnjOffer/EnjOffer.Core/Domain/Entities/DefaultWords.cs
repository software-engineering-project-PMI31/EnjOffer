﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.Entities
{
    public class DefaultWords
    {
        public Guid DefaultWordId { get; set; }
        public string? Word { get; set; }
        public string? WordTranslation { get; set; }
        public int Priority { get; set; }
        public string? ImageSrc { get; set; }
    }
}
