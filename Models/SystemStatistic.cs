using System;
using System.Collections.Generic;

namespace GoEASy.Models;

public partial class SystemStatistic
{
    public int StatisticID { get; set; }

    public DateTime? SnapshotAt { get; set; }

    public int TotalVisits { get; set; }

    public int DeviceMobile { get; set; }

    public int DeviceDesktop { get; set; }

    public int DeviceTablet { get; set; }

    public string? TopCountry { get; set; }

    public int? TopCountryVisitCount { get; set; }

    public int? MostViewedTourID { get; set; }

    public string? MostViewedTourName { get; set; }

    public int? MostViewedCount { get; set; }

    public int? BestSellingTourID { get; set; }

    public string? BestSellingTourName { get; set; }

    public int? BestSellingCount { get; set; }

    public int? HighestRatedTourID { get; set; }

    public string? HighestRatedTourName { get; set; }

    public double? HighestRating { get; set; }

    public int? MostCancelledTourID { get; set; }

    public string? MostCancelledTourName { get; set; }

    public int? CancelledCount { get; set; }

    public decimal? TotalRevenue { get; set; }

    public int? TotalBookings { get; set; }

    public int? TotalUsers { get; set; }
}
