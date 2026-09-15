using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TrendySize.Api.Models
{
    public enum MeasurementStatus
    {
        Pending,
        Submitted,
        Completed
    }

    public class Measurement
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Customer Client { get; set; } = null!;

        public string LinkToken { get; set; } = string.Empty;
        public string? OrderReference { get; set; }
        public string? ImageUrl { get; set; }

        public decimal? Chest { get; set; }
        public decimal? Waist { get; set; }
        public decimal? Hip { get; set; }
        public decimal? Shoulder { get; set; }
        public decimal? Sleeve { get; set; }
        public decimal? Neck { get; set; }
        public decimal? Height { get; set; }

        public MeasurementStatus Status { get; set; } = MeasurementStatus.Pending;
        public DateTime? AnalysedAt { get; set; }
    }

}
