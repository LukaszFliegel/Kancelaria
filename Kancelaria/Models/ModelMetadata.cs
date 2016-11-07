using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kancelaria.Models
{
    internal sealed class KontrahentMetadata
    {
        [StringLength(30, ErrorMessage = "Kod kontrahenta nie moze byc dluższy niż 30 znaków.")]
        public string KodKontrahenta { get; set; }
    }
}
