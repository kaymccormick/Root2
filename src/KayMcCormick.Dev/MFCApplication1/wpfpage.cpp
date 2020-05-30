#include "pch.h"
#include "WPFPage.h"


using namespace System;
using namespace KmDevWpfControls;
using namespace System::Windows;
using namespace System::Windows::Documents;
using namespace System::Threading;
using namespace System::Windows::Controls;
using namespace System::Windows::Media;


void logM1(String^ message)
{

}

WPFPage::WPFPage() {}
WPFPage::WPFPage(int allottedWidth, int allotedHeight)
{
  this->Height = allotedHeight;
  this->Width = allottedWidth;
  this->Background = gcnew SolidColorBrush(Colors::Transparent);
  // auto element = gcnew AvalonDock::DockingManager();
  // auto panel = gcnew AvalonDock::Layout::LayoutPanel();
  // element->Layout->RootPanel = gcnew AvalonDock::Layout::LayoutPanel();
  // element->BottomSidePanel = gcnew AvalonDock::Controls::LayoutAnchorSideControl();

  void(__clrcall * logM)(String ^ message);
  logM = logM1;
  AnalysisControls::ViewModel::Main1Model::SelectVsInstance();
  auto con = gcnew KayMcCormick::Dev::Logging::AppLoggingConfiguration();
  con->Trace();
	
  KayMcCormick::Dev::Logging::AppLoggingConfigHelper::EnsureLoggingConfigured(gcnew KayMcCormick::Dev::Logging::LogDelegates::LogMethod(logM), con, "");
  auto app = gcnew AnalysisControls::ControlsAppInstance(gcnew KayMcCormick::Dev::Application::ApplicationInstance::ApplicationInstanceConfiguration(gcnew KayMcCormick::Dev::Application::ApplicationInstance::LogMethodDelegate(logM), Guid::NewGuid(), nullptr, false, true, true));
  app->Initialize();
  // AnalysisControls::Main1Model^ model = scope->Resolve(AnalysisControls::Main1Model::typeid);
  app->Startup();
  auto element = gcnew AnalysisControls::Main1();
  element->ViewModel = app->Main1Model;
  element->ViewModel2 = static_cast<AnalysisControls::ViewModel::Main1Mode2^>(app->Resolve  (AnalysisControls::ViewModel::Main1Mode2::typeid));
  
  this->Children->Add(element);
  // this->Children->Add(element);
 
  
}

MyPageEventArgs::MyPageEventArgs() {}

MyPageEventArgs::MyPageEventArgs(bool okClicked)
{
  IsOK = okClicked;
}
