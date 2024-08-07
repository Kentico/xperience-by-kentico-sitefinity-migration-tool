﻿using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Sitefinity.Core;
/// <summary>
/// Interface for field types. Used in Dependency Injection to instantiate field types dynamically.
/// </summary>
public interface IFieldType
{
    /// <summary>
    /// Name of the Sitefinity widget type that the Field Type will be used for.
    /// </summary>
    public string SitefinityWidgetTypeName { get; }


    /// <summary>
    /// Gets name of the column type for XbyK based on Sitefinity field.
    /// </summary>
    /// <param name="sitefinityField">Sitefinity field from content type</param>
    /// <returns>XbyK name of column type</returns>
    public string GetColumnType(Field sitefinityField);


    /// <summary>
    /// Gets the column size for XbyK based on Sitefinity field.
    /// </summary>
    /// <param name="sitefinityField">Sitefinity field from content type</param>
    /// <returns>XbyK column size</returns>
    public string? GetColumnSize(Field sitefinityField);


    /// <summary>
    /// Gets the XbyK form field settings based on Sitefinity field.
    /// </summary>
    /// <param name="sitefinityField">Sitefinity field from content type</param>
    /// <returns>Form field settings to be used in XbyK field</returns>
    public FormFieldSettings GetSettings(Field sitefinityField);


    /// <summary>
    /// Handles special cases for field types that need additional processing.
    /// </summary>
    /// <param name="formField">XbyK form field</param>
    /// <param name="sitefinityField">Sitefinity field from content type</param>
    /// <returns>Processed XbyK form field</returns>
    public FormField HandleSpecialCase(FormField formField, Field sitefinityField);

    public object GetData(SdkItem sdkItem, string fieldName);
}
