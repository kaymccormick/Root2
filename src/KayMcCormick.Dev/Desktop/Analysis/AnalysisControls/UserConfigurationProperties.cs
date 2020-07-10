using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;
using AnalysisControls.ViewModel;
using Autofac;
using KayMcCormick.Lib.Wpf;
using Microsoft.EntityFrameworkCore;

namespace AnalysisControls
{
    public static class UserConfigurationProperties
    {
        public static readonly DependencyProperty UserSettingsDbContextProperty = DependencyProperty.RegisterAttached(
            "UserSettingsDbContext", typeof(IUserSettingsDbContext), typeof(UserConfigurationProperties),
            new PropertyMetadata(default(IUserSettingsDbContext), null, CoerceValueCallback));

        public static IUserSettingsDbContext GetUserSettingsDbContext(DependencyObject d)
        {
            return (IUserSettingsDbContext) d.GetValue(UserSettingsDbContextProperty);
        }

        public static void SetUserSettingsDbContext(DependencyObject d, IUserSettingsDbContext d1)
        {
            d.SetValue(UserSettingsDbContextProperty, d1);
        }

        public static object CoerceValueCallback(DependencyObject d, object baseValue)
        {

            var scope = (ILifetimeScope)d.GetValue(AttachedProperties.LifetimeScopeProperty);
            return scope.Resolve<IUserSettingsDbContext>();
        }


        public static readonly DependencyProperty SettingProperty = DependencyProperty.RegisterAttached(
            "Setting", typeof(object), typeof(UserConfigurationProperties),
            new PropertyMetadata(default(object), null, CoerceValueCallback2));

        public static object GetSetting(DependencyObject d)
        {
            return (object)d.GetValue(SettingProperty);
        }

        public static void SetSetting(DependencyObject d, object d1)
        {
            d.SetValue(SettingProperty, d1);
        }
        public static readonly DependencyProperty SettingValueProperty = DependencyProperty.RegisterAttached(
            "SettingValue", typeof(object), typeof(UserConfigurationProperties),
            new PropertyMetadata(default(object),  PropertyChangedCallback, CoerceValueCallback3));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IUserSettingsDbContext @object = (IUserSettingsDbContext)d.GetValue(UserSettingsDbContextProperty);
            var s = d.GetValue(SettingProperty).ToString();
            var objectUserSettings = @object.UserSettings;
            var v  = objectUserSettings.FirstOrDefault(z => z.SettingKey == s);
            v.SettingValue = e.NewValue.ToString();
        }

        private static object CoerceValueCallback3(DependencyObject d, object basevalue)
        {
            IUserSettingsDbContext @object = (IUserSettingsDbContext)d.GetValue(UserSettingsDbContextProperty);
            var s = d.GetValue(SettingProperty).ToString();
            var objectUserSettings = @object.UserSettings;
            return objectUserSettings.FirstOrDefault(z => z.SettingKey == s);

        }

        public static object GetSettingValue(DependencyObject d)
        {
            return (object)d.GetValue(SettingValueProperty);
        }

        public static void SetSettingValue(DependencyObject d, object d1)
        {
            d.SetValue(SettingValueProperty, d1);
        }

        public static object CoerceValueCallback2(DependencyObject d, object baseValue)
        {
            return GetDefaultUserSettingsDbContext(d);
        }

        public static IUserSettingsDbContext GetDefaultUserSettingsDbContext(DependencyObject d)
        {
            var @object = (IUserSettingsDbContext) d.GetValue(UserSettingsDbContextProperty);

            var scope = (ILifetimeScope) d.GetValue(AttachedProperties.LifetimeScopeProperty);
            return scope.Resolve<IUserSettingsDbContext>();
        }
    }

    public class UserSettingExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)

        {

            var ii = (IProvideValueTarget)serviceProvider.GetService(typeof(IDestinationTypeProvider));
            var name = SettingName;

            var iiTargetObject = ii.TargetObject as DependencyObject;
            var t = UserConfigurationProperties.GetUserSettingsDbContext(iiTargetObject);
            if (t == null)
            {
                t = UserConfigurationProperties.GetDefaultUserSettingsDbContext(iiTargetObject);
            }
            var ss = 
                
                t.UserSettings.FirstOrDefault(z => z.SettingKey == name);
            var v = ss?.SettingValue;
            return v;
        }

        public UserSettingExtension(string settingName)
        {
            SettingName = settingName;
        }

        public string SettingName { get; set; }
    }
}