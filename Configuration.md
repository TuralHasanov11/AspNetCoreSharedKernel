### Add Redaction
```cs
builder.Services.AddRedaction(options =>
{
    options.SetRedactor<ErasingRedactor>(new DataClassificationSet(ApplicationLoggingTaxonomy.EUPDataClassification));

    options.SetRedactor<SecretRedactor>(new DataClassificationSet(ApplicationLoggingTaxonomy.EUIIDataClassification));
});
```