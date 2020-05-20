#include "stdafx.h"
#include "WPFPage.h"

#include "MyLayoutPanel.h"


WPFPage::WPFPage() {}
WPFPage::WPFPage(int allottedWidth, int allotedHeight)
{
  this->Height = allotedHeight;
  this->Width = allottedWidth;
  this->Background = gcnew SolidColorBrush(Colors::Transparent);
  auto element = gcnew AvalonDock::DockingManager();
  auto panel = gcnew MyLayoutPanel();
  element->Layout->RootPanel = gcnew MyLayoutPanel();
  // element->BottomSidePanel = gcnew AvalonDock::Controls::LayoutAnchorSideControl();
	
	
	
  this->Children->Add(element);
 
  
}
	MyPageEventArgs::MyPageEventArgs() {}

MyPageEventArgs::MyPageEventArgs(bool okClicked)
{
  IsOK = okClicked;
}
