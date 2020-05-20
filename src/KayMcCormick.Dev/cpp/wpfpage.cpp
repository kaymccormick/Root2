#include "stdafx.h"
#include "WPFPage.h"

WPFPage::WPFPage() {}
WPFPage::WPFPage(int allottedWidth, int allotedHeight)
{
  this->Height = allotedHeight;
  this->Width = allottedWidth;
  this->Background = gcnew SolidColorBrush(Colors::LightGray);
  this->Children->Add(gcnew HostedDockingManager());
 
  
}
	MyPageEventArgs::MyPageEventArgs() {}

MyPageEventArgs::MyPageEventArgs(bool okClicked)
{
  IsOK = okClicked;
}
