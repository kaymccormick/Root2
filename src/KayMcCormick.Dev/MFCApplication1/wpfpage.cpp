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
  auto app = gcnew AnalysisControls::ControlsAppInstance(gcnew KayMcCormick::Dev::Application::ApplicationInstance::ApplicationInstanceConfiguration(gcnew KayMcCormick::Dev::Application::ApplicationInstance::LogMethodDelegate(logM), Guid::NewGuid(), nullptr, false, true, true));
  app->Initialize();
  //auto scope = app->ComponentContext;
  // AnalysisControls::Main1Model^ model = scope->Resolve(AnalysisControls::Main1Model::typeid);
  app->Startup();
  auto element = gcnew AnalysisControls::Main1();
  element->ViewModel = app->Main1Model;
  this->Children->Add(element);
  // this->Children->Add(element);
 
  
}

MyPageEventArgs::MyPageEventArgs() {}

MyPageEventArgs::MyPageEventArgs(bool okClicked)
{
  IsOK = okClicked;
}
