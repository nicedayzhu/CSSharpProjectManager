using System;
using Avalonia.Data.Converters;
using System.Globalization;
using CSSharpProjectManager.ViewModels;

namespace CSSharpProjectManager.Converters;

public class TemplateDescriptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not NewProjectViewModel.TemplateType templateType)
            return "选择模板类型查看描述";
        
        return templateType switch
        {
            NewProjectViewModel.TemplateType.Default => "创建一个基本的 CounterStrikeSharp 插件模板，包含最基础的结构和功能。",
            NewProjectViewModel.TemplateType.Config => "创建一个包含配置系统的插件模板，适合需要可配置选项的插件。",
            NewProjectViewModel.TemplateType.Lang => "创建一个支持多语言的插件模板，适合需要国际化的插件。",
            NewProjectViewModel.TemplateType.ConfigLang => "创建一个同时包含配置系统和多语言支持的插件模板。",
            NewProjectViewModel.TemplateType.DataMySql => "创建一个包含MySQL数据库集成、多语言支持和配置系统的完整插件模板。",
            _ => "选择模板类型查看描述"
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}