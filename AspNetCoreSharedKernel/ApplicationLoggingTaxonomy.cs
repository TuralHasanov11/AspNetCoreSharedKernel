using Microsoft.Extensions.Compliance.Classification;

namespace AspNetCoreSharedKernel;

public static class ApplicationLoggingTaxonomy
{
    public static DataClassification EUIIDataClassification => new("EUIIDataTaxonomy", "EUIIData");

    public static DataClassification EUPDataClassification => new("EUPDataTaxonomy", "EUPData");
}

