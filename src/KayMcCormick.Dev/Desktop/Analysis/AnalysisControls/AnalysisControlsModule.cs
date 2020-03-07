﻿#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// AnalysisControlsModule.cs
// 
// 2020-03-06-12:50 PM
// 
// ---
#endregion
using System.Windows.Controls ;
using Autofac ;
using KayMcCormick.Lib.Wpf ;
using ProjLib ;

namespace AnalysisControls
{
    public class AnalysisControlsModule : Module {
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            builder.RegisterType < CompilationView > ( ).AsSelf ( ) ;
            builder.RegisterType < CompilationViewModel > ( ).As < ICompilationViewModel > ( ) ;
            builder.RegisterType < ComponentViewModel > ( ).As < IComponentViewModel > ( ) ;
            builder.RegisterType < ComponentPage > ( ).As < Page > ( ) ;
            builder.RegisterAssemblyTypes ( ThisAssembly )
                   .Where (
                           type => typeof ( IView1 ).IsAssignableFrom ( type )
                                   || typeof ( IViewModel ).IsAssignableFrom ( type )
                                   || typeof(AppWindow).IsAssignableFrom(type)
                          )
                   .AsSelf ( )
                   .AsImplementedInterfaces ( ) ;
        }
        #endregion
    }
}