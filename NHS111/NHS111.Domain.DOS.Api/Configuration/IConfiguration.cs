﻿namespace NHS111.Domain.DOS.Api.Configuration
{
    public interface IConfiguration
    {
        string DOSIntegrationBaseUrl { get; }
        string DOSIntegrationCheckCapacitySummaryUrl { get; }
        string DOSIntegrationServiceDetailsByIdUrl { get; }
        string DOSIntegrationMonitorHealthUrl { get; }
    }
}
